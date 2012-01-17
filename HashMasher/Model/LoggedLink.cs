using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    }
}