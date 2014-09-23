using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using JimBobBennett.JimLib.Events;

namespace JimBobBennett.JimLib.Xamarin.ios.Network
{
    class ServerDiscovery
    {
        private readonly string _ipAddress;
        private UdpClient _udp;

        public int Port { get; private set; }

        public ServerDiscovery(string ipAddress, int port, int timeout = 10000)
        {
            _ipAddress = ipAddress;
            Port = port;
            CreateUdpClient(timeout);

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    try
                    {
                        var result = await _udp.ReceiveAsync();
                        var server = result.RemoteEndPoint.Address.ToString();
                        OnServerDiscovered(server);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Exception when listening for UDP: " + ex.Message);
                    }

                    if (_udp.Client == null)
                    {
                        try
                        {
                            _udp.Close();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Exception when listening for UDP: " + ex.Message);
                        }

                        CreateUdpClient(timeout);
                    }
                }
            });
        }

        private void CreateUdpClient(int timeout)
        {
            _udp = new UdpClient(Port)
            {
                Client =
                {
                    ReceiveTimeout = timeout,
                    SendTimeout = timeout
                }
            };
        }

        public void Discover()
        {
            using (var client = new UdpClient())
            {
                var ip = new IPEndPoint(IPAddress.Parse(_ipAddress), Port);
                var bytes = Encoding.ASCII.GetBytes("M-SEARCH * HTTP/1.0");
                client.Send(bytes, bytes.Length, ip);
                client.Close();
            }
        }

        public event EventHandler<EventArgs<string>> ServerDiscovered;

        private void OnServerDiscovered(string server)
        {
            var handler = ServerDiscovered;
            if (handler != null) handler(this, new EventArgs<string>(server));
        }
    }
}