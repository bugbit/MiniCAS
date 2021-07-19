using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniCAS.Core.Expr;

namespace MiniCAS.Core.Output
{
    public class LaTex
    {
        private StringBuilder mStr;

        public LaTex()
        {
            mStr = new StringBuilder();
        }

        public override string ToString() => mStr.ToString();

        public LaTex Append(LaTex latex)
        {
            mStr.Append(latex.mStr);

            return this;
        }

        public LaTex Append(IToLatex latex) => Append(latex.ToLatex());

        public LaTex Append(object o)
        {
            if (o is IToLatex latex)
                return Append(latex);

            mStr.Append(Convert.ToString(o));

            return this;
        }

        public LaTex AppendJoin(string sep, params object[] objs) => AppendJoin(sep, objs.AsEnumerable());

        public LaTex AppendJoin(string sep, IEnumerable<object> objs)
        {
            var ini = true;

            foreach (var o in objs)
            {
                if (ini)
                    ini = false;
                else
                    Append(sep);
                Append(o);
            }

            return this;
        }

        //public LaTex AppendParenthesesIfNeeds(Expr parent, Expr e)
        //{
        //    var pNeeds = Expr.NeedsParentheses(parent, e);

        //    if (pNeeds)
        //        Append("(");
        //    Append(e);
        //    if (pNeeds)
        //        Append(")");

        //    return this;
        //}

        public LaTex AppendBeginBrackets() => Append("{");
        public LaTex AppendEndBrackets() => Append("}");

        public LaTex AppendBrackets(object a) => AppendBeginBrackets().Append(a).AppendEndBrackets();

        public LaTex AppendFrac(object a, object b) => Append("\frac").AppendBrackets(a).AppendBrackets(b);

        public LaTex AppendBeginArray() => Append(@"\begin{array} ");
        public LaTex AppendRowArray(params object[] objs)
        {
            var ini = true;

            foreach (var o in objs)
            {
                if (ini)
                    ini = false;
                else
                    Append(@"& ");
                Append(o);
            }

            return Append(@"\\");
        }
        public LaTex AppendEndArray() => Append(@"\end{array}");

        public LaTex AppendEquation(params object[] objs) => AppendEquation(objs.AsEnumerable());
        public LaTex AppendEquation(IEnumerable<object> objs) => AppendJoin("=", objs);
    }
}
