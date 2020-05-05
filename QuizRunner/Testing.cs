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
    }
}