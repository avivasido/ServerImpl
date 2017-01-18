using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Admin
    {
        [Key]
        public string AdminId { get; set; }
        [ForeignKey("AdminId")]
        public virtual User user { get; set; }
    }
}
