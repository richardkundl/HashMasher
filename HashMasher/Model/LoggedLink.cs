using System;
using System.Collections.Generic;
using Norm;

namespace HashMasher.Model
{
    public class LoggedLink
    {
        public LoggedLink()
        {
            StatusContainingLink = new List<LoggedStatus>();
            HashTags = new List<string>();
        }

        public ObjectId Id { get; set; }
        public string Link { get; set; }
        public IList<LoggedStatus> StatusContainingLink { get; set; }
        public List<string> HashTags { get; set; }
        public DateTime? Created { get; set; }
        public DateTime Modified { get; set; }
        public int NumberOfTweets { get; set; }
    }
}