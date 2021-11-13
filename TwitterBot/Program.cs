using System;
using System.Text.Json;
using System.Threading.Tasks;
using TwitterBot.Core.Clients;
using TwitterBot.Core.Commands;
using TwitterBot.Core.Helpers;
using TwitterBot.Core.Models.Twitter;
using TwitterBot.Queries;

namespace TwitterBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var twitterStreamQuery = new TwitterStreamQueries(new TwitterClient()); // this TwitterClient is made for the listening
            await twitterStreamQuery.GetFilteredStream(async (result) => await WriteResults(result));
        }

        public static Task WriteResults(string result)
        {
            if (string.IsNullOrWhiteSpace(result))
                return Task.CompletedTask;

            //deserialise message
            var deserializedTweet = JsonSerializer.Deserialize<StreamTweetDTO>(result, TwitterJsonSettings.Options).Data;

            // get tweet Id
            var tweetId = deserializedTweet.Id;

            // Use retweet commnd with Id
            var retweetCommand = new RetweetCommand(new TwitterClient()); //this twitter client needs to be a different instance than the one used in line: 16 becuase we need to talk back
            Console.WriteLine($"Attempting to execute retweet for: {tweetId}, tweet contents: {deserializedTweet.Text}");
            return retweetCommand.Execute(tweetId);
        }
    }
}
