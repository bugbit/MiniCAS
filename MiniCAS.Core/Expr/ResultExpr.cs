using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCAS.Core.Expr
{
    [DebuggerDisplay("TypeExpr : {TypeExpr} AlgExprType : {AlgExprType} {DebugView}")]
    public class ResultExpr : Expr
    {
        public ResultExpr(Expr r, ArrayList details = null) : base(EExprType.Result)
        {
            Result = r;
            Details = details;
        }

        public Expr Result { get; }
        public ArrayList Details { get; }
    }
}
