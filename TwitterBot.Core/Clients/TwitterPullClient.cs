using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using TwitterBot.Core.Helpers;

namespace TwitterBot.Core.Clients
{
    public interface ITwitterPullClient
    {
        Task<Stream> GetStream(string path);
        Task<Stream> Get(string path);
    }

    public class TwitterPullClient : TwitterBaseClient, ITwitterPullClient
    {
        public TwitterPullClient(IConfiguration configuration, IOAuthRequestHelper oAuthRequestHelper)
            : base(configuration, oAuthRequestHelper)
        {
        }

        public Task<Stream> GetStream(string path)
        {
            _httpClient.DefaultRequestHeaders.Authorization = _oAuthRequestHelper.GetBearerToken();
            return _httpClient.GetStreamAsync(path);
        }

        public async Task<Stream> Get(string path)
        {
            _httpClient.DefaultRequestHeaders.Authorization = _oAuthRequestHelper.GetBearerToken();
            var response = await _httpClient.GetAsync(path);

            if (!response.IsSuccessStatusCode)
                await ThrowError(response);

            return await response.Content.ReadAsStreamAsync();
        }
    }
}
