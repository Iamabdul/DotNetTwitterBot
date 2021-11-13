using System.Threading.Tasks;
using TwitterBot.Core.Clients;
using TwitterBot.Core.Helpers;

namespace TwitterBot.Core.Commands
{
    public interface IRetweetCommand
    {
        Task Execute(string tweetId);
    }
    public class RetweetCommand : IRetweetCommand
    {
        readonly ITwitterClient _twitterClient;
        public RetweetCommand(ITwitterClient twitterClient)
        {
            _twitterClient = twitterClient;
        }

        public Task Execute(string tweetId)
        {
            var retweetObj = new
            {
                tweet_id = tweetId
            };

            return _twitterClient.Post($"users/{ConfigurationManager.AppSettings["Twitter:BotUserId"]}/retweets", retweetObj);
        }
    }
}