using System;
using System.Collections.Generic;
using System.Linq;
using HashMasher.Model;
using ProMongoRepository;
using RestSharp;
using Twitterizer;
using Twitterizer.Entities;

namespace HashMasher
{
    public interface IDataGateway
    {
        void ProcessStatus(TwitterStatus status);
        string GetExpandedLink(string url);
        void ProcessBatch();
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
        private readonly IMongoRepository<ProcessedLink> _processedLinkRepository;

        public DataGateway(IMongoRepository<LoggedLink> tweetRepository, IApplicationConfiguration configuration, IMongoRepository<ProcessedLink> processedLinkRepository )
        {
            _tweetRepository = tweetRepository;
            _configuration = configuration;
            _processedLinkRepository = processedLinkRepository;
        }


        public void ProcessStatus(TwitterStatus status)
        {
           
            

            // Exit the method if there are no entities
            if (status.Entities == null)
                return;

            var hashTag = _configuration.HashTags.Split(',').AsEnumerable().Where(x => status.Text.ToLowerInvariant().Contains(x.ToLowerInvariant())).FirstOrDefault();

            var entitiesSorted = status.Entities.OrderBy(e => e.StartIndex).Reverse();
            foreach (var entity in entitiesSorted)
            {

                //var loggedStatus = new LoggedStatus();

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
                    var expandedLink = GetExpandedLink(urlEntity.Url);
                    var foundLink = _tweetRepository.Linq().FirstOrDefault(x => x.ExpandedLink == expandedLink);
                    if (foundLink == null)
                    {

                        var newLink = new LoggedLink
                                          {
                                              Link = urlEntity.Url, 
                                              ExpandedLink = expandedLink,
                                              Created = DateTime.Now,
                                              NumberOfTweets = 1,
                                              Processed = true
                                          };
                        newLink.StatusContainingLink.Add(loggedStatus);
                        newLink.HashTag = hashTag;
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
            ProcessBatch();
        }

        public void ProcessBatch()
        {
            var unprocessed = _tweetRepository.Linq().Where(x => x.Processed!=null && x.Processed !=true).ToList();
            ProcessRawUrlUpdates(unprocessed);
        }


        public void ProcessRawUrlUpdates(IList<LoggedLink> links)
        {

            foreach (var loggedLink in links)
            {
                var expanded = GetExpandedLink(loggedLink.Link);

                loggedLink.ExpandedLink = expanded;
                loggedLink.Processed = true;
                _tweetRepository.Update(loggedLink.Id, loggedLink);


                var found = _processedLinkRepository.Linq().FirstOrDefault(x => x.ExpandedLink == expanded);
                if(found==null)
                {
                    var processedLink = AutoMapper.Mapper.DynamicMap<LoggedLink, ProcessedLink>(loggedLink);
                    processedLink.Processed = true;
                    _processedLinkRepository.Save(processedLink);
                } else
                {
                    found.Modified = DateTime.Now;
                    found.StatusContainingLink.Add(loggedLink.StatusContainingLink.FirstOrDefault());
                    found.NumberOfTweets = found.StatusContainingLink.Count();
                    found.Processed = true;
                    _processedLinkRepository.Save(found);
                }
            }
        }

        public string GetExpandedLink(string url)
        {
            var client = new RestClient("http://api.longurl.org/v2/expand");
            var restRequest = new RestRequest();
            
            restRequest.AddParameter("format", "json");
            restRequest.AddParameter("url", url);
            var response = client.Execute<ExpandedLink>(restRequest);
            return response.Data.longUrl;
        }
    }
}