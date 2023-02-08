using Microsoft.Extensions.Logging;
using TwitterLib.Interface;

namespace MiniTweetStream.Test
{
    public class DataSink : IDataSink
    {
        private readonly ILogger _logger;

        public DataSink(ILogger logger)
        {
            this._logger = logger;
        }
        public void RecieveTweet(string tweet)
        {
            _logger.LogInformation($"From IDataSink {nameof(DataSink)} {tweet}");   
        }
    }
}