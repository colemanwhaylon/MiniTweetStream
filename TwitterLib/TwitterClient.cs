using Newtonsoft.Json;
using System.Net;
using System.Runtime.CompilerServices;
using TwitterLib.Model;

namespace TwitterLib
{

    public sealed class TwitterClient
    {
        private static int _counter = 0;
        private static bool _running = false;
        private static readonly object _instanceLock = new object();


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

        public static async Task GetTweets()
        {
            await foreach (var name in StartReceivingTweets())
            {
                Console.WriteLine(name);
            }
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        
        public static async IAsyncEnumerable<Root> StartReceivingTweets()
        {

            var url = "https://api.twitter.com/2/tweets/sample/stream";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.Accept = "application/json";
            var token = Environment.GetEnvironmentVariable("BEARERTOKEN");
            httpRequest.Headers["Authorization"] = $"Bearer {token}";


            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var myJsonResponse = streamReader.ReadLine();
                var tweet = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Root>(myJsonResponse));
                yield return tweet;
            }
        }

        public static Task StopReceivingTweets()
        {
            _running = false;
            throw new NotImplementedException();
        }
    }
}