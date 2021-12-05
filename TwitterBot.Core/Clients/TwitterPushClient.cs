using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TwitterBot.Core.Helpers;

namespace TwitterBot.Core.Clients
{
    public interface ITwitterPushClient
    {
        Task<Stream> Post(string path, object body, bool useOAuth2);
    }

    public class TwitterPushClient : TwitterBaseClient, ITwitterPushClient
    {
        public TwitterPushClient(IConfiguration configuration, IOAuthRequestHelper oAuthRequestHelper)
            : base (configuration, oAuthRequestHelper)
        {
        }

        public async Task<Stream> Post(string path, object body, bool useOAuth2)
        {
            _httpClient.DefaultRequestHeaders.Authorization = useOAuth2 ? _oAuthRequestHelper.GetBearerToken()
                : AuthenticationHeaderValue.Parse(_oAuthRequestHelper.PrepareOAuth1Request(_httpClient.BaseAddress.AbsoluteUri + path));

            var stringContent = new StringContent(JsonSerializer.Serialize(body, TwitterJsonSettings.Options), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(path, stringContent);

            if (!response.IsSuccessStatusCode)
                await ThrowError(response);

            return response.Content.ReadAsStream();
        }        
    }
}