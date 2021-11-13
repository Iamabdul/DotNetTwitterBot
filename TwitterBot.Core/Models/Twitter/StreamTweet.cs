namespace TwitterBot.Core.Models.Twitter
{
    public class StreamTweet
    {
        public string Id { get; set; }
        public string Text { get; set; }
    }

    public class StreamTweetDTO
    {
        public StreamTweet Data { get; set; }
    }
}