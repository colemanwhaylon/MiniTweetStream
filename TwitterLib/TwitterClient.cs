using Microsoft.Extensions.Logging;
using TwitterLib.Interface;
using System.Net.Mime;

namespace TwitterLib
{

    public sealed class TwitterClient : ISubject
    {
        private List<IObserver> _observers = new List<IObserver>();
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
            try
            {
                _logger.LogInformation($"StartReceivingTweets started.");

                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(_twitterClientOptions.TwitterAPIUrl);

                httpClient.DefaultRequestHeaders.Add( HeaderNames.Accept, MediaTypeNames.Application.Json);
                
                
                httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization,  $"{AuthorizationValues.Bearer} {_twitterClientOptions.BearerToken}");

                var stream = new StreamReader(await httpClient.GetStreamAsync(_twitterClientOptions.TwitterAPIUrl, 
                    _twitterClientOptions.CancellationToken));
                while (!_twitterClientOptions.CancellationToken.IsCancellationRequested)
                {
                    Running = true;
                    var tweet = await stream.ReadLineAsync();
                    if (!string.IsNullOrEmpty(tweet))
                    {
                        _logger.LogInformation(tweet);
                        NotifyObservers(tweet);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _logger.LogInformation($"StartReceivingTweets finished.");
            }


        }

        public void StopReceivingTweets()
        {
            Running = false;
        }

        public void RegisterObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void UnRegisterObserver(IObserver observer)
        {
            _observers.Remove(observer);    
        }

        public void NotifyObservers(string newTweet)
        {
            foreach (IObserver observer in _observers)
            {
                Parallel.ForEach(_observers, (observer) => { observer.Update(newTweet); });
            }
        }
    }
}