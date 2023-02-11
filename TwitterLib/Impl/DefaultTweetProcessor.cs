using Microsoft.Extensions.Logging;
using TwitterLib.Interface;

namespace TwitterLib.Impl
{
    public class DefaultTweetProcessor : ITweetProcessor
    {
        private readonly ILogger _logger;

        public DefaultTweetProcessor(ILogger logger)
        {
            _logger = logger;
        }
        public void ProcessTweet(string tweet)
        {
            _logger.LogInformation($"From DefaultTweetProcessor {nameof(DefaultTweetProcessor)} {tweet}");
            throw new ApplicationException("Error from DefaultTweetProcessor.");
        }
    }
}