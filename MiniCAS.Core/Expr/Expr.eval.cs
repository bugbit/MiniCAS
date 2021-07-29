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

using MiniCAS.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MiniCAS.Core.Output;

using static MiniCAS.Core.Math.MathEx;
using System.Numerics;
using System.Collections;

namespace MiniCAS.Core.Expr
{
    public partial class Expr
    {
        public virtual Task<Expr> Eval(CancellationToken cancel) => Task.FromResult(this);
        public virtual async Task<Expr> EvalAndApprox(int? numdec, CancellationToken cancel)
        {
            var r = await Eval(cancel);

            if (numdec.HasValue)
                r = await r.Approx(numdec.Value);

            return r;
        }

        public async static Task<Expr> IFactors(Expr[] _params, CancellationToken cancel)
        {
            Function.VerifNumParams(_params.Length, 1, 1);

            var e1 = _params[0];
            var n = e1.VerifIsNumberExpr();
            var ret = await Ifactors(n.ValueAsInteger, cancel);
            var exprs = new List<AlgExpr>();

            exprs.AddIfNotEmpyOrNotEqualsEnd(n);

            var e2 = MakeTerm(1, ret.Select(f => MakeNumber(f.i)).ToArray());

            exprs.AddIfNotEmpyOrNotEqualsEnd(e2);

            var e3 = await e2.SimpyEqualsExprsToPow(cancel);

            exprs.AddIfNotEmpyOrNotEqualsEnd(e3);

            var explain = new ArrayList(new object[]
            {
                //"Realizar divisiones entre sus divisores primos hasta que obtengamos un uno en el cociente."
                Properties.Resources.IFactorsDetail1,
                await IFactorsTable(ret, cancel)
            });

            if (exprs.Count > 1)
                explain.Add(MakeSimplyExprs(exprs));

            return MakeResult(exprs.Last(), explain);
        }

        public static async Task<LaTex> IFactorsTable((BigInteger n, BigInteger i)[] Ifactors, CancellationToken cancel)
        {
            return await Task.Run
            (
                () =>
                {
                    var latex = new LaTex();

                    latex.AppendBeginArray();
                    latex.Append("{r|r}");

                    foreach (var r in Ifactors)
                    {
                        cancel.ThrowIfCancellationRequested();
                        latex.AppendRowArray(r.n, r.i);
                    }
                    latex.AppendRowArray(1);
                    latex.AppendEndArray();

                    return latex;
                }, cancel
            );
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
