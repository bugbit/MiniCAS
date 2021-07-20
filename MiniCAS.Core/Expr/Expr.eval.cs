using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCAS.Core.Expr
{
    public partial class Expr
    {
        public virtual Task<Expr> Eval() => Task.FromResult(this);
        public virtual async Task<Expr> EvalAndApprox(int? numdec)
        {
            var r = await Eval();

            if (numdec.HasValue)
                r = await r.Approx(numdec.Value);

            return r;
        }
    }
}
