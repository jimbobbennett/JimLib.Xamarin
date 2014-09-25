using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using JimBobBennett.JimLib.Events;

// ReSharper disable once CheckNamespace
namespace JimBobBennett.JimLib.Xamarin.Network
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

            Task.Factory.StartNew(async () =>
            {
                await CreateUdpClient(timeout);

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

                        await CreateUdpClient(timeout);
                    }
                }
            });
        }

        private async Task CreateUdpClient(int timeout)
        {
            var created = false;

            while (!created)
            {
                try
                {
                    _udp = new UdpClient(Port)
                    {
                        Client =
                        {
                            ReceiveTimeout = timeout,
                            SendTimeout = timeout
                        }
                    };

                    created = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception when creating UDP client: " + ex.Message);
                }

                if (!created)
                    await Task.Delay(10000);
            }
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