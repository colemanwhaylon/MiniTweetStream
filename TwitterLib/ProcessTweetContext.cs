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
        private ITweetProcessor _tweetProcessor;

        public ProcessTweetContext(ITweetProcessor tweetProcessor)
        {
            _tweetProcessor = tweetProcessor;
        }

        public void SetStrategy(ITweetProcessor tweetProcessor)
        {
            _tweetProcessor = tweetProcessor;
        }

        public void ProcessTweet(string tweet)
        {
            _tweetProcessor.ProcessTweet(tweet);
        }
    }
}
