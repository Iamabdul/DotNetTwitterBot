using System.IO;
using System.Threading.Tasks;
using TwitterBot.Core.Clients;

namespace TwitterBot.Core.Queries
{
    public interface ITwitterRuleQueries
    {
        Task<Stream> GetRules();
    }

    public class TwitterRuleQueries : ITwitterRuleQueries
    {
        readonly ITwitterPullClient _twitterClient;

        public TwitterRuleQueries(ITwitterPullClient twitterClient)
        {
            _twitterClient = twitterClient;
        }

        public async Task<Stream> GetRules()
        {
            return await _twitterClient.Get("tweets/search/stream/rules");
        }
    }
}
