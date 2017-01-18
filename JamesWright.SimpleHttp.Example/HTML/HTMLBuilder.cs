using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI;

namespace JamesWright.SimpleHttp.Example.HTML
{
   public class HTMLBuilder
    {
      
       string lineBreak = Environment.NewLine;
       

       public HTMLBuilder()
       {
          
       }


       public string generateQuestionPage(int[] input)
       {
           string partOne = File.ReadAllText(@"..\\..\\HTML\questionTest.html");
           Regex regex = new Regex("{{values}}");
           string replaceWith ="";
           for(int i = 0; i < input.Length; i++){
               replaceWith = replaceWith + @"<option value="""+ i.ToString()+@""">"+i.ToString()+@"</option>"+lineBreak;
           }


           return regex.Replace(partOne, replaceWith);
       }
    


    }
}
