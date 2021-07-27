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
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCAS.Core.Expr
{
    [DebuggerDisplay("TypeExpr : {TypeExpr} AlgExprType : {AlgExprType} Sign : {Sign} {DebugView}")]
    public partial class TermExpr : AlgExpr
    {
        // Devuelve:
        //     A number that indicates the sign of the System.Numerics.BigInteger object, as
        //     shown in the following table.
        //     Number – Description
        //     -1 – The value of this object is negative.
        //     0 – The value of this object is 0 (zero).
        //     1 – The value of this object is positive.
        public int Sign { get; }
        public ImmutableArray<AlgExpr> Exprs { get; }

        public TermExpr(int sign, IEnumerable<AlgExpr> exprs) : base(EAlgExprType.Term)
        {
            Sign = sign;
            Exprs = exprs.ToImmutableArray();
        }
    }
}
