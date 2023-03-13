using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogSystem.MVCSite.Models
{
    public class KeywordsFilterHelper
    {
        private static List<string> keywords = new List<string>
        {
            "腐败","贪污","共产党","法轮功","港独"
        };
        public static string FilterString(string input)
        {
            foreach (string keyword in keywords)
            {
                string replacestr = "";
                for (int i = 0; i < keyword.Length; i++)
                {
                    replacestr += "*";
                }
                input = input.Replace(keyword, replacestr);
            }
            return input;
        }
    }
}