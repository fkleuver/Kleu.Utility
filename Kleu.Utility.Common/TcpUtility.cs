using System.Net;
using System.Net.Sockets;

namespace Kleu.Utility.Common
{
    public static class TcpUtility
    {
        public static int GetFreeTcpPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}
