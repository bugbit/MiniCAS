﻿#region LICENSE
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

using static MiniCAS.Core.Math.MathEx;

namespace MiniCAS.Core.Expr
{
    [DebuggerDisplay("TypeExpr : {TypeExpr} {DebugView}")]
    public partial class Expr
    {
        public EExprType TypeExpr { get; }

        protected Expr(EExprType _type)
        {
            TypeExpr = _type;
        }

        protected string DebugView => ToString();

        public static NumberIntegerExpr MakeNumber(BigInteger n) => new(n);
        public static NumberExpr MakeNumber(BigDecimal n) => (IsInteger(n)) ? new NumberIntegerExpr((BigInteger)n) : new NumberRealExpr(n);
        public static NumberExpr MakeNumber(decimal n) => (IsInteger(n)) ? new NumberIntegerExpr((BigInteger)n) : new NumberDecimalExpr(n);
    }
}
