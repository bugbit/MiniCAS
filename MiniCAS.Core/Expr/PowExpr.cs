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
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MiniCAS.Core.Expr
{
    [DebuggerDisplay("TypeExpr : {TypeExpr} AlgExprType : {AlgExprType} Base : {Base} Exp : {Exp} {DebugView}")]
    public partial class PowExpr : AlgExpr
    {
        public AlgExpr Base { get; set; }
        public AlgExpr Exp { get; set; }
        public PowExpr(AlgExpr _base, AlgExpr _exp) : base(EAlgExprType.Pow)
        {
            Base = _base;
            Exp = _exp;
        }

        public static AlgExpr SimplyPow(AlgExpr _base, AlgExpr _exp) => (_exp == One) ? _base : (_exp == Zero) ? One : null;
        public AlgExpr SimplyPow() => SimplyPow(Base, Exp);

        public override bool IsPowInteger(out BigInteger _base, out BigInteger _exp)
        {
            if (!IsNumberExpr(out NumberExpr nbase) || !nbase.IsZ || !IsNumberExpr(out NumberExpr nexp) || !nexp.IsZ)
            {
                _base = _exp = default(BigInteger);

                return false;
            }

            _base = nbase.ValueAsInteger;
            _exp = nexp.ValueAsInteger;

            return true;
        }

        public override bool ExprStartWithNumber() => Base.ExprStartWithNumber();

        public override bool ExprEndWithNumber() => Exp.ExprEndWithNumber();
    }
}
