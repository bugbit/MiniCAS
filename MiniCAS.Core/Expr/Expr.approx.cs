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
}
