using AuthServer.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AuthServer.Networking
{
    internal static class AuthStreamMgr
    {
        private static Socket _sock;
        private static List<AuthStream> _streams;
        private static List<AuthStream> _tlsStreams;


        internal static bool Start()
        {
            try
            {
                IPEndPoint ipEndPoint = new(Config.Data.IPAddress, Config.Data.Port);
                _sock = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _sock.Bind(ipEndPoint);
                _sock.Listen();
                _sock.NoDelay = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
            _streams = [];
            _tlsStreams = [];
            new Thread(new ThreadStart(AcceptConnectionLoop)).Start();
            Thread.Sleep(100);
            new Thread(new ThreadStart(AcceptTLSConnectionLoop)).Start();
            Thread.Sleep(100);
            new Thread(new ThreadStart(AcceptAuthConnectionLoop)).Start();
            return true;
        }

        private static void AcceptConnectionLoop()
        {
            Log.Info("Starting accept connections...");
            while (true)
            {
                Thread.Sleep(50);
                Socket sock = _sock.Accept();
                _streams.Add(new(sock));
            }
        }

        private static void AcceptTLSConnectionLoop()
        {
            Log.Info("Starting accept TLS connections...");
            while (true)
            {
                Thread.Sleep(50);
                for (int i = 0; i < _streams.Count; i++)
                {
                    AuthStream stream = _streams[i];
                    if (stream == null) continue;
                    int index = stream.TLSProcess();
                    if (index == 0) continue;
                    if (index == 1)
                        _tlsStreams.Add(stream);
                    _streams[i] = null;
                }
            }
        }

        private static void AcceptAuthConnectionLoop()
        {
            Log.Info("Starting accept Auth connections...");
            while (true)
            {
                Thread.Sleep(50);
                for (int i = 0; i < _tlsStreams.Count; i++)
                {
                    AuthStream stream = _tlsStreams[i];
                    if (stream == null) continue;
                    int index = stream.AuthProcess();
                    if (index == 0) continue;
                    if (index == 1)
                        continue;
                    _tlsStreams[i] = null;
                }
            }
        }
    }
}
