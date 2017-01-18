using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Test
    {
        [Key]
        public int TestId { get; set; }
        public virtual ICollection<Question> questions { get; set; }
    }
}
