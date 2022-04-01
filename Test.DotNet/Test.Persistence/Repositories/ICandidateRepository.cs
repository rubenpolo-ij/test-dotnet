using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Test.Model;

namespace Test.Persistence.Repositories
{
    public interface ICandidateRepository
    {
        List<Candidate> GetAll();

        Candidate GetById(int id);

        void Add(Candidate candidate);

        void Delete(Candidate candidate);

        void Update(Candidate candidate);
        void SaveChanges();
    }
}
