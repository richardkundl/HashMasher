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
        public DateTime? Created { get; set; }
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
                if(Created.HasValue && (Created.Value - DateTime.Now).Days < 1)
                {
                    return "<span class='label success'>New</span>";
                }
                return string.Empty;
            }
        }


        public string LastText()
        {
            var tweet = StatusContainingLink.LastOrDefault();
            return tweet!=null ? tweet.HtmlText : "WAT?";
        }

        public string LastUserImage()
        {
            var tweet = StatusContainingLink.LastOrDefault();
            if (tweet != null)
            {
                return String.Format("<img height='24' width='24' src='{0}' alt='{1}' />", tweet.UserImage, tweet.User);
            }
            return "";
        }
    }
}