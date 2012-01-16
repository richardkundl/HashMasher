using System.Collections.Generic;
using Norm;

namespace HashMasher
{
    public class LoggedLink
    {
        public LoggedLink()
        {
            StatusContainingLink = new List<LoggedStatus>();
        }

        public ObjectId Id { get; set; }
        public string Link { get; set; }
        public IList<LoggedStatus> StatusContainingLink { get; set; }
    }
}