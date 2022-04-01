using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Test.Model;

namespace Test.Services
{
    public interface ICandidateService
    {
        List<Candidate> GetAll();

        Candidate GetById(int id);

        void Add(Candidate candidate);

        void Delete(Candidate candidate);

        void Update(Candidate candidate);

    }
}
