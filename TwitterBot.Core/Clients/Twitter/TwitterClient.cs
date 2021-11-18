using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TwitterBot.Core.Helpers;

namespace TwitterBot.Core.Clients.Twitter
{
    public interface ITwitterClient
    {
        Task<Stream> Get(string path);
        Task<HttpResponseMessage> Post(string path, object body);
    }

    public class TwitterClient : ITwitterClient
    {
        protected readonly HttpClient _httpClient;

        public TwitterClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["Twitter:BaseUri"]);
        }

        public Task<Stream> Get(string path)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" ,$"{ConfigurationManager.AppSettings["Twitter:BearerToken"]}");
            return _httpClient.GetStreamAsync(path);
        }

        public async Task<HttpResponseMessage> Post(string path, object body)
        {
            _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(OAuthRequestHelper.PrepareOAuth1Request(_httpClient.BaseAddress.AbsoluteUri + path));
            var stringContent = new StringContent(JsonSerializer.Serialize(body, TwitterJsonSettings.Options), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(path, stringContent);

            if (!response.IsSuccessStatusCode)
                await CheckForErrors(response);

            return response;
        }

        private async Task CheckForErrors(HttpResponseMessage response)
        {
            var stringError = await response.Content.ReadAsStringAsync();
            throw new Exception(stringError);
        }
    }    

    public class TwitterErrorDTO
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}