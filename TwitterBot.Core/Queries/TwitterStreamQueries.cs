using System;
using System.IO;
using System.Threading.Tasks;
using TwitterBot.Core.Clients;

namespace TwitterBot.Queries
{
    public interface IStreamQueries
    {
        Task GetFilteredStream(Action<string> twitterStreamActivated);
    }

    public class TwitterStreamQueries : IStreamQueries
    {
        readonly ITwitterClient _twitterClient;
        
        public TwitterStreamQueries(ITwitterClient twitterClient)
        {
            _twitterClient = twitterClient;
        }

        public async Task GetFilteredStream(Action<string> twitterStreamActivated)
        {
            var result = await _twitterClient.Get("tweets/search/stream?expansions=author_id");

            using (var reader = new StreamReader(result, null, true, -1,  true))
            {
                try
                {
                    while (!reader.EndOfStream)
                    {
                        twitterStreamActivated.Invoke(reader.ReadLine());
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
