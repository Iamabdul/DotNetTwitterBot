using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TwitterBot.Core.Helpers;
using TwitterBot.Core.Models.Exceptions;

namespace TwitterBot.Core.Clients
{
    public class TwitterBaseClient
    {
        protected readonly HttpClient _httpClient;
        protected readonly IOAuthRequestHelper _oAuthRequestHelper;

        public TwitterBaseClient(IConfiguration configuration, IOAuthRequestHelper oAuthRequestHelper)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(configuration["Twitter:BaseUri"])
            };

            _oAuthRequestHelper = oAuthRequestHelper;
        }

        protected async Task ThrowError(HttpResponseMessage response)
        {
            var stringError = await response.Content.ReadAsStringAsync();
            throw new TwitterApiException(stringError);
        }
    }
}