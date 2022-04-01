using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Test.Model;

namespace Test.Persistence.DB
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
              : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Candidate> Candidates { get; set; }

        public DbSet<CandidateExperience> CandidateExperiences { get; set; }
    }
}
