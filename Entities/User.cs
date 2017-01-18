using Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class User 
    {
        [Key]
        public string UserId { get; set; }
        [Required]
        public string userPassword { get; set; }
        [Required]
        public string userMedicalTraining { get; set; }
        [Required]
        public string userFirstName { get; set; }
        [Required]
        public string userLastName { get; set; }
        [Required]
        public int uniqueInt { get; set; }
    }
}
