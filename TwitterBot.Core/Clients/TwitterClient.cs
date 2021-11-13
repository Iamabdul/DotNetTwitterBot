using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TwitterBot.Core.Helpers;

namespace TwitterBot.Core.Clients
{
    public interface ITwitterClient
    {
        Task<Stream> Get(string path);
        Task<HttpResponseMessage> Post(string path, object body);
    }

    public class TwitterClient : ITwitterClient
    {
        readonly HttpClient _httpClient;

        public TwitterClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["Twitter:BaseUri"]);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ConfigurationManager.AppSettings["Twitter:BearerToken"]}");
        }

        public Task<Stream> Get(string path)
        {
            return _httpClient.GetStreamAsync(path);
        }

        public Task<HttpResponseMessage> Post(string path, object body)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, path);
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(body, TwitterJsonSettings.Options));
            return _httpClient.SendAsync(requestMessage);
        }
    }
}
