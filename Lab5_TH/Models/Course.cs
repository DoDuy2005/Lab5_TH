
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab5_TH.Models
{
    public class Course
    {
        public Course()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Course ID")]
        public int CourseID { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Range(1, 5)]
        public int Credits { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
