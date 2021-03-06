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
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace MiniCAS.Core.Expr
{
    [DebuggerDisplay("TypeExpr : {TypeExpr} AlgExprType : {AlgExprType} IsZ : {IsZ} IsR : {IsR} IsDecimal : {IsDecimal} {DebugView}")]
    public class NumberExpr : AlgExpr
    {
        public bool IsZ { get; protected set; }
        public bool IsR { get; protected set; }
        public bool IsDecimal { get; protected set; }
        public virtual BigInteger ValueAsInteger => 0;
        public virtual BigDecimal ValueAsBDecimal => 0;
        public virtual decimal ValueAsDecimal => 0;

        protected NumberExpr() : base(EAlgExprType.Number) { }

        public override bool IsNumberExpr(out NumberExpr expr)
        {
            expr = this;

            return true;
        }

        public override bool ExprStartWithNumber() => true;

        public override bool ExprEndWithNumber() => true;
    }

    [DebuggerDisplay("TypeExpr : {TypeExpr} AlgExprType : {AlgExprType} IsZ : {IsZ} IsR : {IsR} IsDecimal : {IsDecimal} Value : {Value} {DebugView}")]
    public partial class NumberExpr<T> : NumberExpr
    {
        protected T Value { get; }

        protected NumberExpr(T v) { Value = v; }
    }
}
