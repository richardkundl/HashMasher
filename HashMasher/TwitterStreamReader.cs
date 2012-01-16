using Twitterizer;
using Twitterizer.Streaming;
using log4net;

namespace HashMasher
{
    public class TwitterStreamReader
    {
        protected readonly ILog _logger = LogManager.GetLogger("TwitterStreamReader");

        public void Execuite()
        {
            var streamOptions = new StreamOptions();
            streamOptions.Track.Add("codemash");

            var stream = new TwitterStream(Constants.PrflockOAuthTokens, "hashMasher", streamOptions);

            stream.StartPublicStream(StreamErrorCallback, 
                StatusCreatedCallback, 
                StatusDeletedCallback, 
                EventCallback, 
                RawJsonCallback);

            //stream.EndStream();
        }

        private void RawJsonCallback(string json)
        {
            //throw new NotImplementedException();
        }

        private void EventCallback(TwitterStreamEvent eventDetails)
        {
            _logger.Debug(eventDetails.EventType);
        }

        private void StatusDeletedCallback(TwitterStreamDeletedEvent status)
        {
            _logger.Debug("status deleted");
        }

        private void StatusCreatedCallback(TwitterStatus status)
        {
            _logger.Debug("Logging Message: " + status.Text);
        }

        private void StreamErrorCallback(StopReasons stopreason)
        {
            _logger.Debug("service down");
        }
    }
}
