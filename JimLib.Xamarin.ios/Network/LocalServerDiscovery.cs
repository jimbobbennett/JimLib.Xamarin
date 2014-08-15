using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using JimBobBennett.JimLib.Xamarin.Network;

namespace JimBobBennett.JimLib.Xamarin.ios.Network
{
    public class LocalServerDiscovery : ILocalServerDiscovery
    {
        private const int ReceiveTimeout = 10000;
        private const int SleepTime = 100;

        private UdpClient _udp;
        private readonly HashSet<string> _listeners = new HashSet<string>();
        private bool _listening;

        public int Port { get; set; }

        public async Task<IEnumerable<string>> DiscoverLocalServersAsync(string ipAddress)
        {
            return await GetListeners(ipAddress);
        }

        private async Task<IEnumerable<string>> GetListeners(string ipAddress)
        {
            try
            {
                var result = StartListening();

                var client = new UdpClient();
                var ip = new IPEndPoint(IPAddress.Parse(ipAddress), Port);
                var bytes = Encoding.ASCII.GetBytes("M-SEARCH * HTTP/1.0");
                client.Send(bytes, bytes.Length, ip);
                client.Close();

                var totalWaitTime = 0;

                while (_listening)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(SleepTime));
                    totalWaitTime += SleepTime;

                    if (totalWaitTime > ReceiveTimeout)
                    {
                        if (!result.IsCompleted)
                        {
                            CleanUp();
                        }
                    }
                }
            }
            catch
            {
                CleanUp();
            }

            return _listeners;
        }

        private void CleanUp()
        {
            try
            {
                _udp.Close();
            }
            catch
            {
            }

            _listening = false;
            _listeners.Clear();
        }

        private IAsyncResult StartListening()
        {
            _listening = true;
            _udp = new UdpClient(32414)
            {
                Client =
                {
                    ReceiveTimeout = ReceiveTimeout,
                    SendTimeout = ReceiveTimeout
                }
            };
            return _udp.BeginReceive(Receive, new object());
        }

        private void Receive(IAsyncResult ar)
        {
            var ip = new IPEndPoint(IPAddress.Any, Port);

            try
            {
                _udp.EndReceive(ar, ref ip);

                if (_listeners.Add(ip.Address.ToString()))
                    StartListening();
                else
                    _listening = false;
            }
            catch (Exception)
            {
                _listening = false;
            }
        }
    }
}