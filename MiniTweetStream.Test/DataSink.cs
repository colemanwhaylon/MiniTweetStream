using TwitterLib.Interface;

namespace MiniTweetStream.Test
{
    public class DataSink : IDataSink
    {
        public void RecieveTweet(string tweet)
        {
            Console.WriteLine(tweet);   
        }
    }
}