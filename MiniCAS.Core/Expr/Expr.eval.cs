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
using static MiniCAS.Core.Math.MathEx;

namespace MiniCAS.Core.Expr
{
    public partial class Expr
    {
        public virtual Task<Expr> Eval(CancellationToken token) => Task.FromResult(this);
        public virtual async Task<Expr> EvalAndApprox(int? numdec, CancellationToken token)
        {
            var r = await Eval(token);

            if (numdec.HasValue)
                r = await r.Approx(numdec.Value);

            return r;
        }

        public static Task<Expr> IFactors(Expr[] _params, CancellationToken token)
        {
            Function.VerifNumParams(_params.Length, 1, 1);

            if (!_params[0].IsNumberExpr(out NumberExpr n) || !n.IsZ)
                throw new ExprException();

            //await Ifactors(n.ValueAsInteger, token);

            return null;
        }
    }

    public partial class FunctionExpr
    {
        public async override Task<Expr> Eval(CancellationToken token)
        {
            var _params = await Task.WhenAll(from p in Params select p.Eval(token));
            var result = await Function.Calc.Invoke(_params, token);

            return result;
        }
    }
}
