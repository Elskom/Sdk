// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    public class Decompiler
    {
        private static Stack<Branch> m_backup;
        private readonly int registers;
        private readonly int length;
        private readonly Upvalues upvalues;
        private readonly LFunction[] functions;
        private readonly int m_params;
        private readonly int vararg;
        private readonly Op tForTarget;
        private Registers r;
        private Block outer;
        private List<Block> blocks;
        private bool[] skip;
        private bool[] reverseTarget;

        public Code Code { get; private set; }
        public Declaration[] DeclList { get; private set; }

        // TODO: Pick better names
        protected Function F { get; set; }
        protected LFunction Function { get; set; }

        private int BreakTarget(int line)
        {
            var tLine = int.MaxValue;
            foreach (var block in this.blocks)
            {
                if (block.Breakable && block.Contains(line))
                    tLine = Math.Min(tLine, block.End);
            }

            if (tLine == int.MaxValue)
                return -1;

            return tLine;
        }

        public void Decompile()
        {
            this.r = new Registers(this.registers, this.length, this.DeclList, this.F);
            this.FindReverseTargets();
            this.HandleBranches(true);
            this.outer = this.HandleBranches(false);
            this.ProcessSequence(1, this.length);
        }

        private Block EnclosingBlock(int line)
        {
            //Assumes the outer block is first
            var outer = this.blocks[0];
            var enclosing = outer;
            for (var i = 1; i < this.blocks.Count; i++)
            {
                var next = this.blocks[i];
                if (next.IsContainer && enclosing.Contains(next) && next.Contains(line) && !next.LoopRedirectAdjustment)
                    enclosing = next;
            }

            return enclosing;
        }

        private Block EnclosingBlock(Block block)
        {
            //Assumes the outer block is first
            var outer = this.blocks[0];
            var enclosing = outer;
            for (var i = 1; i < this.blocks.Count; i++)
            {
                var next = this.blocks[i];
                if (next == block)
                    continue;

                if (next.Contains(block) && enclosing.Contains(next))
                    enclosing = next;
            }

            return enclosing;
        }

        private Block EnclosingBreakableBlock(int line)
        {
            var outer = this.blocks[0];
            var enclosing = outer;
            for (var i = 1; i < this.blocks.Count; i++)
            {
                var next = this.blocks[i];
                if (next.Contains(line) && enclosing.Contains(next) && next.Breakable && !next.LoopRedirectAdjustment)
                    enclosing = next;
            }

            return enclosing == outer ? null : enclosing;
        }

        private Block EnclosingUnprotectedBlock(int line)
        {
            //Assumes the outer block is first
            var outer = this.blocks[0];
            var enclosing = outer;
            for (var i = 1; i < this.blocks.Count; i++)
            {
                var next = this.blocks[i];
                if (next.Contains(line) && enclosing.Contains(next) && next.IsUnprotected && !next.LoopRedirectAdjustment)
                    enclosing = next;
            }

            return enclosing == outer ? null : enclosing;
        }

        private void FindReverseTargets()
        {
            this.reverseTarget = new bool[this.length + 1];
            for (var line = 1; line <= this.length; line++)
            {
                var sBx = this.Code.sBx(line);
                if (this.Code.Op(line) == Op.JMP && sBx < 0)
                    this.reverseTarget[line + 1 + sBx] = true;
            }
        }

        private int GetAssignment(int line)
        {
            switch (this.Code.Op(line))
            {
                case Op.MOVE:
                case Op.LOADK:
                case Op.LOADBOOL:
                case Op.GETUPVAL:
                case Op.GETTABUP:
                case Op.GETGLOBAL:
                case Op.GETTABLE:
                case Op.NEWTABLE:
                case Op.ADD:
                case Op.SUB:
                case Op.MUL:
                case Op.DIV:
                case Op.MOD:
                case Op.POW:
                case Op.UNM:
                case Op.NOT:
                case Op.LEN:
                case Op.CONCAT:
                case Op.CLOSURE:
                    return this.Code.A(line);
                case Op.LOADNIL:
                    return (this.Code.A(line) == this.Code.B(line)) ? this.Code.A(line) : -1;
                case Op.SETGLOBAL:
                case Op.SETUPVAL:
                case Op.SETTABUP:
                case Op.SETTABLE:
                case Op.JMP:
                case Op.TAILCALL:
                case Op.RETURN:
                case Op.FORLOOP:
                case Op.FORPREP:
                case Op.TFORCALL:
                case Op.TFORLOOP:
                case Op.CLOSE:
                    return -1;
                case Op.SELF:
                    return -1;
                case Op.EQ:
                case Op.LT:
                case Op.LE:
                case Op.TEST:
                case Op.TESTSET:
                case Op.SETLIST:
                    return -1;
                case Op.CALL:
                    return (this.Code.C(line) == 2) ? this.Code.A(line) : -1;
                case Op.VARARG:
                    return (this.Code.C(line) == 2) ? this.Code.B(line) : -1;
                default:
                    throw new InvalidOperationException("Illegal opcode: " + this.Code.Op(line));
            }
        }

        private Target GetMoveIntoTargetTarget(int line, int previous)
        {
            switch (this.Code.Op(line))
            {
                case Op.MOVE:
                    return this.r.GetTarget(this.Code.A(line), line);
                case Op.SETUPVAL:
                    return new UpvalueTarget(this.upvalues.GetName(this.Code.B(line)));
                case Op.SETGLOBAL:
                    return new GlobalTarget(this.F.GetGlobalName(this.Code.Bx(line)));
                case Op.SETTABLE:
                    return new TableTarget(
                        this.r.GetExpression(this.Code.A(line), previous),
                        this.r.GetKExpression(this.Code.B(line), previous));
                default:
                    throw new InvalidOperationException();
            }
        }

        private Expression GetMoveIntoTargetValue(int line, int previous)
        {
            var A = this.Code.A(line);
            var B = this.Code.B(line);
            var C = this.Code.C(line);
            switch (this.Code.Op(line))
            {
                case Op.MOVE:
                    return this.r.GetValue(B, previous);
                case Op.SETUPVAL:
                case Op.SETGLOBAL:
                    return this.r.GetExpression(A, previous);
                case Op.SETTABLE:
                {
                    if ((C & 0x100) != 0)
                        throw new InvalidOperationException();

                    return this.r.GetExpression(C, previous);
                }
                default:
                    throw new InvalidOperationException();
            }
        }

        // TODO: Optimize / rewrite method
        private OuterBlock HandleBranches(bool first)
        {
            var oldBlocks = this.blocks;
            this.blocks = new List<Block>();
            var outer = new OuterBlock(this.Function, this.length);
            this.blocks.Add(outer);
            var isBreak = new bool[this.length + 1];
            var loopRemoved = new bool[this.length + 1];
            if (!first)
            {
                foreach (var block in oldBlocks)
                {
                    if (block is AlwaysLoop)
                        this.blocks.Add(block);
                    if (block is Break)
                    {
                        this.blocks.Add(block);
                        isBreak[block.Begin] = true;
                    }
                }

                var delete = new LinkedList<Block>();
                foreach (var block in this.blocks)
                {
                    if (block is AlwaysLoop)
                    {
                        foreach (var block2 in this.blocks)
                        {
                            if (block != block2 && block.Begin == block2.Begin)
                            {
                                if (block.End < block2.End)
                                {
                                    delete.AddLast(block);
                                    loopRemoved[block.End - 1] = true;
                                }
                                else
                                {
                                    delete.AddLast(block2);
                                    loopRemoved[block2.End - 1] = true;
                                }
                            }
                        }
                    }
                }

                foreach (var block in delete)
                    this.blocks.Remove(block);
            }

            this.skip = new bool[this.length + 1];
            var stack = new Stack<Branch>();
            var reduce = false;
            var testSet = false;
            var testSetEnd = -1;
            for (var line = 1; line <= this.length; line++)
            {
                if (!this.skip[line])
                {
                    var A   = this.Code.A(line);
                    var B   = this.Code.B(line);
                    var C   = this.Code.C(line);
                    var Bx  = this.Code.Bx(line);
                    var sBx = this.Code.sBx(line);
                    var op = this.Code.Op(line);
                    switch (op)
                    {
                        case Op.EQ:
                        case Op.LT:
                        case Op.LE:
                        {
                            Branch node = null;
                            var invert = A != 0;
                            var begin = line + 2;
                            var end = begin + this.Code.sBx(line + 1);
                            switch (op)
                            {
                                case Op.EQ:
                                    node = new EQNode(B, C, invert, line, begin, end);
                                    break;
                                case Op.LT:
                                    node = new LTNode(B, C, invert, line, begin, end);
                                    break;
                                case Op.LE:
                                    node = new LENode(B, C, invert, line, begin, end);
                                    break;
                            }

                            stack.Push(node);
                            this.skip[line + 1] = true;
                            // TODO: Add code description
                            // Not quite sure what the purpose of this is,
                            // but it seems to be related to conditionals
                            for (var k = 0; k <= 1; k++)
                            {
                                var nend = node.End - k;
                                if (this.Code.Op(nend) == Op.LOADBOOL)
                                {
                                    if (this.Code.C(nend) != 0)
                                    {
                                        node.IsCompareSet = true;
                                        node.SetTarget = this.Code.A(node.End);
                                        // Done
                                        break;
                                    }

                                    // Try next one
                                    continue;
                                }

                                // No action taken
                                break;
                            }
                        }
                            continue;
                        case Op.TEST:
                        {
                            var invert = C != 0;
                            var begin = line + 2;
                            var end = begin + this.Code.sBx(line + 1);
                            stack.Push(new TestNode(A, invert, line, begin, end));
                            this.skip[line + 1] = true;
                        }
                            continue;
                        case Op.TESTSET:
                        {
                            var invert = C != 0;
                            var begin = line + 2;
                            var end = begin + this.Code.sBx(line + 1);
                            testSet = true;
                            testSetEnd = end;
                            stack.Push(new TestSetNode(A, B, invert, line, begin, end));
                            this.skip[line + 1] = true;
                        }
                            continue;
                        case Op.JMP:
                        {
                            reduce = true;
                            var tLine = line + 1 + this.Code.sBx(line);
                            if (tLine >= 2 &&
                                this.Code.Op(tLine - 1) == Op.LOADBOOL &&
                                this.Code.C(tLine - 1) != 0)
                            {
                                stack.Push(new TrueNode(
                                    this.Code.A(tLine - 1),
                                    false,
                                    line,
                                    line + 1,
                                    tLine));
                                this.skip[line + 1] = true;
                            }
                            else if (this.Code.Op(tLine) == this.tForTarget && !this.skip[tLine])
                            {
                                var tReg = this.Code.A(tLine);
                                var tLength = this.Code.C(tLine);
                                if (tLength == 0)
                                    throw new InvalidOperationException();

                                var blockBegin = line + 1;
                                var blockEnd = tLine + 2;
                                for (var k = 0; k < 3; k++)
                                    this.r.SetInternalLoopVariable(tReg + k, tLine, blockBegin); // TODO: end?

                                for (var index = 1; index <= tLength; index++)
                                    this.r.SetExplicitLoopVariable(tReg + 2 + index, line, blockEnd); // TODO: end?

                                this.skip[tLine] = true;
                                this.skip[tLine + 1] = true;
                                this.blocks.Add(new TForBlock(this.Function, blockBegin, blockEnd, tReg, tLength,
                                    this.r));
                            }
                            else if (this.Code.sBx(line) == 2 && this.Code.Op(line + 1) == Op.LOADBOOL &&
                                     this.Code.C(line + 1) != 0)
                            {
                                /* This is the tail of a boolean set with a compare node and assign node */
                                this.blocks.Add(new BooleanIndicator(this.Function, line));
                            }
                            else if (first || loopRemoved[line])
                            {
                                if (tLine > line)
                                {
                                    isBreak[line] = true;
                                    this.blocks.Add(new Break(this.Function, line, tLine));
                                }
                                else
                                {
                                    var enclosing = this.EnclosingBreakableBlock(line);
                                    if (enclosing != null && enclosing.Breakable &&
                                        this.Code.Op(enclosing.End) == Op.JMP &&
                                        (this.Code.sBx(enclosing.End) + enclosing.End + 1 == tLine))
                                    {
                                        isBreak[line] = true;
                                        this.blocks.Add(new Break(this.Function, line, enclosing.End));
                                    }
                                    else
                                    {
                                        this.blocks.Add(new AlwaysLoop(this.Function, tLine, line + 1));
                                    }
                                }
                            }
                        }
                            break;
                        case Op.FORPREP:
                        {
                            reduce = true;
                            var target = line + 1 + sBx;
                            var close = target - 1;
                            var forBegin = line + 1;
                            var forEnd = target + 1;
                            this.blocks.Add(new ForBlock(this.Function, forBegin, forEnd, A, r));
                            this.skip[line + 1 + sBx] = true;
                            for (var k = 0; k < 3; k++)
                                this.r.SetInternalLoopVariable(A + k, forBegin - 2, forEnd - 1);

                            this.r.SetExplicitLoopVariable(A + 3, forBegin - 1, forEnd - 2);
                        }
                            break;
                        case Op.FORLOOP:
                            // Should be skipped by preceding FORPREP
                            throw new InvalidOperationException();
                        default:
                            reduce = this.IsStatement(line);
                            break;
                    }
                }

                if ((line + 1) <= this.length && this.reverseTarget[line + 1])
                    reduce = true;

                if (testSet && testSetEnd == line + 1)
                    reduce = true;

                if (stack.Count == 0)
                    reduce = false;

                if (reduce)
                {
                    reduce = false;
                    var conditions = new Stack<Branch>();
                    var backups = new Stack<Stack<Branch>>();
                    do
                    {
                        var peekNode = stack.Peek();
                        var isAssignNode = peekNode is TestSetNode;
                        var assignEnd = peekNode.End;
                        var compareCorrect = false;
                        if (peekNode is TrueNode)
                        {
                            isAssignNode = true;
                            compareCorrect = true;
                            assignEnd += (this.Code.C(assignEnd) != 0) ? 2 : 1;
                        }
                        else if (peekNode.IsCompareSet)
                        {
                            if (this.Code.Op(peekNode.Begin) != Op.LOADBOOL || this.Code.C(peekNode.Begin) == 0)
                            {
                                isAssignNode = true;
                                assignEnd += (this.Code.C(assignEnd) != 0) ? 2 : 1;
                                compareCorrect = true;
                            }
                        }
                        else if (assignEnd - 3 >= 1 &&
                            this.Code.Op(assignEnd - 2) == Op.LOADBOOL &&
                            this.Code.C(assignEnd - 2) != 0 &&
                            this.Code.Op(assignEnd - 3) == Op.JMP &&
                            this.Code.sBx(assignEnd - 3) == 2)
                        {
                            if (peekNode is TestNode)
                            {
                                var node = peekNode as TestNode;
                                if (node.Test == this.Code.A(assignEnd - 2))
                                    isAssignNode = true;
                            }
                        }
                        else if (assignEnd - 2 >= 1 &&
                            this.Code.Op(assignEnd - 1) == Op.LOADBOOL &&
                            this.Code.C(assignEnd - 1) != 0 &&
                            this.Code.Op(assignEnd - 2) == Op.JMP &&
                            this.Code.sBx(assignEnd - 2) == 2)
                        {
                            if (peekNode is TestNode)
                            {
                                isAssignNode = true;
                                assignEnd += 1;
                            }
                        }
                        else if (assignEnd - 1 >= 1)
                        {
                            if (this.Code.Op(assignEnd) == Op.LOADBOOL &&
                                this.Code.C(assignEnd) != 0 &&
                                this.Code.Op(assignEnd - 1) == Op.JMP &&
                                this.Code.sBx(assignEnd - 1) == 2)
                            {
                                if (peekNode is TestNode)
                                {
                                    isAssignNode = true;
                                    assignEnd += 2;
                                }
                            }
                            else if (this.r.IsLocal(this.GetAssignment(assignEnd - 1), assignEnd - 1) &&
                                assignEnd > peekNode.Line)
                            {
                                var decl = this.r.GetDeclaration(this.GetAssignment(assignEnd - 1), assignEnd - 1);
                                if (decl.Begin == assignEnd - 1 && decl.End > assignEnd - 1)
                                    isAssignNode = true;
                            }
                        }

                        if (!compareCorrect &&
                            assignEnd - 1 == peekNode.Begin &&
                            this.Code.Op(peekNode.Begin) == Op.LOADBOOL &&
                            this.Code.C(peekNode.Begin) != 0)
                        {
                            m_backup = null;
                            var begin = peekNode.Begin;
                            var target = this.Code.A(begin);
                            assignEnd = begin + 2;
                            var condition = this.PopCompareSetCondition(stack, assignEnd, target);
                            condition.SetTarget = target;
                            condition.End = assignEnd;
                            condition.Begin = begin;
                            conditions.Push(condition);
                        }
                        else if (isAssignNode)
                        {
                            m_backup = null;
                            var begin = peekNode.Begin;
                            var target = peekNode.SetTarget;
                            var condition = this.PopSetCondition(stack, assignEnd, target);
                            condition.SetTarget = target;
                            condition.End = assignEnd;
                            condition.Begin = begin;
                            conditions.Push(condition);
                        }
                        else
                        {
                            m_backup = new Stack<Branch>();
                            conditions.Push(this.PopCondition(stack));
                            m_backup.Reverse();
                        }

                        backups.Push(m_backup);
                    } while (stack.Count > 0);

                    do
                    {
                        var cond = conditions.Pop();
                        var backup = backups.Pop();
                        var breakTarget = this.BreakTarget(cond.Begin);
                        var breakable = (breakTarget >= 1);
                        if (breakable && this.Code.Op(breakTarget) == Op.JMP)
                            breakTarget += (1 + this.Code.sBx(breakTarget));
                        if (breakable && breakTarget == cond.End)
                        {
                            var immediateEnclosing = EnclosingBlock(cond.Begin);
                            var breakableEnclosing = this.EnclosingBreakableBlock(cond.Begin);
                            var loopStart = immediateEnclosing.End;
                            if (immediateEnclosing == breakableEnclosing)
                                --loopStart;

                            for (var iline = loopStart; iline >= Math.Max(cond.Begin, immediateEnclosing.Begin); iline--)
                            {
                                var op = this.Code.Op(iline);
                                var target = iline + 1 + this.Code.sBx(iline);
                                if (op == Op.JMP && target == breakTarget)
                                {
                                    cond.End = iline;
                                    break;
                                }
                            }
                        }

                        /* A branch has a tail if the instruction just before the end target is JMP */
                        var hasTail = cond.End >= 2 && this.Code.Op(cond.End - 1) == Op.JMP;

                        /* This is the target of the tail JMP */
                        var tail = hasTail ? cond.End + this.Code.sBx(cond.End - 1) : -1;
                        var originalTail = tail;
                        var enclosing = this.EnclosingUnprotectedBlock(cond.Begin);
                        var breakEnclosing = this.EnclosingBreakableBlock(cond.Begin);
                        var hasScopeIssues = false;

                        /* Checking enclosing unprotected block to undo JMP redirects. */
                        if (enclosing != null)
                        {
                            if (enclosing.GetLoopback() == cond.End)
                            {
                                cond.End = enclosing.End - 1;
                                hasTail = cond.End >= 2 && this.Code.Op(cond.End - 1) == Op.JMP;
                                tail = hasTail ? cond.End + this.Code.sBx(cond.End - 1) : -1;
                            }

                            if (hasTail && enclosing.GetLoopback() == tail)
                                tail = enclosing.End - 1;
                        }/* !!!HACK ALERT!!! */
                        else if (hasTail && breakEnclosing != null && (tail >= breakEnclosing.ScopeEnd))
                        {
                            // HACK: fix scope issues
                            // this is VERY hack-ish, but it works!
                            var scopeIsBad = this.Code.Op(breakEnclosing.ScopeEnd) == Op.JMP && (this.Code.sBx(breakEnclosing.ScopeEnd) + 1) == tail;

                            // before, 'else' statements would be misinterpreted as 'break' statements
                            // thankfully, 'else' statements can be found using this method
                            var isElse = ((cond.End + this.Code.sBx(cond.End - 1) - 1) == breakEnclosing.ScopeEnd);
                            if (scopeIsBad && !isElse && !isBreak[tail - 1])
                            {
                                hasScopeIssues = true;
                                tail = breakEnclosing.ScopeEnd;
                            }
                        }

                        if (cond.IsSet)
                        {
                            var empty = cond.Begin == cond.End;
                            if (this.Code.Op(cond.Begin) == Op.JMP && this.Code.sBx(cond.Begin) == 2 &&
                                this.Code.Op(cond.Begin + 1) == Op.LOADBOOL && this.Code.C(cond.Begin + 1) != 0)
                            {
                                empty = true;
                            }

                            this.blocks.Add(new SetBlock(this.Function, cond, cond.SetTarget, line, cond.Begin, cond.End, empty, this.r));
                        }
                        else if (this.Code.Op(cond.Begin) == Op.LOADBOOL && this.Code.C(cond.Begin) != 0)
                        {
                            var begin = cond.Begin;
                            var target = this.Code.A(begin);
                            if (this.Code.B(begin) == 0)
                                cond = cond.Invert();

                            this.blocks.Add(new CompareBlock(this.Function, begin, begin + 2, target, cond));
                        }
                        else if (cond.End < cond.Begin)
                        {
                            this.blocks.Add(new RepeatBlock(this.Function, cond, this.r));
                        }
                        else if (hasTail)
                        {
                            var endOp = this.Code.Op(cond.End - 2);
                            var isEndCondJump = endOp == Op.EQ || endOp == Op.LE || endOp == Op.LT || endOp == Op.TEST || endOp == Op.TESTSET;
                            if (tail > cond.End || (tail == cond.End && !isEndCondJump))
                            {
                                var op = this.Code.Op(tail - 1);
                                var sbx = this.Code.sBx(tail - 1);
                                var loopback2 = tail + sbx;
                                var isBreakableLoopEnd = this.Function.Header.Version.IsBreakableLoopEnd(op);
                                var isElse = (this.Code.Op(cond.Begin - 1) == Op.JMP && (cond.Begin + this.Code.sBx(cond.Begin - 1)) >= cond.End);

                                // --- clean check -------- hacky check ----------------------------
                                if ((isBreakableLoopEnd || (breakEnclosing != null && hasScopeIssues)) && loopback2 <= cond.Begin && !isBreak[tail - 1])
                                {
                                    /* (ends with break) */
                                    this.blocks.Add(new IfThenEndBlock(this.Function, cond, backup, this.r));
                                }
                                else
                                {
                                    this.skip[cond.End - 1] = true; // Skip the JMP over the else block
                                    var emptyElse = tail == cond.End;
                                    this.blocks.Add(new IfThenElseBlock(this.Function, cond, originalTail, emptyElse, this.r));
                                    if (!emptyElse)
                                        this.blocks.Add(new ElseEndBlock(this.Function, cond.End, tail));

                                }
                            }
                            else
                            {
                                var loopback = tail;
                                var existsStatement = false;
                                for (var sl = loopback; sl < cond.Begin; sl++)
                                {
                                    if (!this.skip[sl] && this.IsStatement(sl))
                                    {
                                        existsStatement = true;
                                        break;
                                    }
                                }

                                //TODO: check for 5.2-style if cond then break end
                                if (loopback >= cond.Begin || existsStatement)
                                {
                                    this.blocks.Add(new IfThenEndBlock(this.Function, cond, backup, this.r));
                                }
                                else
                                {
                                    this.skip[cond.End - 1] = true;
                                    this.blocks.Add(new WhileBlock(this.Function, cond, originalTail, this.r));
                                }
                            }
                        }
                        else
                        {
                            this.blocks.Add(new IfThenEndBlock(this.Function, cond, backup, this.r));
                        }

                    } while (conditions.Count > 0);
                }
            }

            //Find variables whose scope isn't controlled by existing blocks:
            foreach (var decl in this.DeclList)
            {
                if (!decl.ForLoop && !decl.ForLoopExplicit)
                {
                    var needsDoEnd = true;
                    foreach (var block in this.blocks)
                    {
                        /* !!!HACK ALERT!!! */
                        // scope fix should have fixed this problem
                        if (block.Contains(decl.Begin) /*&& (block.ScopeEnd >= decl.End)*/)
                        {
                            needsDoEnd = false;
                            break;
                        }
                    }

                    if (needsDoEnd)
                    {
                        //Without accounting for the order of declarations, we might
                        //create another do..end block later that would eliminate the
                        //need for this one. But order of decls should fix this.
                        this.blocks.Add(new DoEndBlock(this.Function, decl.Begin, decl.End + 1));
                    }
                }
            }

            var newBlocks = new List<Block>();
            for (var b = 0; b < this.blocks.Count; b++)
            {
                var block = this.blocks[b];

                // Remove breaks that were later parsed as else jumps
                if (this.skip[block.Begin])
                {
                    if (block is Break)
                        continue;
                }

                newBlocks.Add(block);
            }

            newBlocks.Sort();
            this.blocks = newBlocks;
            m_backup = null;
            return outer;
        }

        private void HandleInitialDeclares(Output output)
        {
            var initDecls = new List<Declaration>(this.DeclList.Length);
            for (var i = this.m_params + (this.vararg & 1); i < this.DeclList.Length; i++)
            {
                var decl = this.DeclList[i];
                if (decl.Begin == 0)
                    initDecls.Add(decl);
            }

            if (initDecls.Count > 0)
            {
                output.Print("local ");
                output.Print(initDecls[0].Name);
                for (var i = 1; i < initDecls.Count; i++)
                {
                    output.Print(", ");
                    output.Print(initDecls[i].Name);
                }

                output.PrintLine();
            }
        }

        private bool IsMoveIntoTarget(int line)
        {
            switch (this.Code.Op(line))
            {
                case Op.MOVE:
                    return this.r.IsAssignable(this.Code.A(line), line) &&
                           !this.r.IsLocal(this.Code.B(line), line);
                case Op.SETUPVAL:
                case Op.SETGLOBAL:
                    return !this.r.IsLocal(this.Code.A(line), line);
                case Op.SETTABLE:
                {
                    var c = this.Code.C(line);
                    return (c & 0x100) != 0 ? false : !this.r.IsLocal(c, line);
                }
                default:
                    return false;
            }
        }

        private bool IsStatement(int line)
            => this.IsStatement(line, -1);

        private bool IsStatement(int line, int testRegister)
        {
            switch (this.Code.Op(line))
            {
                case Op.MOVE:
                case Op.LOADK:
                case Op.LOADBOOL:
                case Op.GETUPVAL:
                case Op.GETTABUP:
                case Op.GETGLOBAL:
                case Op.GETTABLE:
                case Op.NEWTABLE:
                case Op.ADD:
                case Op.SUB:
                case Op.MUL:
                case Op.DIV:
                case Op.MOD:
                case Op.POW:
                case Op.UNM:
                case Op.NOT:
                case Op.LEN:
                case Op.CONCAT:
                case Op.CLOSURE:
                    return this.r.IsLocal(this.Code.A(line), line) || this.Code.A(line) == testRegister;
                case Op.LOADNIL:
                {
                    for (var register = this.Code.A(line); register <= this.Code.B(line); register++)
                    {
                        if (this.r.IsLocal(register, line))
                            return true;
                    }

                    return false;
                }
                case Op.SETGLOBAL:
                case Op.SETUPVAL:
                case Op.SETTABUP:
                case Op.SETTABLE:
                case Op.JMP:
                case Op.TAILCALL:
                case Op.RETURN:
                case Op.FORLOOP:
                case Op.FORPREP:
                case Op.TFORCALL:
                case Op.TFORLOOP:
                case Op.CLOSE:
                    return true;
                case Op.SELF:
                {
                    var a = this.Code.A(line);
                    return this.r.IsLocal(a, line) || this.r.IsLocal(a + 1, line);
                }
                case Op.EQ:
                case Op.LT:
                case Op.LE:
                case Op.TEST:
                case Op.TESTSET:
                case Op.SETLIST:
                    return false;
                case Op.CALL:
                {
                    var a = this.Code.A(line);
                    var c = this.Code.C(line);
                    if (c == 1)
                        return true;

                    if (c == 0)
                        c = this.registers - a + 1;

                    for (var register = a; register < a + c - 1; register++)
                    {
                        if (this.r.IsLocal(register, line))
                            return true;
                    }

                    return (c == 2 && a == testRegister);
                }
                case Op.VARARG:
                {
                    var a = this.Code.A(line);
                    var b = this.Code.B(line);
                    if (b == 0)
                        b = this.registers - a + 1;

                    for (var register = a; register < a + b - 1; register++)
                    {
                        if (this.r.IsLocal(register, line))
                            return true;
                    }

                    return false;
                }
                default:
                    throw new InvalidOperationException("Illegal opcode: " + this.Code.Op(line));
            }
        }

        private LinkedList<Operation> ProcessLine(int line)
        {
            var operations = new LinkedList<Operation>();
            var A = this.Code.A(line);
            var B = this.Code.B(line);
            var C = this.Code.C(line);
            var Bx = this.Code.Bx(line);
            switch (this.Code.Op(line))
            {
                case Op.MOVE:
                    operations.AddLast(new RegisterSet(line, A, this.r.GetExpression(B, line)));
                    break;
                case Op.LOADK:
                    operations.AddLast(new RegisterSet(line, A, this.F.GetConstantExpression(Bx)));
                    break;
                case Op.LOADBOOL:
                {
                    var constant = new Constant((B != 0) ? LBoolean.LTRUE : LBoolean.LFALSE);
                    operations.AddLast(new RegisterSet(line, A, new ConstantExpression(constant, -1)));
                }
                    break;
                case Op.LOADNIL:
                {
                    var maximum = (this.Function.Header.Version.UsesOldLoadNilEncoding) ? B : (A + B);
                    while (A <= maximum)
                    {
                        operations.AddLast(new RegisterSet(line, A, Expression.NIL));
                        A++;
                    }
                }
                    break;
                case Op.GETUPVAL:
                    operations.AddLast(new RegisterSet(line, A, this.upvalues.GetExpression(B)));
                    break;
                case Op.GETTABUP:
                {
                    var expr = (B == 0 && (C & 0x100) != 0)
                        ? this.F.GetGlobalExpression(C & 0xFF) as Expression
                        : new TableReference(this.upvalues.GetExpression(B), this.r.GetKExpression(C, line)) as
                            Expression;
                    operations.AddLast(new RegisterSet(line, A, expr));
                }
                    break;
                case Op.GETGLOBAL:
                    operations.AddLast(new RegisterSet(line, A, this.F.GetGlobalExpression(Bx)));
                    break;
                case Op.GETTABLE:
                    operations.AddLast(new RegisterSet(line, A,
                        new TableReference(this.r.GetExpression(B, line), this.r.GetKExpression(C, line))));
                    break;
                case Op.SETUPVAL:
                    operations.AddLast(new UpvalueSet(line, this.upvalues.GetName(B), this.r.GetExpression(A, line)));
                    break;
                case Op.SETTABUP:
                {

                    if (A == 0 && (B & 0x100) != 0)
                    {
                        //TODO: check
                        operations.AddLast(new GlobalSet(line, this.F.GetGlobalName(B & 0xFF),
                            this.r.GetKExpression(C, line)));
                    }
                    else
                    {
                        operations.AddLast(new TableSet(line, this.upvalues.GetExpression(A),
                            this.r.GetKExpression(B, line), this.r.GetKExpression(C, line), true, line));
                    }

                    break;
                }

                case Op.SETGLOBAL:
                    operations.AddLast(new GlobalSet(line, this.F.GetGlobalName(Bx), this.r.GetExpression(A, line)));
                    break;
                case Op.SETTABLE:
                    operations.AddLast(new TableSet(line, this.r.GetExpression(A, line), this.r.GetKExpression(B, line),
                        this.r.GetKExpression(C, line), true, line));
                    break;
                case Op.NEWTABLE:
                    operations.AddLast(new RegisterSet(line, A, new TableLiteral(B, C)));
                    break;
                case Op.SELF:
                {
                    // We can later determine is : syntax was used by comparing subexpressions with ==
                    var common = this.r.GetExpression(B, line);
                    operations.AddLast(new RegisterSet(line, A + 1, common));
                    operations.AddLast(new RegisterSet(line, A,
                        new TableReference(common, this.r.GetKExpression(C, line))));
                }
                    break;
                case Op.ADD:
                    operations.AddLast(new RegisterSet(line, A,
                        Expression.MakeADD(this.r.GetKExpression(B, line), this.r.GetKExpression(C, line))));
                    break;
                case Op.SUB:
                    operations.AddLast(new RegisterSet(line, A,
                        Expression.MakeSUB(this.r.GetKExpression(B, line), this.r.GetKExpression(C, line))));
                    break;
                case Op.MUL:
                    operations.AddLast(new RegisterSet(line, A,
                        Expression.MakeMUL(this.r.GetKExpression(B, line), this.r.GetKExpression(C, line))));
                    break;
                case Op.DIV:
                    operations.AddLast(new RegisterSet(line, A,
                        Expression.MakeDIV(this.r.GetKExpression(B, line), this.r.GetKExpression(C, line))));
                    break;
                case Op.MOD:
                    operations.AddLast(new RegisterSet(line, A,
                        Expression.MakeMOD(this.r.GetKExpression(B, line), this.r.GetKExpression(C, line))));
                    break;
                case Op.POW:
                    operations.AddLast(new RegisterSet(line, A,
                        Expression.MakePOW(this.r.GetKExpression(B, line), this.r.GetKExpression(C, line))));
                    break;
                case Op.UNM:
                    operations.AddLast(new RegisterSet(line, A, Expression.MakeUNM(this.r.GetExpression(B, line))));
                    break;
                case Op.NOT:
                    operations.AddLast(new RegisterSet(line, A, Expression.MakeNOT(this.r.GetExpression(B, line))));
                    break;
                case Op.LEN:
                    operations.AddLast(new RegisterSet(line, A, Expression.MakeLEN(this.r.GetExpression(B, line))));
                    break;
                case Op.CONCAT:
                {
                    var value = this.r.GetExpression(C, line);

                    //Remember that CONCAT is right associative.
                    while (C-- > B)
                        value = Expression.MakeCONCAT(this.r.GetExpression(C, line), value);

                    operations.AddLast(new RegisterSet(line, A, value));
                }
                    break;
                case Op.JMP:
                case Op.EQ:
                case Op.LT:
                case Op.LE:
                case Op.TEST:
                case Op.TESTSET:
                    /* Do nothing ... handled with branches */
                    break;
                case Op.CALL:
                {
                    var multiple = (C >= 3 || C == 0);
                    if (B == 0)
                        B = this.registers - A;

                    if (C == 0)
                        C = this.registers - A + 1;

                    var function = this.r.GetExpression(A, line);
                    var arguments = new Expression[B - 1];
                    for (var register = A + 1; register <= A + B - 1; register++)
                    {
                        var expr = this.r.GetExpression(register, line);

                        arguments[register - A - 1] = expr;
                    }

                    var value = new FunctionCall(function, arguments, multiple);
                    if (C == 1)
                    {
                        operations.AddLast(new CallOperation(line, value));
                    }
                    else
                    {
                        if (C == 2 && !multiple)
                        {
                            operations.AddLast(new RegisterSet(line, A, value));
                        }
                        else
                        {
                            for (var register = A; register <= A + C - 2; register++)
                                operations.AddLast(new RegisterSet(line, register, value));
                        }
                    }
                }
                    break;
                case Op.TAILCALL:
                {
                    if (B == 0)
                        B = this.registers - A;

                    var function = this.r.GetExpression(A, line);
                    var arguments = new Expression[B - 1];
                    for (var register = A + 1; register <= A + B - 1; register++)
                        arguments[register - A - 1] = this.r.GetExpression(register, line);

                    var value = new FunctionCall(function, arguments, true);
                    operations.AddLast(new ReturnOperation(line, value));
                    this.skip[line + 1] = true;
                }
                    break;
                case Op.RETURN:
                {
                    if (B == 0)
                        B = this.registers - A + 1;

                    var values = new Expression[B - 1];
                    for (var register = A; register <= A + B - 2; register++)
                        values[register - A] = this.r.GetExpression(register, line);

                    operations.AddLast(new ReturnOperation(line, values));
                }
                    break;
                case Op.FORLOOP:
                case Op.FORPREP:
                case Op.TFORCALL:
                case Op.TFORLOOP:
                    /* Do nothing ... handled with branches */
                    break;
                case Op.SETLIST:
                {
                    if (C == 0)
                    {
                        C = this.Code.CodePoint(line + 1);
                        this.skip[line + 1] = true;
                    }

                    if (B == 0)
                        B = this.registers - A - 1;

                    var table = this.r.GetValue(A, line);
                    for (var i = 1; i <= B; i++)
                        operations.AddLast(new TableSet(line, table,
                            new ConstantExpression(new Constant((C - 1) * 50 + i), -1),
                            this.r.GetExpression(A + i, line), false, this.r.GetUpdated(A + i, line)));
                }
                    break;
                case Op.CLOSE:
                    break;
                case Op.CLOSURE:
                {
                    var f = this.functions[Bx];
                    operations.AddLast(new RegisterSet(line, A, new ClosureExpression(f, this.DeclList, line + 1)));
                    if (this.Function.Header.Version.UsesInlineUpvalueDeclaritions)
                    {
                        // Skip upvalue declarations
                        for (var i = 0; i < f.NumUpValues; i++)
                            this.skip[line + 1 + i] = true;
                    }
                }
                    break;
                case Op.VARARG:
                {
                    var multiple = (B != 2);
                    if (B == 1)
                        throw new InvalidOperationException();

                    if (B == 0)
                        B = this.registers - A + 1;

                    var value = new Vararg(B - 1, multiple);
                    for (var register = A; register <= A + B - 2; register++)
                        operations.AddLast(new RegisterSet(line, register, value));
                }
                    break;
                default:
                    throw new InvalidOperationException("Illegal instruction: " + this.Code.Op(line));
            }

            return operations;
        }

        private Assignment ProcessOperation(Operation operation, int line, int nextLine, Block block)
        {
            Assignment assign = null;
            var wasMultiple = false;
            var stmt = operation.Process(this.r, block);
            
            // TODO: Optimize code
            if (stmt != null)
            {
                if (stmt is Assignment)
                {
                    assign = stmt as Assignment;
                    if (!assign.GetFirstValue().IsMultiple)
                        block.AddStatement(stmt);
                    else
                        wasMultiple = true;
                }
                else
                {
                    block.AddStatement(stmt);
                }

                if (assign != null)
                {
                    while (nextLine < block.End && this.IsMoveIntoTarget(nextLine))
                    {
                        var target = this.GetMoveIntoTargetTarget(nextLine, line + 1);
                        var value = this.GetMoveIntoTargetValue(nextLine, line + 1); // updated?
                        assign.AddFirst(target, value);
                        this.skip[nextLine] = true;
                        nextLine++;
                    }

                    if (wasMultiple && !assign.GetFirstValue().IsMultiple)
                        block.AddStatement(stmt);
                }
            }

            return assign;
        }

        private void ProcessSequence(int begin, int end)
        {
            var blockIndex = 1;
            var blockStack = new Stack<Block>();
            blockStack.Push(this.blocks[0]);
            this.skip = new bool[end + 1];
            for (var line = begin; line <= end; line++)
            {
                Operation blockHandler = null;
                while (blockStack.Peek().End <= line)
                {
                    blockHandler = blockStack.Pop().Process(this);
                    if (blockHandler != null)
                        break;
                }

                if (blockHandler == null)
                {
                    while (blockIndex < this.blocks.Count && this.blocks[blockIndex].Begin <= line)
                        blockStack.Push(this.blocks[blockIndex++]);
                }

                var block = blockStack.Peek();
                this.r.StartLine(line); // Must occur AFTER block.rewrite (???)
                if (this.skip[line])
                {
                    var nLocals = this.r.GetNewLocals(line);
                    if (nLocals.Count > 0)
                    {
                        var a = new Assignment();
                        a.Declare(nLocals[0].Begin);
                        foreach (var decl in nLocals)
                            a.AddLast(new VariableTarget(decl), this.r.GetValue(decl.Register, line));

                        block.AddStatement(a);
                    }

                    continue;
                }

                var operations = this.ProcessLine(line);
                var newLocals = this.r.GetNewLocals(blockHandler == null ? line : line - 1);
                Assignment assign = null;
                if (blockHandler == null)
                {
                    if (this.Code.Op(line) == Op.LOADNIL)
                    {
                        assign = new Assignment();
                        var count = 0;
                        foreach (var operation in operations)
                        {
                            var set = operation as RegisterSet;
                            operation.Process(this.r, block);
                            if (this.r.IsAssignable(set.Register, set.Line))
                            {
                                assign.AddLast(this.r.GetTarget(set.Register, set.Line), set.Value);
                                count++;
                            }
                        }

                        if (count > 0)
                            block.AddStatement(assign);
                    }
                    else
                    {
                        foreach (var operation in operations)
                        {
                            var temp = this.ProcessOperation(operation, line, line + 1, block);
                            if (temp != null)
                                assign = temp;
                        }

                        if (assign != null && assign.GetFirstValue().IsMultiple)
                            block.AddStatement(assign);
                    }
                }
                else
                {
                    assign = this.ProcessOperation(blockHandler, line, line, block);
                }

                if (assign != null)
                {
                    if (newLocals.Count > 0)
                    {
                        assign.Declare(newLocals[0].Begin);
                        foreach (var decl in newLocals)
                            assign.AddLast(new VariableTarget(decl), this.r.GetValue(decl.Register, line + 1));
                    }
                }

                if (blockHandler == null)
                {
                    if (assign != null)
                    {
                        // TODO: Handle when 'blockHandler' is null and 'assign' is NOT null
                    }
                    else if (newLocals.Count > 0 && this.Code.Op(line) != Op.FORPREP)
                    {
                        if (this.Code.Op(line) != Op.JMP || this.Code.Op(line + 1 + this.Code.sBx(line)) != this.tForTarget)
                        {
                            assign = new Assignment();
                            assign.Declare(newLocals[0].Begin);
                            foreach (var decl in newLocals)
                                assign.AddLast(new VariableTarget(decl), this.r.GetValue(decl.Register, line));

                            block.AddStatement(assign);
                        }
                    }
                }
                else
                {
                    line--;
                    continue;
                }
            }
        }

        public Branch PopCompareSetCondition(Stack<Branch> stack, int assignEnd, int target)
        {
            var top = stack.Pop();
            var invert = false;
            if (this.Code.B(top.Begin) == 0)
                invert = true;

            top.Begin = assignEnd;
            top.End = assignEnd;
            stack.Push(top);

            // Invert argument doesn't matter because begin == end
            return this.PopSetConditionInternal(stack, invert, assignEnd, target);
        }

        public Branch PopCondition(Stack<Branch> stack)
        {
            var branch = stack.Pop();
            if (m_backup != null)
                m_backup.Push(branch);

            if (branch is TestSetNode)
                throw new InvalidOperationException();

            var begin = branch.Begin;
            if (this.Code.Op(branch.Begin) == Op.JMP)
                begin += (1 + this.Code.sBx(branch.Begin));

            while (stack.Count > 0)
            {
                var next = stack.Peek();
                if (next is TestSetNode)
                    break;

                if (next.End == begin)
                    branch = new OrBranch(this.PopCondition(stack).Invert(), branch);
                else if (next.End == branch.End)
                    branch = new AndBranch(this.PopCondition(stack), branch);
                else
                    break;
            }

            return branch;
        }

        public Branch PopSetCondition(Stack<Branch> stack, int assignEnd, int target)
        {
            stack.Push(new AssignNode(assignEnd - 1, assignEnd, assignEnd));
            
            //Invert argument doesn't matter because begin == end
            return this.PopSetConditionInternal(stack, false, assignEnd, target);
        }

        private int AdjustLine(int line, int target)
        {
            var testLine = line;
            while (testLine >= 1 && this.Code.Op(testLine) == Op.LOADBOOL && (target == -1 || this.Code.A(testLine) == target))
                testLine--;

            if (testLine == line)
                return testLine;

            testLine++;
            testLine += (this.Code.C(testLine) != 0) ? 2 : 1;
            return testLine;
        }

        private Branch PopSetConditionInternal(Stack<Branch> stack, bool invert, int assignEnd, int target)
        {
            var branch = stack.Pop();
            var begin = branch.Begin;
            var end = branch.End;
            if (invert)
                branch = branch.Invert();

            begin = this.AdjustLine(begin, target);
            end = this.AdjustLine(end, target);
            var btarget = branch.SetTarget;
            while (stack.Count > 0)
            {
                var next = stack.Peek();
                var nInvert = false;
                var nEnd = next.End;
                if (this.Code.Op(nEnd) == Op.LOADBOOL && (target == -1 || this.Code.A(nEnd) == target))
                {
                    nInvert = this.Code.B(nEnd) != 0;
                    nEnd = this.AdjustLine(nEnd, target);
                }
                else if (next is TestNode)
                {
                    // also applies to TestSetNode's
                    nInvert = ((TestNode)next).Inverted;
                }
                else if (nEnd >= assignEnd)
                {
                    break;
                }

                var addr = (nInvert == invert) ? end : begin;
                if (addr == nEnd)
                {
                    // TODO: Fix impossible statement
                    //if (addr != nEnd)
                    //    nInvert = !nInvert;
                    var left = this.PopSetConditionInternal(stack, nInvert, assignEnd, target);
                    if (nInvert)
                        branch = new OrBranch(left, branch);
                    else
                        branch = new AndBranch(left, branch);

                    branch.End = nEnd;
                }
                else
                {
                    if (!(branch is TestSetNode))
                    {
                        stack.Push(branch);
                        branch = this.PopCondition(stack);
                    }

                    break;
                }
            }

            branch.IsSet = true;
            branch.SetTarget = btarget;
            return branch;
        }

        public void Print()
            => this.Print(new Output());

        public void Print(Output output)
        {
            this.HandleInitialDeclares(output);
            this.outer.Print(output);
        }

        public Decompiler(LFunction function)
        {
            this.F = new Function(function);
            this.Function = function;
            this.registers = function.MaxStackSize;
            this.length = function.Code.Length;
            this.Code = new Code(function);
            if (function.Locals.Length >= function.NumParams)
            {
                this.DeclList = new Declaration[function.Locals.Length];
                for (var i = 0; i < this.DeclList.Length; i++)
                    this.DeclList[i] = new Declaration(function.Locals[i]);
            }
            else
            {
                // TODO: debug info missing;
                this.DeclList = new Declaration[function.NumParams];
                for (var i = 0; i < this.DeclList.Length; i++)
                {
                    var name = string.Format("_ARG_{0}_", i);
                    this.DeclList[i] = new Declaration(name, 0, this.length - 1);
                }
            }

            this.upvalues = new Upvalues(function.UpValues);
            this.functions = function.Functions;
            this.m_params = function.NumParams;
            this.vararg = function.VarArg;
            this.tForTarget = function.Header.Version.TForTarget;
        }
    }
}
