// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs;

internal static class MiniDump
{
    internal static int ExceptionEventHandlerCode(Exception e, bool threadException)
    {
        var exceptionData = PrintExceptions(e);

        // do not dump or close if in a debugger.
        if (!Debugger.IsAttached)
        {
            if (!MiniDumpAttribute.ForceClose)
            {
                MiniDumpAttribute.ForceClose = true;
            }

            if (string.IsNullOrEmpty(MiniDumpAttribute.CurrentInstance.DumpLogFileName))
            {
                MiniDumpAttribute.CurrentInstance.DumpLogFileName = SettingsFile.ErrorLogPath;
            }

            if (string.IsNullOrEmpty(MiniDumpAttribute.CurrentInstance.DumpFileName))
            {
                MiniDumpAttribute.CurrentInstance.DumpFileName = SettingsFile.MiniDumpPath;
            }

            File.WriteAllText(MiniDumpAttribute.CurrentInstance.DumpLogFileName, exceptionData);
            var diagnosticsClient = new DiagnosticsClient(SettingsFile.ThisProcessId);
            MessageEventArgs args;
            try
            {
                diagnosticsClient.WriteDump(MiniDumpAttribute.CurrentInstance.DumpType, MiniDumpAttribute.CurrentInstance.DumpFileName);
                args = new(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        MiniDumpAttribute.CurrentInstance.Text,
                        MiniDumpAttribute.CurrentInstance.DumpLogFileName),
                    threadException ? MiniDumpAttribute.CurrentInstance.ThreadExceptionTitle : MiniDumpAttribute.CurrentInstance.ExceptionTitle,
                    ErrorLevel.Error);
            }
            catch (ServerErrorException ex)
            {
                args = new(
                    ex.Message,
                    threadException ? MiniDumpAttribute.CurrentInstance.ThreadExceptionTitle : MiniDumpAttribute.CurrentInstance.ExceptionTitle,
                    ErrorLevel.Error);
            }

            MiniDumpAttribute.InvokeDumpMessage(args);
            return args.ExitCode;
        }

        return 1;
    }

    private static string PrintExceptions(Exception exception)
    {
        StringBuilder sb = new();
        sb.AppendLine($"{exception.GetType()}: {exception.Message}{Environment.NewLine}{exception.StackTrace}");
        var currException = exception.InnerException;
        while (currException is not null)
        {
            sb.AppendLine($"{currException.GetType()}: {currException.Message}{Environment.NewLine}{currException.StackTrace}");
            currException = currException.InnerException;
        }

        return sb.ToString();
    }
}
