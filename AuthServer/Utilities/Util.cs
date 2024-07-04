using System;

namespace AuthServer.Utilities
{
    internal static class Util
    {
        internal static string GetTimeMillisecondsStr()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
        }

        internal static string GetTimeStr()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
        }
    }
}
