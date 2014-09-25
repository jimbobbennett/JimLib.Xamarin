using System.Collections.Generic;
using System.Threading.Tasks;

namespace JimBobBennett.JimLib.Xamarin.Network
{
    public class RestResponse<T>
    {
        public RestResponse(string message, int statusCode, T responseObject)
        {
            Message = message;
            StatusCode = statusCode;
            ResponseObject = responseObject;
        }

        public string Message { get; private set; }
        public int StatusCode { get; private set; }
        public T ResponseObject { get; private set; }
    }

    public interface IRestConnection
    {
        Task<RestResponse<T>> MakeRequestAsync<T, TData>(Method method, ResponseType responseType, string baseUrl,
            string resource = "/", string username = null, string password = null, int timeout = 10000,
            Dictionary<string, string> headers = null, TData postData = null)
            where T : class, new()
            where TData : class;

        Task<byte[]> MakeRawGetRequestAsync(string baseUrl, string resource = "/",
            string username = null, string password = null, int timeout = 10000,
            Dictionary<string, string> headers = null);
    }
}
