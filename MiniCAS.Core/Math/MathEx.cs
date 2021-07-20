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
using System.Threading.Tasks;
using System.Numerics;
using System.Globalization;

namespace MiniCAS.Core.Math
{
    public static class MathEx
    {
        public static string NumberToString(BigInteger n) => n.ToString(CultureInfo.InvariantCulture);
        public static string NumberToString(BigDecimal n) => n.ToString();
        public static string NumberToString(decimal n) => n.ToString(CultureInfo.InvariantCulture);

        public static bool IsInteger(BigDecimal n) => n.Scale <= 0;
        public static bool IsInteger(decimal n) => n % 1 == 0;

        public static BigDecimal Round(BigDecimal n, int numdec) => n.Round(numdec);
        public static decimal Round(decimal n, int numdec) => decimal.Round(n, numdec);
    }
}
