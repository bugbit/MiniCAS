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
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MiniCAS.Core.Syntax
{
    public sealed class Tokenizer
    {
        private static readonly (Regex regex, ETokenType type)[] regTokens = new[]
            {
                (new Regex(@"^[ \f\t\v]+",RegexOptions.Compiled),ETokenType.Spaces),
                (new Regex(@"^[\r\n]+",RegexOptions.Compiled),ETokenType.NewLine),
                (new Regex(@"(?<token>^(\d*\.)?\d+)",RegexOptions.Compiled),ETokenType.Number)
            };

        private string text;

        public int Position { get; private set; } = 0;
        public int Line { get; private set; } = 1;
        public int Column { get; private set; } = 1;
        public bool EOF { get; private set; }
        public ETokenType Token { get; private set; }
        public string TokenStr { get; private set; }

        public Tokenizer(string _text)
        {
            text = _text;
        }

        public async Task<bool> NextToken(CancellationToken token)
        {
            if (EOF)
                return false;

            do
            {
                token.ThrowIfCancellationRequested();
                if (text.Length == 0)
                {
                    EOF = true;

                    return false;
                }

                await ReadTokenAsync(token);

            } while (Token == ETokenType.Spaces || Token == ETokenType.NewLine);

            return true;
        }

        public Token ToToken() => new(Position, Line, Column, Token, TokenStr);

        private Task ReadTokenAsync(CancellationToken token) => Task.Run(() => ReadToken(token), token);

        private void ReadToken(CancellationToken token)
        {
            var q =
            (
                from r in regTokens.AsParallel().WithCancellation(token)
                let m = r.regex.Match(text)
                where m.Success
                select new { r.type, m }
            ).FirstOrDefault();

            if (q == null)
                throw new STException(Position, Line, Column, string.Format(Properties.Resources.NoRecognizeStError, text[0]));

            var match = q.m;
            var len = match.Length;
            var gtoken = match.Groups["token"];

            Position += len;
            Token = q.type;
            TokenStr = (gtoken.Success) ? gtoken.Value : "";

            switch (Token)
            {
                case ETokenType.NewLine:
                    Column = 1;
                    Line++;
                    break;
                default:
                    Column += len;
                    break;
            }

            text = text.Substring(len);
        }
    }
}
