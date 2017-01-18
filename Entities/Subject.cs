using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Subject
    {
        [Key]
        public string SubjectId { get; set; }
        [Required]
        public DateTime timeAdded { get; set; }
        public virtual ICollection<Topic> topics { get; set; }
    }
}
