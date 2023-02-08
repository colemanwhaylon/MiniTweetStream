using Microsoft.Extensions.Logging;
using System.Net;
using TwitterLib.Interface;
using static System.Net.Mime.MediaTypeNames;

namespace TwitterLib
{

    public sealed class TwitterClient
    {
        private static readonly object _instanceLock = new object();
        private static CancellationToken _cancellationToken = new CancellationToken();
        private static ILogger _logger { get; set; }
        public static bool Running { get; private set; } = false;

        public TwitterClient(ILogger logger) { _logger = logger; }

        public static async Task StartReceivingTweets(IEnumerable<IDataSink> dataSinks)
        {
            _logger.LogInformation($"StartReceivingTweets started.");

            if (dataSinks == null)
                throw new ApplicationException("dataSinks can't be null.");

            var token = Environment.GetEnvironmentVariable("BEARERTOKEN");

            var url = "https://api.twitter.com/2/tweets/sample/stream";

            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(url);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var stream = new StreamReader(await httpClient.GetStreamAsync(url, _cancellationToken));
            while (!_cancellationToken.IsCancellationRequested)
            {
                Running = true;
                var tweet = await stream.ReadLineAsync();
                if (!string.IsNullOrEmpty(tweet))
                {
                    _logger.LogInformation(tweet);
                    Parallel.ForEach(dataSinks, (sink) => { sink.RecieveTweet(tweet); });
                }
            }
            _logger.LogInformation($"StartReceivingTweets finished.");
        }

        public static void StopReceivingTweets()
        {
            Running = false;
        }
    }
}