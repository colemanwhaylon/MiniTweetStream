using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterLib.Interface;

namespace TwitterLib
{
    public class ProcessTweetContext
    {
        private IObserver _tweetProcessor;

        public ProcessTweetContext(IObserver tweetProcessor)
        {
            _tweetProcessor = tweetProcessor;
        }

        public void SetStrategy(IObserver tweetProcessor)
        {
            _tweetProcessor = tweetProcessor;
        }

        public void ProcessTweet(string tweet)
        {
            _tweetProcessor.Update(tweet);
        }
    }
}
