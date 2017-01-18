using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace messagesClasses
{
    public class registerData
    {
        public string mail { get; set; }
        public string password { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string medTraining { get; set; }

        public string isDataLegal()
        {
            if (mail == null | password == null | fname == null | lname == null | medTraining == null)
            {
                return "error, all data must be supplied";
            }
            return "success";

        }
    }
}
