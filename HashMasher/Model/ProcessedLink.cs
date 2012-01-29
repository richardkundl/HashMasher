using System;
using System.Collections.Generic;
using System.Linq;
using Norm;

namespace HashMasher.Model
{
    public class ProcessedLink
    {
        public ProcessedLink()
        {
            StatusContainingLink = new List<LoggedStatus>();
        }

        public ObjectId Id { get; set; }
        public string Link { get; set; }
        public string ExpandedLink { get; set; }
        public IList<LoggedStatus> StatusContainingLink { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public int NumberOfTweets { get; set; }
        public bool Processed { get; set; }
        public string HashTag { get; set; }

        public string Flair
        {
            get
            {
                //this is gross but whateva.
                //If the tweet was crated today.
                if ((DateTime.Now - Created).Days < 2)
                {
                    return "<span class='label success'>New</span>";
                }
                return string.Empty;
            }
        }


        public string LastText()
        {
            var tweet = StatusContainingLink.OrderBy(x=>x.CreatedDate).LastOrDefault();
            return tweet!=null ? tweet.HtmlText : "WAT?";
        }

        public string LastDetails()
        {
            var tweet = StatusContainingLink.LastOrDefault();
            if (tweet != null)
            {
                return String.Format("<a href='http://twitter.com/{0}' target='_blank'>@{0}</a> {1}", tweet.User, String.Format("{0:g}", tweet.CreatedDate));
            }
            return "";
        }

        public string LastUserImage()
        {
            var tweet = StatusContainingLink.OrderBy(x=>x.CreatedDate).LastOrDefault();
            if (tweet != null)
            {
                return String.Format("<img height='36' width='36' src='{0}' alt='{1}' />", tweet.UserImage, tweet.User);
            }
            return "";
        }

        public string FirstDetails()
        {
            var tweet = StatusContainingLink.OrderBy(x=>x.CreatedDate).FirstOrDefault();
            if (tweet != null)
            {
                return String.Format("{0} by <a href='http://twitter.com/{1}' target='_blank'>@{1}</a>", String.Format("{0:g}", tweet.CreatedDate), tweet.User);
            }
            return "";
        }

     
    }
}