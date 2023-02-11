using Microsoft.Extensions.Logging;
using TwitterLib.Interface;
using System.Collections.Generic;
using System.Collections.Concurrent;


namespace TwitterLib.Impl
{
    public class TweetProcessor : IObserver
    {
        private Queue<string> _queue = new Queue<string>();
        private readonly string _name;
        private readonly ILogger _logger;

        public TweetProcessor(string name, 
            ISubject subject,
            ILogger logger)
        {
            this._name = name;
            subject.RegisterObserver(this);
            _logger = logger;
        }

        public void ProcessTweet(string tweet)
        {
            _logger.LogInformation($"From TweetProcessor {nameof(TweetProcessor)} {tweet}");
        }

        public void Update(string tweet)
        {
            _queue.Enqueue(tweet);
        }
    }
}