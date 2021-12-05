using System.Threading.Tasks;
using TwitterBot.Core.Clients;

namespace TwitterBot.Core.Commands
{
    public interface IDeleteRuleCommand
    {
        Task Execute(string ruleId);
    }
    public class DeleteRuleCommand : IDeleteRuleCommand
    {
        readonly ITwitterPushClient _twitterClient;

        public DeleteRuleCommand(ITwitterPushClient twitterClient)
        {
            _twitterClient = twitterClient;
        }

        public Task Execute(string ruleId)
        {
            var deleteRuleObject = new
            {
                delete = new
                {
                    ids = new object[] { ruleId }
                }
            };
            return _twitterClient.Post("tweets/search/stream/rules", deleteRuleObject, true);
        }
    }
}
