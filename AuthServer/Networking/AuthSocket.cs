using AuthServer.Configuration;
using AuthServer.Utilities;
using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Security.Authentication;

namespace AuthServer.Networking
{
    internal abstract class AuthSocket
    {
        private readonly Socket _sock;
        private readonly string _id;
        private enum Status { None, PreTLS, TLS, Failed }
        private Status _status;
        protected bool closed;
        private SslStream _tls;


        protected AuthSocket(Socket sock)
        {
            _sock = sock;
            _id = sock.RemoteEndPoint.ToString() + "-" + Util.GetTimeMillisecondsStr();
            _sock.NoDelay = true;
        }

        internal int TLSProcess()
        {
            if (closed) return 2;
            if (_status == Status.None)
            {
                int aBytes = _sock.Available;
                if (aBytes == 0) return 0;
                if (aBytes > 100 && aBytes < 200)
                {
                    _status = Status.PreTLS;
                    AuthenticateAsServerAsync();
                    return 0;
                }
            }
            else if (_status == Status.PreTLS) return 0;
            else if (_status == Status.TLS) return 1;
            Disc();
            return 2;
        }

        private async void AuthenticateAsServerAsync()
        {
            await Task.Yield();
            _tls = new(new NetworkStream(_sock, true), false);
            SslServerAuthenticationOptions opt = new()
            {
                ServerCertificate = Config.X509Certificate,
                EnabledSslProtocols = SslProtocols.Tls12,
                AllowRenegotiation = true
            };
            try
            {
                _tls.AuthenticateAsServer(opt);
            }
            catch (Exception ex)
            {
                Log.Warn($"Connection {_id} get TLS exception, {ex}");
            }
            if (_tls.IsSigned && _tls.IsAuthenticated && _tls.IsEncrypted)
            {
#if DEBUG
                Log.Debug($"Connection {_id} success secured with TLS.");
#endif
                _status = Status.TLS;
            }
            else
            {
                _status = Status.Failed;
            }
        }

        protected void Disc()
        {
#if DEBUG
            Log.Debug($"Connection {_id} disconnected.");
#endif
            closed = true;
            _sock.Close(500);
        }
    }
}
