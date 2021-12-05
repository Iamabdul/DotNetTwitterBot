using System;

namespace TwitterBot.Core.Models.Exceptions
{
    public class TwitterApiException : Exception
    {
        public TwitterApiException(string message) : base(message)
        {
        }
    }
}
