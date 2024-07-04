using AuthServer.Utilities;
using System;

namespace AuthServer
{
    internal static class Log
    {
#if DEBUG
        internal static void Debug(string msg)
        {
            Write(msg, -1);
        }
#endif

        internal static void Info(string msg)
        {
            Write(msg);
        }

        internal static void Warn(string msg)
        {
            Write(msg, 1);
        }

        internal static void Error(string msg)
        {
            Write(msg, 2);
        }

        private static void Write(string msg, int index = 0)
        {
            string result = $"[{Util.GetTimeStr()}]:";
            result += index switch
            {
#if DEBUG
                -1 => $"[Debug]: ",
#endif
                0 => $"[Info]: ",
                1 => $"[Warn]: ",
                2 => $"[Error]: ",
                _ => $"[None]: "
            };
            result += msg;
            Console.WriteLine(result);
        }
    }
}
