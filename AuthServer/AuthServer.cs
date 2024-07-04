using AuthServer.Configuration;
using AuthServer.Networking;
using System;

namespace AuthServer
{
    internal class AuthServer
    {
        internal static void Main(/*string[] args*/)
        {
            Log.Info("AuthServer is starting...");
            if (!Config.Start())
            {
                Log.Error("Can't initialize configuration.");
                return;
            }
            if (!AuthStreamMgr.Start())
            {
                Log.Error("Can't start network.");
                return;
            }
            bool flag = true;
            while (flag)
            {
                string cmd = Console.ReadLine().Trim();
                switch (cmd)
                {
                    case "stop":
                    case "exit":
                    case "cancel":
                        flag = false;
                        break;
                }
            }
            Log.Info("AuthServer is stopping..");
        }
    }
}
