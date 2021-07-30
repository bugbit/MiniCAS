#region LICENSE
/*
MIT License

Copyright (c) 2018 bugbit

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MiniCAS.Core.Expr
{
    public class Functions
    {
        private static readonly Lazy<Functions> instance = new Lazy<Functions>(() => new());
        private readonly Dictionary<string, Function> funciones = new Function[]
        {
            new("ifactors","ifactorsDef", Expr.IFactors, 1, 1)
        }.ToDictionary(f => f.name, f => f, StringComparer.InvariantCultureIgnoreCase);
        private readonly Lazy<Regex> regToken;

        public static Functions Instance => instance.Value;

        private Functions()
        {
            regToken = new Lazy<Regex>(MakeRegToken);
        }

        public Regex RegToken => regToken.Value;

        public Function GetFunction(string fnname)
        {
            funciones.TryGetValue(fnname, out Function f);

            return f;
        }

        private Regex MakeRegToken()
        {
            var fnnames = string.Join("|", funciones.Keys);
            var regex = new Regex($@"(?<token>^{fnnames})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return regex;
        }
    }
}
