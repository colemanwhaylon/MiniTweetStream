namespace TwitterLib
{
    public class TwitterClientOptions
    {
        private readonly string _bearerToken;
        private readonly CancellationToken _cancellationToken;
        private readonly string _twitterAPIUrl;

        public TwitterClientOptions(string bearerToken,
            CancellationToken cancellationToken,
            string twitterAPIUrl)
        {
            this._bearerToken = bearerToken;
            this._cancellationToken = cancellationToken;
            _twitterAPIUrl = twitterAPIUrl;
        }

        public string BearerToken => _bearerToken;

        public CancellationToken CancellationToken => _cancellationToken;

        public string TwitterAPIUrl => _twitterAPIUrl;
    }
}