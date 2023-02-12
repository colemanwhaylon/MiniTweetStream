using Microsoft.Extensions.Logging;
using System.Net.Mime;
using TwitterLib.Interface;

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
                            NotifyObservers(tweet);
                        }
                    }
                    catch (ArgumentOutOfRangeException aoore)
                    {
                        break;
                    }
                    catch (ObjectDisposedException ode)
                    {
                        break;
                    }
                    catch (InvalidOperationException ioe)
                    {
                        break;
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
            ((List<IObserver>)_observers).Add(observer);
        }

        public void UnRegisterObserver(IObserver observer)
        {
            ((List<IObserver>)_observers).Remove(observer);
        }

        public void NotifyObservers(string newTweet)
        {
            Parallel.ForEach(_observers, (observer, cts) => { observer.Update(newTweet); });
        }
    }
}