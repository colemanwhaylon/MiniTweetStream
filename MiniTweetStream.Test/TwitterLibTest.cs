using TwitterLib;

namespace MiniTweetStream.Test
{
    public class TwitterLibTest
    {
        private TwitterClient _twitterClient = null;

        public TwitterLibTest()
        {
            _twitterClient = TwitterClient.GetInstance;
        }


        [Fact]
        public void ShouldBeAbleToStartReceivingTweets()
        {
            var x = TwitterClient.GetTweets();
        }

        //[Fact]
        //public void ShouldBeAbleToStopReceivingTweets()
        //{
        //    var x = TwitterClient.StopReceivingTweets();
        //}
    }
}