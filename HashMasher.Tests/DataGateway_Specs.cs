using System;
using FluentAssertions;
using NUnit.Framework;

namespace HashMasher.Tests
{
     [TestFixture]
    public class DataGateway_Specs
     {
         private IDataGateway _dataGateway;

         [SetUp]
         public void Setup()
         {
             //DataGateway(IMongoRepository<LoggedLink> tweetRepository, IApplicationConfiguration configuration)
             _dataGateway = new DataGateway(null,null, null);
         }

         [Test]
        public void GetExpandedLink_should_return_correct_uri()
         {
             var returnedUrl = _dataGateway.GetExpandedLink("https://t.co/VR3Sbz9B");
             returnedUrl.Should().Be("https://www.destroyallsoftware.com/talks/wat");
         }
    }
}