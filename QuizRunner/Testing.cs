using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;
using System.IO;

namespace QuizRunner.Testing
{
    public class Testing
    {

        private int _closingBracket = 0; // Хранит индекс последней ']' в строке

        /// <summary>
        /// Удаляет все пробелы из строки.
        /// </summary>
        public string SimplifyArg(string arg) // arg - строка типа [] = [] + x. ( GetAnswerArgument(k, 0) )
        {
            return Regex.Replace(arg, @"\s+", "");
        }

        /// <summary>
        /// Находит математический знак операции.
        /// </summary>
        public char GetMathOperator(string arg)
        {
            foreach (var item in arg.Select((value, i) => new { i, value }))
            {
                if (item.value == ']')
                {
                    _closingBracket = item.i;
                }
            }

            return arg[_closingBracket + 1];
        }

        /// <summary>
        /// Находит значение, на которое увеличивается результат.
        /// </summary>
        public string GetIncreaseValue(string arg)
        {
            string increaseValue = "";
            for (var x = _closingBracket + 2; x < arg.Length; x++)
            {
                increaseValue += arg[x];
            }

            return increaseValue;
        }

        public string GetArgumentName(string arg)
        {
            var argName = "";

            foreach (var item in arg)
            {            
                if (item != '[' && item != ']' && item != '=')
                {
                    argName += item;
                }

                if (item != '=')
                {
                    break;
                }
            }

            return argName;
        }

        /// <summary>
        /// Счётчик результат в соответствии с математической операцией.
        /// </summary>
        public int Calculate(char mathOperator, string increaseValue, int result)
        {
            switch (mathOperator)
            {
                case '+':
                    return result + int.Parse(increaseValue);
                case '-':
                    return result - int.Parse(increaseValue);
                case '*':
                    if (result == 0)
                    {
                        return int.Parse(increaseValue);
                    }
                    else
                    {
                        return result * int.Parse(increaseValue);
                    }
                case '/':
                    return result / int.Parse(increaseValue);
            }

            return result + 1;
        }
        
        public object GetCompute(string arg, Dictionary<string, double> D)
        {
            string Comp = arg.Substring(arg.LastIndexOf('=') + 1).TrimStart(' ');;

            while (Comp.Contains("["))
            {
                string Name = Comp.Substring(Comp.IndexOf("[") + 1, Comp.IndexOf("]") - (Comp.IndexOf("[") + 1));
                Comp = Comp.Replace("[" + Name + "]", D[Name].ToString());
            }

            return new DataTable().Compute(Comp, "");
        }
    }
}