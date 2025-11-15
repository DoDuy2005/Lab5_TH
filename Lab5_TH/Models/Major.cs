
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab5_TH.Models
{
    public class Major
    {
        public Major()
        {
            Learners = new HashSet<Learner>();
        }

        public int MajorID { get; set; }

        [Required]
        [StringLength(50)]
        public string MajorName { get; set; }

        public virtual ICollection<Learner> Learners { get; set; }
    }
}
