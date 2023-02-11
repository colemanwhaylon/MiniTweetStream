using Microsoft.Extensions.Logging;
using TwitterLib.Interface;
using System.Collections.Generic;
using System.Collections.Concurrent;


namespace TwitterLib.Impl
{
    public class TweetProcessor : IObserver, IDisposable
    {
        private Queue<string> _queue = new Queue<string>();
        private readonly string _name;
        private readonly ISubject _subject;
        private readonly ILogger _logger;

        public TweetProcessor(string name,
            ISubject subject,
            ILogger logger)
        {
            this._name = name;
            this._subject = subject;
            subject.RegisterObserver(this);
            _logger = logger;
        }

        public async Task<bool> ProcessTweet(string tweet)
        {
            _logger.LogInformation($"From TweetProcessor {nameof(TweetProcessor)} {tweet}");
            return await Task.FromResult(true);
        }

        public async Task Update(string tweet)
        {
            _queue.Enqueue(tweet);
            await ProcessTweet(tweet);
        }

        public void Dispose()
        {
            _subject.UnRegisterObserver(this);
        }
    }
}