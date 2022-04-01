using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Model;

namespace Test.Persistence.DB.Configuration
{
    internal class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
    {
        public void Configure(EntityTypeBuilder<Candidate> builder)
        {
            builder.HasKey(b => b.IdCandidate);

            builder.Property(b => b.Name).HasMaxLength(50).IsRequired();
            builder.Property(b => b.Surname).HasMaxLength(150).IsRequired();
            builder.Property(b => b.Birthdate).IsRequired();
            builder.Property(b => b.Email).HasMaxLength(250).IsRequired();
            builder.Property(b => b.InsertDate).IsRequired();
            builder.Property(b => b.ModifyDate);

            builder.HasIndex(b => b.Email).IsUnique(true);
            builder.HasMany(b => b.Experiences).WithOne()
                   .HasForeignKey(b=>b.IdCandidate);
        }
    }
}
