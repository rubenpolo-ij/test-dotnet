using System;
using System.Collections.Generic;
using MemoryCache.Testing.Moq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Test.Model;
using Test.Persistence.Repositories;

namespace Test.Services.Tests
{
    public class CandidateServiceTests
    {
        [Test]
        public void CandidateServiceConstructor_WhenCandidateRepositoryIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new CandidateService(
                    null,
                    null,
                    null,
                    null));
        }

        [Test]
        public void CandidateServiceConstructor_WhenCacheIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new CandidateService(Mock.Of<ICandidateRepository>(),
                null,
                null,
                null));
        }

        [Test]
        public void CandidateServiceConstructor_WhenLoggerIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new CandidateService(
                    Mock.Of<ICandidateRepository>(),
                    Mock.Of<IMemoryCache>(),
                    null,
                    null));
        }

        [Test]
        public void CandidateServiceConstructor_WhenConfigurationIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new CandidateService(
                    Mock.Of<ICandidateRepository>(),
                    Mock.Of<IMemoryCache>(),
                    Mock.Of< ILogger<CandidateService>>(),
                    null));
        }

        [Test]
        public void CandidateServiceConstructor_WhenValidParameters_CreatesInstance()
        {
            object result = new CandidateService(
                    Mock.Of<ICandidateRepository>(),
                    Mock.Of<IMemoryCache>(),
                    Mock.Of<ILogger<CandidateService>>(),
                    Mock.Of<IConfiguration>());

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CandidateService>(result);
        }

        [Test]
        public void GetAll_CallsMemoryCache_TryGetValueOnce()
        {
            string cacheKey = "all-candidates";
            IMemoryCache memoryCacheMockObject = Create.MockedMemoryCache();
            var memoryCacheMock = Mock.Get(memoryCacheMockObject);

            Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(m => m["Cache:Expiration"]).Returns("10");

            CandidateService classUnderTest = new CandidateService(
                    Mock.Of<ICandidateRepository>(),
                    memoryCacheMock.Object,
                    Mock.Of<ILogger<CandidateService>>(),
                    configurationMock.Object);

            object result = classUnderTest.GetAll();
            object cacheEntryValue;
            memoryCacheMock.Verify(m => m.TryGetValue(cacheKey, out cacheEntryValue), Times.Once);
        }

        [Test]
        public void GetAll_WhenNotCachedValue_CallsCandidateRepository_GetAllOnce()
        {
            string cacheKey = "all-candidates";
            IMemoryCache memoryCacheMockObject = Create.MockedMemoryCache();
            var memoryCacheMock = Mock.Get(memoryCacheMockObject);

            Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(m => m["Cache:Expiration"]).Returns("10");
            
            Mock<ICandidateRepository> candidateRepositoryMock = new Mock<ICandidateRepository>();

            CandidateService classUnderTest = new CandidateService(
                    candidateRepositoryMock.Object,
                    memoryCacheMock.Object,
                    Mock.Of<ILogger<CandidateService>>(),
                    configurationMock.Object);

            object result = classUnderTest.GetAll();

            candidateRepositoryMock.Verify(m => m.GetAll(), Times.Once);
        }


        [Test]
        public void GetAll_WhenApiRepositoryResponseIsOk_ReturnsTaskOfListOfRateConversion()
        {
            string cacheKey = "all-candidates";
            IMemoryCache memoryCacheMockObject = Create.MockedMemoryCache();
            var memoryCacheMock = Mock.Get(memoryCacheMockObject);

            Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(m => m["Cache:Expiration"]).Returns("10");

            Mock<ICandidateRepository> candidateRepositoryMock = new Mock<ICandidateRepository>();

            List<Candidate> candidatesFake = GetCandidatesFake();
            candidateRepositoryMock.Setup(m => m.GetAll()).Returns(candidatesFake);

            CandidateService classUnderTest = new CandidateService(
                    candidateRepositoryMock.Object,
                    memoryCacheMock.Object,
                    Mock.Of<ILogger<CandidateService>>(),
                    configurationMock.Object);

            object result = classUnderTest.GetAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<Candidate>>(result);
            List<Candidate> castedResult = result as List<Candidate>;
            Assert.IsNotNull(castedResult);
            Assert.IsTrue(castedResult.Count == 2);
        }


        private List<Candidate> GetCandidatesFake()
        {
            return new List<Candidate>()
            {
                new Candidate()
                {
                    IdCandidate=1,
                    Name = "Name1",
                    Surname="Surname1",
                    Email="Email1@Email.com",
                    Birthdate = DateTime.Today,
                    InsertDate= DateTime.Today,
                    ModifyDate = null
                },
                new Candidate()
                {
                    IdCandidate=2,
                    Name = "Name2",
                    Surname="Surname2",
                    Email="Email2@Email.com",
                    Birthdate = DateTime.Today,
                    InsertDate= DateTime.Today,
                    ModifyDate = null
                }
            };
        }

    }
}