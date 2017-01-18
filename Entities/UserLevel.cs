using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class UserLevel
    {
        [Key]
        [Column(Order = 1)]
        public string userId { get; set; }
        [ForeignKey("userId")]
        public virtual User user { get; set; }
        [Key]
        [Column(Order = 2)]
        public string subjectName { get; set; }
        [ForeignKey("subjectName")]
        public virtual Subject subject { get; set; }
        [Key]
        [Column(Order = 3)]
        public string topicName { get; set; }
        [ForeignKey("topicName")]
        public virtual Topic topic { get; set; }
        [Required]
        public int level { get; set; }
        [Required]
        public int timesAnswered { get; set; }
        [Required]
        public int timesAnsweredCorrectly { get; set; }
    }
}
