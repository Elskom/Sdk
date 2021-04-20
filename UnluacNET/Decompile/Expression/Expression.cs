// Copyright (c) 2020-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: see LICENSE for more details.

namespace Elskom.Generic.Libs.UnluacNET
{
    using System;
    using System.Collections.Generic;

    public abstract class Expression
    {
        public static readonly int PRECEDENCE_OR = 1;
        public static readonly int PRECEDENCE_AND = 2;
        public static readonly int PRECEDENCE_COMPARE = 3;
        public static readonly int PRECEDENCE_CONCAT = 4;
        public static readonly int PRECEDENCE_ADD = 5;
        public static readonly int PRECEDENCE_MUL = 6;
        public static readonly int PRECEDENCE_UNARY = 7;
        public static readonly int PRECEDENCE_POW = 8;
        public static readonly int PRECEDENCE_ATOMIC = 9;
        public static readonly int ASSOCIATIVITY_NONE = 0;
        public static readonly int ASSOCIATIVITY_LEFT = 1;
        public static readonly int ASSOCIATIVITY_RIGHT = 2;
        public static readonly Expression NIL = new ConstantExpression(new(LNil.NIL), -1);

        protected Expression(int precedence)
            => this.Precedence = precedence;

        public abstract int ConstantIndex { get; }

        public virtual int ClosureUpvalueLine => throw new InvalidOperationException();

        public virtual bool BeginsWithParen => false;

        public virtual bool IsBoolean => false;

        public virtual bool IsBrief => false;

        public virtual bool IsClosure => false;

        public virtual bool IsConstant => false;

        public virtual bool IsDotChain => false;

        public virtual bool IsIdentifier => false;

        public virtual bool IsInteger => false;

        public virtual bool IsMultiple => false;

        public virtual bool IsMemberAccess => false;

        public virtual bool IsNil => false;

        public virtual bool IsString => false;

        public virtual bool IsTableLiteral => false;

        public virtual bool IsNewEntryAllowed => false;

        public int Precedence { get; private set; }

        public static BinaryExpression MakeADD(Expression left, Expression right)
            => new("+", left, right, PRECEDENCE_ADD, ASSOCIATIVITY_LEFT);

        public static BinaryExpression MakeAND(Expression left, Expression right)
            => new("and", left, right, PRECEDENCE_AND, ASSOCIATIVITY_NONE);

        public static BinaryExpression MakeCONCAT(Expression left, Expression right)
            => new("..", left, right, PRECEDENCE_CONCAT, ASSOCIATIVITY_RIGHT);

        public static BinaryExpression MakeDIV(Expression left, Expression right)
            => new("/", left, right, PRECEDENCE_MUL, ASSOCIATIVITY_LEFT);

        public static BinaryExpression MakeMOD(Expression left, Expression right)
            => new("%", left, right, PRECEDENCE_MUL, ASSOCIATIVITY_LEFT);

        public static BinaryExpression MakeMUL(Expression left, Expression right)
            => new("*", left, right, PRECEDENCE_MUL, ASSOCIATIVITY_LEFT);

        public static BinaryExpression MakeOR(Expression left, Expression right)
            => new("or", left, right, PRECEDENCE_OR, ASSOCIATIVITY_NONE);

        public static BinaryExpression MakePOW(Expression left, Expression right)
            => new("^", left, right, PRECEDENCE_POW, ASSOCIATIVITY_RIGHT);

        public static BinaryExpression MakeSUB(Expression left, Expression right)
            => new("-", left, right, PRECEDENCE_ADD, ASSOCIATIVITY_LEFT);

        public static UnaryExpression MakeUNM(Expression expression)
            => new("-", expression, PRECEDENCE_UNARY);

        public static UnaryExpression MakeNOT(Expression expression)
            => new("not ", expression, PRECEDENCE_UNARY);

        public static UnaryExpression MakeLEN(Expression expression)
            => new("#", expression, PRECEDENCE_UNARY);

        public static void PrintSequence(Output output, List<Expression> exprs, bool lineBreak, bool multiple)
        {
            var n = exprs.Count;
            var i = 1;
            foreach (var expr in exprs)
            {
                var last = i == n;
                if (expr.IsMultiple)
                {
                    last = true;
                }

                if (last)
                {
                    if (multiple)
                    {
                        expr.PrintMultiple(output);
                    }
                    else
                    {
                        expr.Print(output);
                    }

                    break;
                }
                else
                {
                    expr.Print(output);
                    output.Print(",");
                    if (lineBreak)
                    {
                        output.PrintLine();
                    }
                    else
                    {
                        output.Print(" ");
                    }
                }

                i++;
            }
        }

        public virtual bool IsUpvalueOf(int register)
            => throw new InvalidOperationException();

        public virtual void AddEntry(TableLiteral.Entry entry)
            => throw new InvalidOperationException();

        public virtual string GetField()
            => throw new InvalidOperationException();

        public virtual Expression GetTable()
            => throw new InvalidOperationException();

        public abstract void Print(Output output);

        public virtual void PrintClosure(Output output, Target name)
            => throw new InvalidOperationException();

        public virtual void PrintMultiple(Output output)
            => this.Print(output);

        public virtual string AsName()
            => throw new InvalidOperationException();

        public virtual int AsInteger()
            => throw new InvalidOperationException();

        protected static void PrintBinary(Output output, string op, Expression left, Expression right)
        {
            left.Print(output);
            output.Print(" ");
            output.Print(op);
            output.Print(" ");
            right.Print(output);
        }

        protected static void PrintUnary(Output output, string op, Expression expression)
        {
            output.Print(op);
            expression.Print(output);
        }
    }
}
