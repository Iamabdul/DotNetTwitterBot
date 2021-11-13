using System.Threading.Tasks;
using TwitterBot.Core.Clients;

namespace TwitterBot.Core.Helpers
{
    public class RulesInitiator
    {
        ITwitterClient _twitterClient;

        public RulesInitiator(ITwitterClient twitterClient)
        {
            _twitterClient = twitterClient;
        }

        public async Task InitiateRulesForStream(string[] stringArray)
        {

        }
    }
}
