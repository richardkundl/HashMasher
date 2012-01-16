using System.Linq;
using ProMongoRepository;
using Twitterizer;
using Twitterizer.Entities;

namespace HashMasher
{
    public interface IDataGateway
    {
        void ProcessStatus(TwitterStatus status);
    }

    /// <summary>
    /// Ha! What a dumb name for a class.
    /// This class inspects the tweets (or whatever data) to see if it should be added to the database.
    /// We only add each link once.
    /// </summary>
    public class DataGateway : IDataGateway
    {

        private readonly IMongoRepository<LoggedLink> _tweetRepository;

        public DataGateway(IMongoRepository<LoggedLink> tweetRepository)
        {
            _tweetRepository = tweetRepository;
        }


        public void ProcessStatus(TwitterStatus status)
        {
          
            // Exit the method if there are no entities
            if (status.Entities == null)
                return;

            var entitiesSorted = status.Entities.OrderBy(e => e.StartIndex).Reverse();
            foreach (var entity in entitiesSorted)
            {
                var loggedStatus = new LoggedStatus
                {
                    CreatedDate = status.CreatedDate,
                    Source = status.Source,
                    Text = status.Text,
                    User = status.User.ScreenName
                };

                var urlEntity = entity as TwitterUrlEntity;
                if (urlEntity != null)
                {
                    var foundLink = _tweetRepository.Linq().FirstOrDefault(x => x.Link == urlEntity.Url);
                    if(foundLink==null)
                    {
                        var newLink = new LoggedLink { Link = urlEntity.Url };
                        newLink.StatusContainingLink.Add(loggedStatus);
                        _tweetRepository.Save(newLink);
                    } else
                    {
                        foundLink.StatusContainingLink.Add(loggedStatus);
                        _tweetRepository.Save(foundLink);
                    }
                    
                }
            }
        }
    }
}