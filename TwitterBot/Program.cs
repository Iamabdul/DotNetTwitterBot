using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TwitterBot.Core.Clients.Twitter;
using TwitterBot.Core.Commands;
using TwitterBot.Core.Services;
using TwitterBot.Queries;

namespace TwitterBot
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                   .ConfigureServices((hostBuilderContext, services) =>
                   {
                       //singletons
                       services.AddSingleton<ITwitterStreamQueries, TwitterStreamQueries>();
                       services.AddSingleton<IRetweetCommand, RetweetCommand>();

                       //transients
                       services.AddTransient<ITwitterClient, TwitterClient>();

                       //Hosted services, could probably add other social network APIs soon
                       services.AddHostedService<TwitterStreamService>();
                   });
    }
}
