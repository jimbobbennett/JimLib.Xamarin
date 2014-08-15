using System.Collections.Generic;
using System.Threading.Tasks;

namespace JimBobBennett.JimLib.Xamarin.Network
{
    public interface IRestConnection
    {
        Task<T> MakeRequestAsync<T, TData>(Method method, ResponseType responseType, string baseUrl,
            string resource = "/", string username = null, string password = null, int timeout = 5000,
            Dictionary<string, string> headers = null, TData postData = null)
            where T : class, new()
            where TData : class, new();
    }
}
