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
using System.Threading;
using System.Threading.Tasks;

namespace MiniCAS.Core.Expr
{
    public record Function(string name, string DefinitionRes, Func<Expr[], CancellationToken, Task<Expr>> Calc, int? NumParamMin = null, int? NumParamMax = null)
    {
        public static void VerifNumParams(int numP, int? min = null, int? max = null)
        {
            var pMsg = string.Empty;

            if (min.HasValue && max.HasValue && min == max)
                pMsg += string.Format(Properties.Resources.NumParamsException, max);
            else
            {
                if (min.HasValue && numP < min.Value)
                    pMsg += string.Format(Properties.Resources.MinNumParamsException, min);
                if (max.HasValue && numP > max.Value)
                    pMsg += string.Format(Properties.Resources.MaxNumParamsException, max);
            }

            if (!string.IsNullOrEmpty(pMsg))
                throw new ExprException(pMsg);
        }
        public void VerifNumParams(int numP) => VerifNumParams(numP, NumParamMin, NumParamMax);
    }
}
