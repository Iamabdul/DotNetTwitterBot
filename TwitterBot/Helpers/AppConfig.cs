using Microsoft.AspNetCore.Mvc;
using TwitterBot.Core.Commands;
using TwitterBot.Core.Models;
using TwitterBot.Core.Queries;

namespace TwitterBot.Helpers
{
    public static class AppConfig
    {
        public static void MapEndpoints(this WebApplication app)
        {
            var rulesEndpoint = "/rules";
            app.MapGet(rulesEndpoint, async (ITwitterRuleQueries twitterRuleQueries) => Results.Stream(await twitterRuleQueries.GetRules()));

            app.MapPost(rulesEndpoint, async (Rule rule, IInsertRuleCommand insertRuleCommand) =>  await insertRuleCommand.Execute(rule));

            app.MapDelete(rulesEndpoint, async ([FromQuery] string id, IDeleteRuleCommand deleteRuleCommand) => await deleteRuleCommand.Execute(id));
        }
    }
}
