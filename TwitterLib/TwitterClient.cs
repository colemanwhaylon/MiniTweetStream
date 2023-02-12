using Microsoft.Extensions.Logging;
using TwitterLib.Interface;
using System.Net.Mime;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

namespace TwitterLib
{

    public class TwitterClient : ISubject
    {
        private IEnumerable<IObserver> _observers = new List<IObserver>();
        private readonly TwitterClientOptions _twitterClientOptions;
        private ILogger _logger;

        public bool Running { get; private set; }

        public TwitterClient(TwitterClientOptions twitterClientOptions, ILogger logger)
        {
            this._twitterClientOptions = twitterClientOptions;
            _logger = logger;
        }

        public async Task StartReceivingTweets()
        {
            StreamReader? stream = default(StreamReader);

            try
            {
                _logger.LogInformation($"StartReceivingTweets started.");

                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(_twitterClientOptions.TwitterAPIUrl);
                httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, MediaTypeNames.Application.Json);
                httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"{AuthorizationValues.Bearer} {_twitterClientOptions.BearerToken}");

                stream = new StreamReader(await httpClient.GetStreamAsync(_twitterClientOptions.TwitterAPIUrl,
                    _twitterClientOptions.CancellationToken));

                while (!_twitterClientOptions.CancellationToken.IsCancellationRequested)
                {
                    Running = true;

                    try
                    {
                        var tweet = await stream.ReadLineAsync();
                        if (!string.IsNullOrEmpty(tweet))
                        {
                            _logger.LogInformation(tweet);
                            await NotifyObservers(tweet);
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                stream?.Close();
                _logger.LogInformation($"StartReceivingTweets finished.");
            }


        }

        public void StopReceivingTweets()
        {
            Running = false;
        }

        public void RegisterObserver(IObserver observer)
        {
            _observers.Append(observer);
        }

        public void UnRegisterObserver(IObserver observer)
        {
            ((List<IObserver>)_observers).Remove(observer);
        }

        public async Task NotifyObservers(string newTweet)
        {
            var retVal = new ValueTask(Task.CompletedTask);
            await Parallel.ForEachAsync(_observers, (observer, cts) => 
            {
                if (cts.IsCancellationRequested)
                    return retVal;
                observer.Update(newTweet);
                return retVal;
            });
        }
    }
}