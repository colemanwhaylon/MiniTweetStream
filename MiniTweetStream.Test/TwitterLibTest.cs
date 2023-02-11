using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Text.Json;
using TwitterLib;
using TwitterLib.Impl;
using TwitterLib.Interface;

namespace MiniTweetStream.Test
{
    public class TwitterLibTest
    {
        private const string twitterApiUrl = "https://api.twitter.com/2/tweets/sample/stream";
        private readonly CancellationToken _cancellationToken;
        private readonly TwitterClient _twitterClient;
        private readonly ILogger _logger;

        public TwitterLibTest()
        {
            using IHost host = Host.CreateDefaultBuilder()
                                .ConfigureLogging(builder =>
                                    builder.AddJsonConsole(options =>
                                    {
                                        options.IncludeScopes = false;
                                        options.TimestampFormat = "HH:mm:ss ";
                                        options.JsonWriterOptions = new JsonWriterOptions
                                        {
                                            Indented = true
                                        };
                                    }))
                                .Build();

            _logger = host.Services
                        .GetRequiredService<ILoggerFactory>()
                        .CreateLogger("TwitterClient");

            _cancellationToken = new CancellationToken();
            var bearerToken = Environment.GetEnvironmentVariable("BEARERTOKEN");

            _twitterClient = new TwitterClient(new TwitterClientOptions(bearerToken, 
                _cancellationToken, 
                twitterApiUrl),
                _logger);
        }


        [Fact]
        public async Task ShouldBeAbleToStartReceivingTweets()
        {
            var tweetProcessor = new TweetProcessor("Default Tweet Processor", _twitterClient, _logger);
            await _twitterClient.StartReceivingTweets();
        }

        //[Fact]
        //public void ShouldBeAbleToStopReceivingTweets()
        //{
        //    var x = TwitterClient.StopReceivingTweets();
        //}
    }
}