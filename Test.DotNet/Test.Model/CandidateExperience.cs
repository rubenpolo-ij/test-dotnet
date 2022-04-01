using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Test.Model
{
    public class CandidateExperience : IAuditable
    {
        public int IdCandidateExperience { get; set; }

        public int IdCandidate { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "{0} cannot exceed {1} characters")]
        public string Company { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "{0} cannot exceed {1} characters")]
        public string Job { get; set; }

        [Required]
        [MaxLength(4000, ErrorMessage = "{0} cannot exceed {1} characters")]
        public string Description { get; set; }

        [Required]
        public decimal? Salary { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "The BeginDate field is invalid")]
        public DateTime? BeginDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "The BeginDate field is invalid")]
        public DateTime? EndDate { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime? ModifyDate { get; set; }
    }
}
