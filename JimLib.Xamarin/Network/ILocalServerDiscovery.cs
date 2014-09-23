using System;
using System.Threading.Tasks;
using JimBobBennett.JimLib.Events;

namespace JimBobBennett.JimLib.Xamarin.Network
{
    public interface ILocalServerDiscovery
    {
        Task<string> DiscoverLocalServersAsync(string ipAddress, int port);
        event EventHandler<EventArgs<string>> ServerDiscovered;
    }
}
