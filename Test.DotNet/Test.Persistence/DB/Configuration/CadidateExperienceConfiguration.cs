using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Model;

namespace Test.Persistence.DB.Configuration
{
    internal class CadidateExperienceConfiguration : IEntityTypeConfiguration<CandidateExperience>
    {
        public void Configure(EntityTypeBuilder<CandidateExperience> builder)
        {
            builder.HasKey(b => b.IdCandidateExperience);
            builder.Property(b => b.Company).HasMaxLength(100).IsRequired();
            builder.Property(b => b.Job).HasMaxLength(100).IsRequired();
            builder.Property(b => b.Description).HasMaxLength(4000).IsRequired();
            builder.Property(b => b.Salary).HasColumnType("decimal(8,2)").IsRequired();
            builder.Property(b => b.BeginDate).IsRequired();
            builder.Property(b => b.EndDate);
            builder.Property(b => b.InsertDate).IsRequired();
            builder.Property(b => b.ModifyDate);
        }
    }
}
