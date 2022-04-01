using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Test.Model;
using Test.Persistence.DB;

namespace Test.Persistence.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly Context context;

        public CandidateRepository(Context context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<Candidate> GetAll()
        {
            return context.Candidates
                          .AsNoTracking().ToList();
        }

        public Candidate GetById(int id)
        {
            return context.Candidates
                          .Where(c => c.IdCandidate == id)
                          .AsNoTracking().FirstOrDefault();
        }

        public void Add(Candidate candidate)
        {
            context.Candidates.Add(candidate);
        }

        public void Delete(Candidate candidate)
        {
            context.Candidates.Remove(candidate);
        }

        public void Update(Candidate candidate)
        {
            context.Candidates.Update(candidate);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
