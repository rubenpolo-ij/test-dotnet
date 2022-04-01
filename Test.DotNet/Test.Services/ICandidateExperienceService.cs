using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Test.Model;

namespace Test.Services
{
    public interface ICandidateExperienceService
    {
       List<CandidateExperience> GetByIdCandidate(int idCandidate);

       CandidateExperience GetById(int idCandidate, int id);

       void Add(CandidateExperience candidateExperience);

       void Delete(CandidateExperience candidateExperience);

       void Update(CandidateExperience candidateExperience);

    }
}
