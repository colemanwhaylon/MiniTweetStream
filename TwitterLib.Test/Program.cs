
namespace TwitterLib.Driver
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            try
            {
                Console.WriteLine("TRY COMPLETE!");
            }
            finally
            {
                Console.WriteLine("DONE!");

            }
        }
    }
}

 /* curl -X GET "https://api.twitter.com/2/tweets/sample/stream" -H "Authorization: Bearer AAAAAAAAAAAAAAAAAAAAAC0elgEAAAAAsd2a0zCPJ26FBlpYdVZqprUl5ig%3D5Q5cik9u2O71FLpxPRYpWK5yohcG0nVt7IAcEVwsMyp3fPt3JK" 
 *
 */