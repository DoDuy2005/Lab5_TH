
using System.ComponentModel.DataAnnotations;

namespace Lab5_TH.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }

        public int CourseID { get; set; }

        public int LearnerID { get; set; }

        [Range(0, 10)]
        public float Grade { get; set; }

        public virtual Learner? Learner { get; set; }
        public virtual Course? Course { get; set; }
    }
}
