using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        [Required]
        public string adminId { get; set; }
        [ForeignKey("adminId")]
        public virtual Admin admin { get; set; }
        public virtual ICollection<User> members { get; set; }
        public virtual ICollection<User> invitedUsers { get; set; }
        public virtual ICollection<Test> tests { get; set; }
    }
}
