using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        [Required]
        public string subjectName { get; set; }
        [ForeignKey("subjectName")]
        public virtual Subject subject { get; set; }
        [Required]
        public string topicName { get; set; }
        [ForeignKey("topicName")]
        [Required]
        public virtual Topic topic { get; set; }
        [Required]
        public bool normal { get; set; }
        [Required]
        public string text { get; set; }
        [Required]
        public DateTime timeAdded { get; set; }
        [Required]
        public int level { get; set; }
        [Required]
        public virtual ICollection<string> diagnoses { get; set; }
        [Required]
        public virtual ICollection<byte[]> images { get; set; }
        [Required]
        public int timesAnswered { get; set; }
        [Required]
        public int timesAnsweredCorrectly { get; set; }
    }
}
