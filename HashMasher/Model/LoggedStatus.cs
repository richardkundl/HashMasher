using System;

namespace HashMasher.Model
{
    /// <summary>
    /// This is the almost raw response from twitter. mapped w/o some of the many items we don't need.
    /// </summary>
    public class LoggedStatus
    {
        public DateTime CreatedDate { get; set; }
        public string Source { get; set; }
        public string Text { get; set; }
        public string HtmlText { get; set; }
        public string User { get; set; }
        public string TweetId { get; set; }
    }
}