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


        public string LastText()
        {
            var tweet = StatusContainingLink.LastOrDefault();
            if(tweet!=null)
            {
                return tweet.HtmlText;
            }
            return "WAT?";
        }

    }
}