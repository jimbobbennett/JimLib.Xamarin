using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JimBobBennett.JimLib.Xml;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace JimBobBennett.JimLib.Xamarin.Network
{
    /// <summary>
    /// Provides the base class for connections - this file can be shared between the mono and win versions
    /// as the using directives match despite the assemblies being different.
    /// </summary>
    public class RestConnectionBase : IRestConnection
    {
        public async Task<T> MakeRequestAsync<T, TData>(Method method, ResponseType responseType, string baseUrl,
            string resource = "/", string username = null, string password = null, int timeout = 5000,
            Dictionary<string, string> headers = null, TData postData = null)
            where T : class, new()
            where TData : class, new()
        {
            using (var clientHandler = new HttpClientHandler{UseCookies = false})
            {
                if (!string.IsNullOrEmpty(username))
                    clientHandler.Credentials = new NetworkCredential(username, password);

                using (var client = new HttpClient(clientHandler))
                {
                    try
                    {
                        client.BaseAddress = new Uri(baseUrl);
                        client.Timeout = new TimeSpan(0, 0, 0, 0, timeout);

                        if (headers != null)
                        {
                            foreach (var h in headers)
                                client.DefaultRequestHeaders.Add(h.Key, h.Value);
                        }

                        var requestUri = new Uri(resource, UriKind.Relative);

                        switch (method)
                        {
                            case Method.Get:
                                var responseBody = await client.GetStringAsync(requestUri);
                                return DeserializeResponse<T>(responseBody, responseType);

                            case Method.Post:
                                var response = await client.PostAsync(requestUri, SerializePostData(postData, responseType));
                                response.EnsureSuccessStatusCode();
                                var postResponseBody = await response.Content.ReadAsStringAsync();
                                return DeserializeResponse<T>(postResponseBody, responseType);
                        }

                        return null;
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }
        }

        private static HttpContent SerializePostData<T>(T postData, ResponseType responseType)
            where T : class, new()
        {
            if (postData == null)
                return new StringContent(string.Empty);
            
            if (responseType == ResponseType.Xml)
                throw new NotSupportedException();

            var jsonRequest = JsonConvert.SerializeObject(postData);
            return new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        }

        private static T DeserializeResponse<T>(string responseBody, ResponseType responseType)
            where T : class, new()
        {
            if (string.IsNullOrEmpty(responseBody))
                return null;

            if (responseType == ResponseType.Xml)
                return new XmlDeserializer().Deserialize<T>(responseBody);

            return JsonConvert.DeserializeObject<T>(responseBody);
        }
    }
}