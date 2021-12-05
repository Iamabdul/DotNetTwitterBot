using System.Threading.Tasks;
using TwitterBot.Core.Clients;
using TwitterBot.Core.Models;

namespace TwitterBot.Core.Commands
{
    public interface IInsertRuleCommand
    {
        Task Execute(Rule rule);
    }
    public class InsertRuleCommand : IInsertRuleCommand
    {
        readonly ITwitterPushClient _twitterClient;

        public InsertRuleCommand(ITwitterPushClient twitterClient)
        {
            _twitterClient = twitterClient;
        }

        public Task Execute(Rule rule)
        {
            var newRuleObject = new
            {
                add = new object[]
                { 
                    rule
                }
            };

            return _twitterClient.Post($"tweets/search/stream/rules", newRuleObject, true);
        }
    }
}