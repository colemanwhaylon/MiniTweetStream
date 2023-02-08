using TwitterLib;
using TwitterLib.Interface;

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
        public async Task ShouldBeAbleToStartReceivingTweets()
        {
            IDataSink dataSink = new DataSink();
            List<IDataSink> dataSinkList = new List<IDataSink>();
            dataSinkList.Add(dataSink); 
            await TwitterClient.StartReceivingTweets(dataSinkList);
        }

        //[Fact]
        //public void ShouldBeAbleToStopReceivingTweets()
        //{
        //    var x = TwitterClient.StopReceivingTweets();
        //}
    }
}