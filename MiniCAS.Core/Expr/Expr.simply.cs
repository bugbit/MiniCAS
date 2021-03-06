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
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiniCAS.Core.Expr
{
    public partial class Expr
    {
        public Expr Simply() => null;
        public Expr SimplyOrExpr(Func<Expr, Expr> simply, Expr e) => simply(e) ?? e;
        //public BigInteger SimplyToInteger()
        //{
        //    var expr = Simply();

        //    if (expr.AlgExprType != EAlgExprType.Number || !(expr is NumberExpr exprn) || !exprn.IsZ)
        //        throw new ExprException();

        //    return exprn.ValueAsInteger;
        //}
    }

    public partial class AlgExpr
    {
        public virtual Task<AlgExpr> SimpyEqualsExprsToPow(CancellationToken cancel) => Task.FromResult((AlgExpr)null);
    }

    public partial class TermExpr
    {
        public async override Task<AlgExpr> SimpyEqualsExprsToPow(CancellationToken cancel)
            => await Task.Run(() =>
            {
                var done = false;
                var exprs = Exprs.ToList();

                for (var i = 0; i < exprs.Count; i++)
                {
                    var e1 = exprs[i];
                    var exp = 1;

                    for (var j = i + 1; j < exprs.Count;)
                    {
                        var e2 = exprs[j];

                        cancel.ThrowIfCancellationRequested();
                        if (e1 == e2)
                        {
                            exp++;
                            exprs.RemoveAt(j);
                        }
                        else
                            j++;
                    }
                    if (exp > 1)
                    {
                        done = true;
                        exprs[i] = MakePow(e1, exp);
                    }
                }

                return !done ? null : MakeTerm(Sign, exprs);
            }, cancel);
    }
}
