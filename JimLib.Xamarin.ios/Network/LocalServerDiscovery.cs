using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JimBobBennett.JimLib.Events;
using JimBobBennett.JimLib.Extensions;

namespace JimBobBennett.JimLib.Xamarin.Network
{
    public class LocalServerDiscovery : ILocalServerDiscovery
    {
        private readonly Dictionary<int, ServerDiscovery> _serverDiscoveries = new Dictionary<int, ServerDiscovery>();
        private readonly Dictionary<int, string> _foundServer = new Dictionary<int, string>();

        public async Task<string> DiscoverLocalServersAsync(string ipAddress, int port)
        {
            ServerDiscovery serverDiscovery;
            if (!_serverDiscoveries.TryGetValue(port, out serverDiscovery))
            {
                serverDiscovery = new ServerDiscovery(ipAddress, port);
                serverDiscovery.ServerDiscovered += ServerDiscoveryOnServerDiscovered;
                _serverDiscoveries.Add(port, serverDiscovery);
            }

            _foundServer.Remove(port);
            serverDiscovery.Discover();
            string firstServer = null;

            await this.WaitForAsync(() => _foundServer.TryGetValue(port, out firstServer), 10000);

            return firstServer;
        }

        public event EventHandler<EventArgs<string>> ServerDiscovered;

        private void OnServerDiscovered(string server)
        {
            var handler = ServerDiscovered;
            if (handler != null) handler(this, new EventArgs<string>(server));
        }

        private void ServerDiscoveryOnServerDiscovered(object sender, EventArgs<string> eventArgs)
        {
            _foundServer[((ServerDiscovery)sender).Port] = eventArgs.Value;
            OnServerDiscovered(eventArgs.Value);
        }
    }
}