using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Text.Json;
using TwitterLib;
using TwitterLib.Interface;

namespace MiniTweetStream.Test
{
    public class TwitterLibTest
    {
        private TwitterClient _twitterClient = null;
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

            _twitterClient = new TwitterClient(_logger);
        }


        [Fact]
        public async Task ShouldBeAbleToStartReceivingTweets()
        {
            IDataSink dataSink = new DataSink(_logger);
            List<IDataSink> dataSinkList = new List<IDataSink>();
            dataSinkList.Add(dataSink);
            await TwitterClient.StartReceivingTweets(dataSinkList);
        }

        //[Fact]
        //public void ShouldBeAbleToStopReceivingTweets()
        //{
        //    var x = TwitterClient.StopReceivingTweets();
        //}
    }
}