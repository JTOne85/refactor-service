using Adv;
using BadProject;
using NUnit.Framework;
using Moq;
using System;
using ThirdParty;

namespace TestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var cacheManager = new Mock<ICacheManager>();
            var errorManager = new Mock<IErrorManager>();
            var dateTime = new DateTimeOffset(DateTime.UtcNow);

            var provider = new Mock<IAdvProviderBuilder>();
            provider.Setup(x => x.CreateAdvProvider("some_random_id", false)).Returns(new Advertisement { WebId = "some_random_id" });
            provider.Setup(x => x.BuildProvider(new Advertisement { WebId = "some_random_id"}, errorManager.Object)).Returns(new Advertisement());


            cacheManager.Setup(x => x.Get("")).Returns(new ThirdParty.Advertisement());

            var service = new AdvertisementService(cacheManager.Object, errorManager.Object);

            var result = service.GetAdvertisement("some_id");

            Assert.IsNotNull(result);


        }
    }
}