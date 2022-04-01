using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Test.Model
{
    public class Candidate: IAuditable
    {
        public Candidate()
        {
            Experiences = new List<CandidateExperience>();
        }

        public int IdCandidate { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "{0} cannot exceed {1} characters")]
        public string Name { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "{0} cannot exceed {1} characters")]
        public string Surname { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "The Birthdate field is invalid")]
        public DateTime? Birthdate { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "The Email field is invalid")]
        [MaxLength(250, ErrorMessage = "{0} cannot exceed {1} characters")]
        public string Email { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public virtual List<CandidateExperience> Experiences { get; set; }
    }
}
