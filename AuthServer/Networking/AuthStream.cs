using System.Net.Sockets;

namespace AuthServer.Networking
{
    internal class AuthStream(Socket sock) : AuthSocket(sock)
    {
        private enum Status { None, Failed }
        private Status _status;


        internal int AuthProcess()
        {
            if (closed) return 2;
            if (_status == Status.None)
            {

            }
            Disc();
            return 2;
        }
    }
}
