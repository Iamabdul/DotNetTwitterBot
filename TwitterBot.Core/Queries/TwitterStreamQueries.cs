using System;
using System.IO;
using System.Threading.Tasks;
using TwitterBot.Core.Clients;

namespace TwitterBot.Core.Queries
{
    public interface ITwitterStreamQueries
    {
        Task GetFilteredStream(Action<string> twitterStreamActivated);
    }

    public class TwitterStreamQueries : ITwitterStreamQueries
    {
        readonly ITwitterPullClient _twitterClient;
        
        public TwitterStreamQueries(ITwitterPullClient twitterClient)
        {
            _twitterClient = twitterClient;
        }

        public async Task GetFilteredStream(Action<string> processResult)
        {
            var result = await _twitterClient.GetStream("tweets/search/stream?expansions=author_id");

            using (var reader = new StreamReader(result, null, true, -1,  true))
            {
                while (!reader.EndOfStream)
                {
                    var readableString = reader.ReadLine();
                    if(readableString.Length > 0)
                        processResult.Invoke(readableString);
                }
            }
        }
    }
}
