using System;
using FluentAssertions;
using HashMasher.Model;
using HashMasherRunner;
using NUnit.Framework;
using ProMongoRepository;

namespace HashMasher.Tests
{
     [TestFixture]
    public class DataGateway_Specs
     {
         private IDataGateway _dataGateway;

         [SetUp]
         public void Setup()
         {
             Bootstrapper.Run();
             _dataGateway = Container.Windsor.Resolve<IDataGateway>();
         }

         [Test]
        public void GetExpandedLink_should_return_correct_uri()
         {
             var returnedUrl = _dataGateway.GetExpandedLink("https://t.co/VR3Sbz9B");
             returnedUrl.Should().Be("https://www.destroyallsoftware.com/talks/wat");
         }


         [Test]
         public void ProcessBatch()
         {
            _dataGateway.ProcessBatch();
         }
    }
}