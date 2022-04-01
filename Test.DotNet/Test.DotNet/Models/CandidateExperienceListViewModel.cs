using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.DotNet.Models
{
    public class CandidateExperienceListViewModel
    {
        public int IdCandidate { get; set; }
        public int IdCandidateExperience { get; set; }
        public string Company { get; set; }
        public string Job { get; set; }
        public string Description { get; set; }
        public decimal Salary { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
    }
}
