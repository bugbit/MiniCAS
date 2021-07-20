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
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MiniCAS.Core.Output;

namespace MiniCAS.Core.Syntax
{
    public class Parser
    {
        private Stack<(Token Operation, List<(Token Token, Expr.Expr Expr)> Exprs)> stack;
        private Tokenizer tokenizer;

        public static Task<Expr.Expr> Parse(string expr, CancellationToken token) => new Parser().Parse(expr, false, token);

        public static async Task<LaTex> ParseToLatex(string expr, CancellationToken token)
        {
            try
            {
                var e = await new Parser().Parse(expr, true, token);

                token.ThrowIfCancellationRequested();

                var latex = e.ToLatex();

                return latex;
            }
            catch
            {
                return new LaTex();
            }
        }

        private async Task<Expr.Expr> Parse(string expr, bool argNoThrowEx, CancellationToken token)
        {
            stack = new();
            tokenizer = new(expr);
            while (!tokenizer.EOF)
            {
                token.ThrowIfCancellationRequested();

                try
                {
                    await tokenizer.NextToken(token);
                    if (!ProcessToken(argNoThrowEx))
                        break;
                }
                catch
                {
                    if (argNoThrowEx)
                    {
                        CompleteAct();

                        break;
                    }

                    throw;
                }
            }

            return ProcessAll(token);
        }

        private bool ProcessToken(bool argNoThrowEx)
        {
            if (tokenizer.EOF)
                return false;

            var token = tokenizer.ToToken();
            Expr.Expr expr = null;

            if (token.TokenType == ETokenType.Number)
            {
                var n = BigDecimal.Parse(token.TokenStr);

                expr = Expr.Expr.MakeNumber(n);
            }
            else
            {
                if (argNoThrowEx)
                    expr = new Expr.TokenExpr(token);
                else
                    throw new STException(token.Position, token.Line, token.Column, string.Format(Properties.Resources.NoRecognizeStError, token.TokenStr));
            }

            if (stack.Count == 0)
                stack.Push(new(token, new(new[] { (token, expr) })));
            else
            {
                throw new NotImplementedException();
                //throw new STException(token.Position, token.Line, token.Column, string.Format(Properties.Resources.NoExpectTokenException, token.TokenStr));
            }

            return expr.TypeExpr != Expr.EExprType.Token;
        }

        private Expr.Expr PopAndProcess()
        {
            if (stack.Count == 0)
                return null;

            var expr = stack.Pop().Exprs[0].Expr;

            ProcessExprPeek(expr);

            return expr;
        }

        private void ProcessExprPeek(Expr.Expr expr)
        {
            if (stack.Count == 0)
                return;

            throw new NotImplementedException();
        }

        private void CompleteAct()
        {

        }

        private Expr.Expr ProcessAll(CancellationToken token)
        {
            Expr.Expr expr = null;

            while (stack.Count > 0)
            {
                token.ThrowIfCancellationRequested();
                expr = PopAndProcess();
            }

            return expr;
        }
    }
}
