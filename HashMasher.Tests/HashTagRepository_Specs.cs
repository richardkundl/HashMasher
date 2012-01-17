using System.Linq;
using FluentAssertions;
using HashMasher.Model;
using HashMasherRunner;
using NUnit.Framework;

namespace HashMasher.Tests
{
    [TestFixture]
    public class HashTagRepository_Specs
    {

        [Test]
        public void should_contain_codemash()
        {
            Bootstrapper.Run();

            var config = Container.Windsor.Resolve<IApplicationConfiguration>();
            config.HashTags.Should().NotBeNull();
            var tagList = config.HashTags.Split(',').ToList();
            tagList.Contains("codemash").Should().BeTrue();

        }
    }
}