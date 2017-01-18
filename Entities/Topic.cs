using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Topic
    {
        [Key]
        public string TopicId { get; set; }
        [Required]
        public string subjectName { get; set; }
        [ForeignKey("subjectName")]
        public virtual Subject subject { get; set; }
        [Required]
        public DateTime timeAdded { get; set; }
    }
}
