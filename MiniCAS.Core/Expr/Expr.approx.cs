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

using static MiniCAS.Core.Math.MathEx;

namespace MiniCAS.Core.Expr
{
    public partial class Expr
    {
        public virtual Task<Expr> Approx(int numdec) => Task.FromResult(this);
    }

    public partial class NumberRealExpr
    {
        public override Task<Expr> Approx(int numdec) => Task.FromResult((Expr)MakeNumber(Round(Value, numdec)));
    }

    public partial class NumberDecimalExpr
    {
        public override Task<Expr> Approx(int numdec) => Task.FromResult((Expr)MakeNumber(Round(Value, numdec)));
    }

    public partial class ResultExpr
    {
        public async override Task<Expr> Approx(int numdec)
        {
            var e = await Result.Approx(numdec);

            if (e == Result)
                return Result;

            return MakeResult(e, this);
        }
    }
}