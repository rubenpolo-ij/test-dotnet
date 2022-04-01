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
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository candidateRepository;
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<CandidateService> logger;
        private readonly IConfiguration configuration;

        public CandidateService(
             ICandidateRepository candidateRepository,
             IMemoryCache memoryCache,
             ILogger<CandidateService> logger,
             IConfiguration configuration)
        {
            this.candidateRepository = candidateRepository ?? throw new ArgumentNullException(nameof(candidateRepository));
            this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public List<Candidate> GetAll()
        {
            string cacheKey = "all-candidates";
            List<Candidate> candidates;

            double expiration = double.Parse(configuration["Cache:Expiration"]);

            logger.LogDebug("Getting all candidates...");

            if (!memoryCache.TryGetValue(cacheKey, out candidates))
            {
                candidates = candidateRepository.GetAll();
                var offset = TimeSpan.FromSeconds(expiration);
                var options = new MemoryCacheEntryOptions().SetSlidingExpiration(offset);
                memoryCache.Set(cacheKey, candidates, options);
                logger.LogInformation("Got all candidates");
            }

            return candidates;
        }

        public Candidate GetById(int id)
        {
            string cacheKey = $"candidate-{id}";
            Candidate candidate;

            double expiration = double.Parse(configuration["Cache:Expiration"]);

            logger.LogDebug("Getting candidate...");

            if (!memoryCache.TryGetValue(cacheKey, out candidate))
            {
                candidate = candidateRepository.GetById(id);
                var offset = TimeSpan.FromSeconds(expiration);
                var options = new MemoryCacheEntryOptions().SetSlidingExpiration(offset);
                memoryCache.Set(cacheKey, candidate, options);
                logger.LogInformation($"Got candidate by id: {id}");
            }

            return candidate;
        }

        public void Add(Candidate candidate)
        {
            candidate.InsertDate = DateTime.Today;
            candidateRepository.Add(candidate);
            candidateRepository.SaveChanges();
        }

        public void Delete(Candidate candidate)
        {
            candidateRepository.Delete(candidate);
            candidateRepository.SaveChanges();
        }

        public void Update(Candidate candidate)
        {
            candidate.ModifyDate = DateTime.Today;
            candidateRepository.Update(candidate);
            candidateRepository.SaveChanges();
        }
    }
}
