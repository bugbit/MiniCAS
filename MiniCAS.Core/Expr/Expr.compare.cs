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

namespace MiniCAS.Core.Expr
{
    public partial class Expr : IEquatable<Expr>
    {
        public virtual bool Equals(Expr other) => TypeExpr == other.TypeExpr;
        public override bool Equals(object obj) => (obj is Expr e) && Equals(e);
        public override int GetHashCode() => TypeExpr.GetHashCode();

        public static bool operator ==(Expr e1, Expr e2)
        {
            var i1 = ReferenceEquals(e1, null);
            var i2 = ReferenceEquals(e2, null);

            return (i1 || i2) ? i1 == i2 : e1.Equals(e2);
        }
        public static bool operator !=(Expr e1, Expr e2)
        {
            var i1 = ReferenceEquals(e1, null);
            var i2 = ReferenceEquals(e2, null);

            return (i1 || i2) ? i1 != i2 : !e1.Equals(e2);
        }
    }

    public partial class AlgExpr
    {
        public override bool Equals(Expr other) => base.Equals(other) && (other is AlgExpr e) && Equals(e);

        private bool Equals(AlgExpr other) => AlgExprType == other.AlgExprType;
    }

    public partial class TokenExpr
    {
        public override bool Equals(Expr other) => base.Equals(other) && (other is TokenExpr e) && Equals(e);

        private bool Equals(TokenExpr other) => other.Token.Equals(other);
    }

    public partial class FunctionExpr
    {
        public override bool Equals(Expr other) => base.Equals(other) && (other is FunctionExpr e) && Equals(e);

        private bool Equals(FunctionExpr other) => Function.name.Equals(other.Function.name) && Params.Length == other.Params.Length && Params.SequenceEqual(other.Params);
    }

    public partial class ResultExpr
    {
        public override bool Equals(Expr other) => base.Equals(other) && (other is ResultExpr e) && Equals(e);

        private bool Equals(ResultExpr other) => Result.Equals(other);
    }

    public partial class NumberExpr<T>
    {
        public override bool Equals(Expr other) => base.Equals(other) && (other is NumberExpr e) && Equals(e);

        private bool Equals(NumberExpr other)
        {
            if (IsZ != other.IsZ)
                return false;

            if (IsZ)
                return ValueAsInteger == other.ValueAsInteger;

            if (IsDecimal && other.IsDecimal)
                return ValueAsDecimal == other.ValueAsDecimal;

            return ValueAsBDecimal == other.ValueAsBDecimal;
        }
    }

    public partial class TermExpr
    {
        public override bool Equals(Expr other) => base.Equals(other) && (other is TermExpr e) && Equals(e);

        private bool Equals(TermExpr other)
        {
            return Sign == other.Sign && Exprs.Equals(other.Exprs);
        }
    }
}
