using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace messagesClasses
{
    public class answerData
    {
        public bool normalOrNot { get; set; }
        public int sure1 { get; set; }
        public List<string> diagnosis { get; set; }
        public List<int> sure2 { get; set; }


        public string isDataReasonable()
        {
            if ((normalOrNot.Equals("abnormal")) && (sure1 != null))
            {
                if ((diagnosis.Count > 0) && (sure2 != null))
                {
                    return "success";
                }
                return "error, invalid data";
            }
            if (normalOrNot.Equals("abnormal"))
            {
                return "error, missing data";
            }
            return "success";
        }

        public bool normToBool()
        {
            if (normalOrNot.Equals("normal"))
            {
                return true;
            }
            return false;
        }

    }
}
