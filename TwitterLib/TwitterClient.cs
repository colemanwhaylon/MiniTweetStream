using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using TwitterLib.Interface;
using TwitterLib.Model;

namespace TwitterLib
{

    public sealed class TwitterClient
    {
        private static int _counter = 0;
        private static bool _running = false;
        private static readonly object _instanceLock = new object();
        private static CancellationToken _cancellationToken = new CancellationToken();


        private TwitterClient()
        {
            _counter++;
            Console.WriteLine("Counter Value " + _counter.ToString());
        }
        private static TwitterClient instance = null;

        public static bool Running { get { return _running; } private set { } }
        public static TwitterClient GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (_instanceLock)
                    {
                        if (instance == null)
                        {
                            instance = new TwitterClient();
                        }
                    }
                }
                return instance;
            }
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public static async Task StartReceivingTweets(IEnumerable<IDataSink> dataSinks)
        {
            if (dataSinks == null)
                throw new ApplicationException("dataSinks can't be null.");

            var token = Environment.GetEnvironmentVariable("BEARERTOKEN");

            var url = "https://api.twitter.com/2/tweets/sample/stream";

            //var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(url);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var httpResponse = (HttpWebResponse)httpClient.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    var tweet = await streamReader.ReadLineAsync(_cancellationToken);
                    if (!string.IsNullOrEmpty(tweet))
                        Parallel.ForEach(dataSinks, (sink) => { sink.RecieveTweet(tweet); });
                }
            }
        }

        public static Task StopReceivingTweets()
        {
            _running = false;
            throw new NotImplementedException();
        }
    }
}