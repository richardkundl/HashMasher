using NUnit.Framework;

namespace HashMasher.Tests
{
    [TestFixture]
    public class TwitterStreamReader_Specs
    {

        [Test]
        public void should_connect_to_twitter()
        {
            var twitterStreamReader = new TwitterStreamReader();
            twitterStreamReader.StartService();
        }
    }
}
