using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Test.Model;
using Test.Persistence.DB;

namespace Test.Persistence.Repositories
{
    public class CandidateExperienceRepository : ICandidateExperienceRepository
    {
        private readonly Context context;

        public CandidateExperienceRepository(Context context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<CandidateExperience> GetByIdCandidate(int idCandidate)
        {
            return context.CandidateExperiences.Where(e => e.IdCandidate == idCandidate).AsNoTracking().ToList();
        }

        public CandidateExperience GetById(int idCandidate, int id)
        {
            return context.CandidateExperiences
                          .Where(c => c.IdCandidate == idCandidate && c.IdCandidateExperience == id)
                          .AsNoTracking().FirstOrDefault();
        }

        public void Add(CandidateExperience candidateExperience)
        {
            context.CandidateExperiences.Add(candidateExperience);
        }

        public void Delete(CandidateExperience candidateExperience)
        {
            context.CandidateExperiences.Remove(candidateExperience);
        }

        public void Update(CandidateExperience candidateExperience)
        {
            context.CandidateExperiences.Update(candidateExperience);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
