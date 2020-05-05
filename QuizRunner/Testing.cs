using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace QuizRunner.Testing
{
    public class Testing
    {
        private int _closingBracket = 0;

        public string SimplifyArg(string arg)
        {
            return Regex.Replace(arg, @"\s+", "");
        }

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

    }
}