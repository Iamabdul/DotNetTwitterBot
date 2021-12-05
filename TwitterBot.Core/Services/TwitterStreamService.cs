using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TwitterBot.Core.Commands;
using TwitterBot.Core.Helpers;
using TwitterBot.Core.Models;
using TwitterBot.Core.Queries;

namespace TwitterBot.Core.Services
{
    public class TwitterStreamService : BackgroundService
    {
        readonly ILogger<TwitterStreamService> _logger;
        readonly ITwitterStreamQueries _twitterStreamQueries;
        readonly IRetweetCommand _retweetCommand;

        public TwitterStreamService(
            ILogger<TwitterStreamService> logger, 
            ITwitterStreamQueries twitterStreamQueries, 
            IRetweetCommand retweetCommand)
        {
            _logger = logger;
            _twitterStreamQueries = twitterStreamQueries;
            _retweetCommand = retweetCommand;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _twitterStreamQueries.GetFilteredStream(async (response) => await ProcessResult(response));
        }

        public async Task ProcessResult(string response)
        {
            //deserialise message
            var deserializedTweet = JsonSerializer.Deserialize<StreamTweetDTO>(response, TwitterJsonSettings.Options).Data;

            // get tweet Id
            var tweetId = deserializedTweet.Id;

            // Use retweet commnd with Id
            _logger.LogInformation($"Attempting to execute retweet for: {tweetId}, tweet contents: {deserializedTweet.Text}");
            await _retweetCommand.Execute(tweetId);
            _logger.LogInformation($"Eexecute retweet for: {tweetId} successful");
        }
    }
}
