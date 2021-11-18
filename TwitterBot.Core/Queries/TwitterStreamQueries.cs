using System;
using System.IO;
using System.Threading.Tasks;
using TwitterBot.Core.Clients;
using TwitterBot.Core.Clients.Twitter;

namespace TwitterBot.Queries
{
    public interface ITwitterStreamQueries
    {
        Task GetFilteredStream(Action<string> twitterStreamActivated);
    }

    public class TwitterStreamQueries : ITwitterStreamQueries
    {
        readonly ITwitterClient _twitterClient;
        
        public TwitterStreamQueries(ITwitterClient twitterClient)
        {
            _twitterClient = twitterClient;
        }

        public async Task GetFilteredStream(Action<string> processResult)
        {
            var result = await _twitterClient.Get("tweets/search/stream?expansions=author_id");

            using (var reader = new StreamReader(result, null, true, -1,  true))
            {
                try
                {
                    while (!reader.EndOfStream)
                    {
                        var readableString = reader.ReadLine();
                        if(readableString.Length > 0)
                            processResult.Invoke(readableString);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
