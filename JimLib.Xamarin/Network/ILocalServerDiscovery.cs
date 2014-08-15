using System.Collections.Generic;
using System.Threading.Tasks;

namespace JimBobBennett.JimLib.Xamarin.Network
{
    public interface ILocalServerDiscovery
    {
        int Port { get; set; }
        Task<IEnumerable<string>> DiscoverLocalServersAsync(string ipAddress);
    }
}
