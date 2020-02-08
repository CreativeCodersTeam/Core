using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CreativeCoders.Net
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var jsonData = await content.ReadAsStringAsync()
                .ConfigureAwait(false);

            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        public static async Task<object> ReadAsJsonAsync(this HttpContent content, Type dataType)
        {
            var jsonData = await content.ReadAsStringAsync()
                .ConfigureAwait(false);

            return JsonConvert.DeserializeObject(jsonData, dataType);
        }
    }
}