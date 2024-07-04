using System.Net;

namespace AuthServer.Configuration
{
    internal class ConfigFileData
    {
        internal IPAddress IPAddress { get; private set; }
        internal int Port { get; private set; }
        internal IPAddress DBIPAddress { get; private set; }
        internal int DBPort { get; private set; }
        internal string DBLogin { get; private set; }
        internal string DBPassword { get; private set; }
        internal string DBNameAuth { get; private set; }
        internal string DBNameCharacters { get; private set; }


        internal ConfigFileData()
        {
            SetIPAddress();
            SetPort();
            SetDBIPAddress();
            SetDBPort();
            SetDBLogin();
            SetDBPassword();
            SetDBNameAuth();
            SetDBNameCharacters();
        }

        internal bool SetIPAddress(string sIP = null)
        {
            bool flag = IPAddress.TryParse(sIP, out IPAddress ip);
            if (flag)
            {
                IPAddress = ip;
                return true;
            }
            IPAddress = new(new byte[] { 127, 0, 0, 1 });
            return false;
        }

        internal bool SetPort(string sPort = null)
        {
            bool flag = int.TryParse(sPort, out int port);
            if (flag && port > 0 && port < 65536)
            {
                Port = port;
                return true;
            }
            Port = 3805;
            return false;
        }

        internal bool SetDBIPAddress(string sDBIP = null)
        {
            bool flag = IPAddress.TryParse(sDBIP, out IPAddress dbIP);
            if (flag)
            {
                DBIPAddress = dbIP;
                return true;
            }
            else
            {
                DBIPAddress = new(new byte[] { 127, 0, 0, 1 });
                return false;
            }
        }

        internal bool SetDBPort(string sDBPort = null)
        {
            bool flag = int.TryParse(sDBPort, out int dbPort);
            if (flag && dbPort > 0 && dbPort < 65536)
            {
                DBPort = dbPort;
                return true;
            }
            DBPort = 3306;
            return false;
        }

        internal bool SetDBLogin(string sDBLogin = null)
        {
            if (!string.IsNullOrEmpty(sDBLogin))
            {
                DBLogin = sDBLogin;
                return true;
            }
            DBLogin = "test_db_login";
            return false;
        }

        internal bool SetDBPassword(string sDBPassword = null)
        {
            if (!string.IsNullOrEmpty(sDBPassword))
            {
                DBPassword = sDBPassword;
                return true;
            }
            DBPassword = "test_db_password";
            return false;
        }

        internal bool SetDBNameAuth(string sDBNameAuth = null)
        {
            if (!string.IsNullOrEmpty(sDBNameAuth))
            {
                DBNameAuth = sDBNameAuth;
                return true;
            }
            DBNameAuth = "auth";
            return false;
        }

        internal bool SetDBNameCharacters(string sDBNameCharacters = null)
        {
            if (!string.IsNullOrEmpty(sDBNameCharacters))
            {
                DBNameCharacters = sDBNameCharacters;
                return true;
            }
            DBNameCharacters = "characters";
            return false;
        }
    }
}
