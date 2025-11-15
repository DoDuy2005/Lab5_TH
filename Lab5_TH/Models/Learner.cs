
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab5_TH.Models
{
    public class Learner
    {
        public Learner()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        public int LearnerID { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        public int MajorID { get; set; }

        public virtual Major? Major { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
