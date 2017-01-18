using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class GroupTestAnswer
    {
        public virtual User user { get; set; }
        [Key]
        [Column(Order = 1)]
        public int groupId { get; set; }
        [ForeignKey("groupId")]
        public virtual Group group { get; set; }
        [Key]
        [Column(Order = 2)]
        public int testId { get; set; }
        [ForeignKey("testId")]
        public virtual Test test { get; set; }
        [Key]
        [Column(Order = 3)]
        public virtual Answer answer { get; set; }
    }
}
