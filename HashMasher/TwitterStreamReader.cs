using System.Linq;
using HashMasher.Model;
using ProMongoRepository;
using Twitterizer;
using Twitterizer.Streaming;
using log4net;

namespace HashMasher
{
    public class TwitterStreamReader
    {
        protected readonly ILog _logger = LogManager.GetLogger("TwitterStreamReader");


        public void StartService()
        {
            var streamOptions = new UserStreamOptions();
            var config = Container.Windsor.Resolve<IApplicationConfiguration>();

            
            streamOptions.Track.AddRange(config.HashTags.Split(',').ToList());

            var stream = new TwitterStream(Constants.OAuthTokens, "hashMasher", streamOptions);

            stream.StartPublicStream(StreamErrorCallback, 
                StatusCreatedCallback, 
                StatusDeletedCallback, 
                EventCallback, 
                RawJsonCallback);

            //stream.EndStream();
            _logger.Debug("service up");
            TwitterDirectMessage.Send(Constants.OAuthTokens, "detroitpro", "hashmash is UP");
        }

        public void StopService()
        {

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
            var dataGateway = Container.Windsor.Resolve<IDataGateway>();
            dataGateway.ProcessStatus(status);
        }

        private void StreamErrorCallback(StopReasons stopreason)
        {
            _logger.Debug("service down");
            TwitterDirectMessage.Send(Constants.OAuthTokens, "detroitpro", "hashmash is down");
        }
    }
}
