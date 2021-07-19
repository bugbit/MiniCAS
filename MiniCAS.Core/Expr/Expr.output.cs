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
using MiniCAS.Core.Output;

using static MiniCAS.Core.Math.MathEx;

namespace MiniCAS.Core.Expr
{
    public partial class Expr : IToLatex
    {
        public override string ToString() => string.Empty;

        public virtual LaTex ToLatex() => new LaTex();
    }

    public partial class TokenExpr
    {
        public override string ToString() => Token.TokenStr;
        public override LaTex ToLatex() => base.ToLatex().Append(Token.TokenStr);
    }

    public partial class NumberExpr<T>
    {
        public override LaTex ToLatex() => base.ToLatex().Append(Value);
    }

    public partial class NumberIntegerExpr
    {
        // NumberDecimalSeparator always ".", not used NumberFormatInfo
        public override string ToString() => NumberToString(Value);
    }

    public partial class NumberRealExpr
    {
        // NumberDecimalSeparator always ".", not used NumberFormatInfo
        public override string ToString() => NumberToString(Value);
    }

    public partial class NumberDecimalExpr
    {
        // NumberDecimalSeparator always ".", not used NumberFormatInfo
        public override string ToString() => NumberToString(Value);
    }
}
