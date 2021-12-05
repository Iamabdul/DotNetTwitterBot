using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TwitterBot.Core.Clients;

namespace TwitterBot.Core.Commands
{
    public interface IRetweetCommand
    {
        Task Execute(string tweetId);
    }
    public class RetweetCommand : IRetweetCommand
    {
        readonly ITwitterPushClient _twitterClient;
        readonly string _botUserId;
        public RetweetCommand(ITwitterPushClient twitterClient, IConfiguration configuration)
        {
            _twitterClient = twitterClient;
            _botUserId = configuration["Twitter:BotUserId"];
        }

        public Task Execute(string tweetId)
        {
            var retweetObj = new
            {
                tweet_id = tweetId
            };

            return _twitterClient.Post($"users/{_botUserId}/retweets", retweetObj, false);
        }
    }
}