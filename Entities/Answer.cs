using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Answer
    {
        [Key]
        [Column(Order = 1)]
        public int questionId { get; set; }
        [ForeignKey("questionId")]
        public virtual Question question { get; set; }
        [Key]
        [Column(Order = 2)]
        public string userId { get; set; }
        [ForeignKey("userId")]
        public virtual User user { get; set; }
        [Key]
        [Column(Order = 3)]
        public DateTime timeAdded { get; set; }
        [Required]
        public int questionLevel { get; set; }
        [Required]
        public int userLevel { get; set; }
        [Required]
        public bool isCorrectAnswer { get; set; }
        [Required]
        public bool normal { get; set; }
        [Required]
        public int normalityCertainty { get; set; }
        public virtual ICollection<Tuple<string, int>> diagnoses { get; set; }
    }
}
