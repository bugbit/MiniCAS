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
using MiniCAS.Core.Output;

using static MiniCAS.Core.Math.MathEx;

namespace MiniCAS.Core.Expr
{
    public partial class Expr : IToLatex
    {
        public override string ToString() => string.Empty;

        public virtual LaTex ToLatex() => new LaTex();
    }

    public partial class TokenExpr
    {
        public override string ToString() => Token.TokenStr;
        public override LaTex ToLatex() => base.ToLatex().Append(Token.TokenStr);
    }

    public partial class ResultExpr
    {
        public override string ToString() => Result.ToString();

        public override LaTex ToLatex() => Result.ToLatex();
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

    public partial class FunctionExpr
    {
        public override string ToString()
        {
            if (Params.Length == 1 && !NeedsParentheses(this, Params[0]))
                return $"{Function.name} {Params[0]}";

            var _params = string.Join<Expr>(',', Params);

            return $"{Function.name}({_params})";
        }

        public override LaTex ToLatex()
        {
            var latex = base.ToLatex();
            var _params = string.Join<Expr>(',', Params);

            latex.AppendOperationName(Function.name, _params);

            return latex;
        }
    }

    public partial class TermExpr
    {
        public override string ToString()
        {
            if (Exprs.Length <= 0)
                return string.Empty;

            var e1 = Exprs.First();
            var str = (Sign == -1) ? "-" : "";

            str += e1.ToString();

            foreach (var e in Exprs.Skip(1))
            {
                if (NeedsParentheses(e1, e))
                    str += $"({e})";
                else if (e1.ExprEndWithNumber() || e.ExprStartWithNumber())
                    str += $"*{e}";
                else
                    str += e;
                e1 = e;
            }

            return str;
        }

        public override LaTex ToLatex()
        {
            var latex = base.ToLatex();

            if (Exprs.Length <= 0)
                return latex;

            var e1 = Exprs.First();

            if (Sign == -1)
                latex.Append("-");

            latex.Append(e1);

            foreach (var e in Exprs.Skip(1))
            {
                if (NeedsParentheses(e1, e))
                    latex.Append("(").Append(e).Append(")");
                else if (e1.IsNumberExpr(out NumberExpr ne1) && ne1.IsZ && e.IsNumberExpr(out NumberExpr ne) && ne.IsZ)
                    latex.Append(@"\cdot").Append(e);
                else
                    latex.Append(e);
                e1 = e;
            }

            return latex;
        }
    }

    public partial class PowExpr
    {
        public override string ToString() => $"{Base}^{ToStringParenthesesIfNeeds(Base, Exp)}";

        public override LaTex ToLatex() => base.ToLatex().AppendBrackets(Base).Append("^").AppendBrackets(Exp);
    }

    public partial class SimplyExprs
    {
        public override string ToString() => string.Join(" = ", Exprs);

        public override LaTex ToLatex() => base.ToLatex().AppendEquation(Exprs);
    }
}
