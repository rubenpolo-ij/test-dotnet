using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Test.Model;
using Test.Persistence.Repositories;

namespace Test.Services
{
    public class CandidateExperienceService : ICandidateExperienceService
    {
        private readonly ICandidateExperienceRepository candidateExperienceRepository;
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<CandidateService> logger;
        private readonly IConfiguration configuration;

        public CandidateExperienceService(
             ICandidateExperienceRepository candidateExperienceRepository,
             IMemoryCache memoryCache,
             ILogger<CandidateService> logger,
             IConfiguration configuration)
        {
            this.candidateExperienceRepository = candidateExperienceRepository ?? throw new ArgumentNullException(nameof(candidateExperienceRepository));
            this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public List<CandidateExperience> GetByIdCandidate(int idCandidate)
        {
            string cacheKey = "all-candidate-experiences";
            List<CandidateExperience> candidateExperiences;

            double expiration = double.Parse(configuration["Cache:Expiration"]);

            logger.LogDebug("Getting all candidate experiences...");

            if (!memoryCache.TryGetValue(cacheKey, out candidateExperiences))
            {
                candidateExperiences = candidateExperienceRepository.GetByIdCandidate(idCandidate);
                var offset = TimeSpan.FromSeconds(expiration);
                var options = new MemoryCacheEntryOptions().SetSlidingExpiration(offset);
                memoryCache.Set(cacheKey, candidateExperiences, options);
                logger.LogInformation("Got all candidate experiences");
            }

            return candidateExperiences;
        }

        public CandidateExperience GetById(int idCandidate, int id)
        {
            string cacheKey = $"candidate-experience-{id}";
            CandidateExperience candidateExperience;

            double expiration = double.Parse(configuration["Cache:Expiration"]);

            logger.LogDebug("Getting candidateExperience...");

            if (!memoryCache.TryGetValue(cacheKey, out candidateExperience))
            {
                candidateExperience = candidateExperienceRepository.GetById(idCandidate, id);
                var offset = TimeSpan.FromSeconds(expiration);
                var options = new MemoryCacheEntryOptions().SetSlidingExpiration(offset);
                memoryCache.Set(cacheKey, candidateExperience, options);
                logger.LogInformation($"Got candidate experience by id: {id}");
            }

            return candidateExperience;
        }

        public void Add(CandidateExperience candidateExperience)
        {
            candidateExperience.InsertDate = DateTime.Today;
            candidateExperienceRepository.Add(candidateExperience);
            candidateExperienceRepository.SaveChanges();
        }

        public void Delete(CandidateExperience candidateExperience)
        {
            candidateExperienceRepository.Delete(candidateExperience);
            candidateExperienceRepository.SaveChanges();
        }

        public void Update(CandidateExperience candidateExperience)
        {
            candidateExperience.ModifyDate = DateTime.Today;
            candidateExperienceRepository.Update(candidateExperience);
            candidateExperienceRepository.SaveChanges();
        }
    }
}
