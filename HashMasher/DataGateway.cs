using System;
using System.Linq;
using HashMasher.Model;
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
        private readonly IApplicationConfiguration _configuration;

        public DataGateway(IMongoRepository<LoggedLink> tweetRepository, IApplicationConfiguration configuration)
        {
            _tweetRepository = tweetRepository;
            _configuration = configuration;
        }


        public void ProcessStatus(TwitterStatus status)
        {

            // Exit the method if there are no entities
            if (status.Entities == null)
                return;

            var foundHashTags = _configuration.HashTags.Split(',').AsEnumerable().Where(x => status.Text.ToLowerInvariant().Contains(x.ToLowerInvariant())).ToList();

            var entitiesSorted = status.Entities.OrderBy(e => e.StartIndex).Reverse();
            foreach (var entity in entitiesSorted)
            {
                var loggedStatus = new LoggedStatus
                {
                    CreatedDate = status.CreatedDate,
                    Source = status.Source,
                    Text = status.Text,
                    User = status.User.ScreenName,
                    UserImage = status.User.ProfileImageLocation,
                    HtmlText = status.LinkifiedText(),
                    TweetId = status.StringId
                };

                var urlEntity = entity as TwitterUrlEntity;
                if (urlEntity != null)
                {
                    var foundLink = _tweetRepository.Linq().FirstOrDefault(x => x.Link == urlEntity.Url);
                    if (foundLink == null)
                    {

                        var newLink = new LoggedLink
                                          {
                                              Link = urlEntity.Url, 
                                              Created = DateTime.Now,
                                              NumberOfTweets = 1
                                          };
                        newLink.StatusContainingLink.Add(loggedStatus);
                        newLink.HashTags.AddRange(foundHashTags);
                        _tweetRepository.Save(newLink);

                    }
                    else
                    {
                        foundLink.Modified = DateTime.Now;
                        if(foundLink.Created==null)
                        {
                            foundLink.Created = DateTime.Now;
                        }
                        foundLink.NumberOfTweets = foundLink.StatusContainingLink.Count() + 1;
                        foundLink.StatusContainingLink.Add(loggedStatus);
                        _tweetRepository.Save(foundLink);
                    }

                }
            }
        }
    }
}