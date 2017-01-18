using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class InputTester
    {
        public static bool isValidInput(List<string> input)
        {
            List<string> matches = input.Where(s => s != null && !s.Equals("null") && !s.Equals("")).ToList();
            return input.Count == matches.Count;
        }

        public static bool isLegalPassword(string password)
        {
            return isValidInput(new List<string>() { password }) && 5 <= password.Length && password.Length <= 15 && password.All(c => Char.IsLetterOrDigit(c));
        }

        public static bool isLegalEmail(string eMail)
        {
            if (!isValidInput(new List<string>() { eMail }) || eMail.Count(c => c == '@') != 1 || eMail.StartsWith("@") || eMail.EndsWith("@") || eMail.EndsWith("."))
            {
                return false;
            }
            string eMailSuffix = eMail.Substring(eMail.IndexOf('@') + 1);
            if (!eMailSuffix.Contains('.') || eMailSuffix.StartsWith("."))
            {
                return false;
            }
            return true;
        }
    }
}
