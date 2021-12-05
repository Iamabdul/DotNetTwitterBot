using Microsoft.Extensions.DependencyInjection;
using TwitterBot.Core.Clients;
using TwitterBot.Core.Commands;
using TwitterBot.Core.Queries;
using TwitterBot.Core.Services;

namespace TwitterBot.Core.Helpers
{
    public static class DiConfig
    {
        public static void ConfigureTwitterBotServices(this IServiceCollection services)
        {
            //singletons
            services.AddSingleton<ITwitterStreamQueries, TwitterStreamQueries>();
            services.AddTransient<ITwitterRuleQueries, TwitterRuleQueries>();

            services.AddTransient<IInsertRuleCommand, InsertRuleCommand>();
            services.AddTransient<IDeleteRuleCommand, DeleteRuleCommand>();
            services.AddSingleton<IRetweetCommand, RetweetCommand>();

            //transients
            services.AddTransient<ITwitterPushClient, TwitterPushClient>();
            services.AddTransient<ITwitterPullClient, TwitterPullClient>();

            //helpers
            services.AddSingleton<IOAuthRequestHelper, OAuthRequestHelper>();

            //Hosted services
            services.AddHostedService<TwitterStreamService>();
        }
    }
}
