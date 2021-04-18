using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace tstRegExp
{
    class Program
    {
        static void Main(string[] args)
        {

            string instr = "2020/01/05";
            string pat = @"(\d{2})";

            bool ism = IsMatch(instr, pat);
            Console.WriteLine(ism.ToString());

            var res = MatchSimple(instr, pat, 0, 1);
            Console.WriteLine(res);

            pat = @"^(\d{4}).?(\d{2}).?(\d{2})$";
            res = MatchConstructor(instr, pat, 0, @"(10)-(2)-(1)");
            Console.WriteLine(res);

            //Простой метод возвращающий булево значение True если паттерн хотя бы раз имеет совпадение со строкой
            //Параметры:
            //str - строка распознования
            //pat - паттерн регулярного выражения
            static bool IsMatch(string str, string pat)
            {
                bool m = false;
                m = Regex.IsMatch(str, pat, RegexOptions.IgnoreCase);
                return m;
            }

            //Метод возвращающий строку в определенном формате, состоящую из групп вхождения. 
            //Параметры:
            //str - строка распознования
            //pat - паттерн регулярного выражения
            //MatchNum - номер вхождения, по умолчанию берется первое вхождение
            //returnpatstr - формат выводимой строки, где в скобках указаны номер группы вхождения. По умолчанию берется нулевая группа, т.е. весь результат вхождения
            public static string MatchConstructor(string str, string pat, int MatchNum = 0, string returnpatstr = "(0)")
            {
                string res = "";
                List<int> grps = new List<int>();
                MatchCollection matches = Regex.Matches(returnpatstr, @"(?<=\()\d+?(?=\))", RegexOptions.IgnoreCase);
                foreach (var m in matches)
                {
                    if (int.TryParse(m.ToString(), out var num))
                    {
                        grps.Add(num);
                    }
                }

                MatchCollection matchesmain = Regex.Matches(str, pat, RegexOptions.IgnoreCase);
                if (matchesmain.Count >= MatchNum + 1)
                {
                    Match match = matchesmain[MatchNum];
                    foreach (var g in grps)
                    {
                        res = returnpatstr.Replace($"({g.ToString()})", match.Groups[g].Value);
                    }
                }
                return res;
            }

            //Метод возвращающий по порядковому номеру результат вхождения полностью или результат вхождения группы по порядковому номеру 
            //Параметры:
            //str - строка распознования
            //pat - паттерн регулярного выражения
            //MatchNum - номер вхождения, по умолчанию берется первое вхождение
            //subMatchNum - номер группы вхождения. По умолчанию берется нулевая группа, т.е. весь результат вхождения
            static string MatchGroups(string str, string pat, int MatchNum = 0, int groupNum = 0)
            {
                string matchStr = "";
                Match match;
                MatchCollection matches = Regex.Matches(str, pat, RegexOptions.IgnoreCase);
                if (matches.Count >= MatchNum + 1)
                {
                    match = matches[MatchNum];
                    if (match.Groups.Count >= (groupNum + 1))
                    {
                        matchStr = match.Groups[groupNum].Value;
                    }
                }
                return matchStr;
            }

        }
    }
}
