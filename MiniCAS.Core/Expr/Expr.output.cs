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
