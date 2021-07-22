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
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Numerics;
using MiniCAS.Core.Expr;

using static System.Console;
using static MiniCAS.Core.Math.MathEx;
using System.Diagnostics;

namespace MiniCAS.Tests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await RunTest();
            WriteLine("Tests done. press ENTER to exit");
            ReadLine();
        }

        static async Task RunTest()
        {
            var pMethods = typeof(Program).Assembly.GetTypes().Select(t => t.FindMembers(MemberTypes.Method, BindingFlags.Static | BindingFlags.NonPublic, (m, c) => ((MethodInfo)m).GetCustomAttributes((Type)c, true).Length != 0, typeof(TestAttribute)).OfType<MethodInfo>()).SelectMany(m => m);

            foreach (var pMethod in pMethods)
            {
                if (pMethod.ReturnType == typeof(Task))
                    await ((TestHandler)pMethod.CreateDelegate(typeof(TestHandler))).Invoke();
                else
                    pMethod.Invoke(null, null);
                break;
            }
        }

        //[Test]
        static async Task TokernizerTest()
        {
            var texts = new[]
            {
                "   ","   \r\n","   \r\n20","    20","20","2","2.30","-30","aa"
            };

            foreach (var t in texts)
            {
                var token = new Core.Syntax.Tokenizer(t);

                Write($"{t}: ");

                try
                {
                    do
                    {
                        if (await token.NextToken(CancellationToken.None))
                            Write($"{token.Token} {token.TokenStr} ");
                    } while (!token.EOF);
                    WriteLine(".");
                }
                catch (Core.Syntax.STException ex)
                {
                    PrintError($"{ex.Message} en line {ex.Line} col {ex.Column} pos {ex.Position}");
                    PrintError(new string(' ', ex.Column - 1) + '^');
                    PrintError(t.Split('\n')[ex.Line - 1]);
                }
                catch (Exception ex)
                {
                    PrintError(ex.Message);
                }
            }
        }

        //[Test]
        static void NumberExprTest()
        {
            var nums = new NumberExpr[]
            {
                Expr.MakeNumber((BigInteger)1),Expr.MakeNumber(3.1415927),Expr.MakeNumber(1.234567890123456789m)
            };

            foreach (var n in nums)
            {
                WriteLine($"{n} = (Integer) {NumberToString(n.ValueAsInteger)}");
                WriteLine($"{n} = (BigDecimal) {NumberToString(n.ValueAsBDecimal)}");
                WriteLine($"{n} = (decimal) {NumberToString(n.ValueAsDecimal)}");
            }
        }

        //[Test]
        static void BigDecimalParseTest()
        {
            var texts = new[]
            {
                "20","2.30","-30","+30.23",".986","20,33"
            };

            foreach (var t in texts)
            {
                Write($"{t}: ");

                try
                {
                    var n = BigDecimal.Parse(t);

                    WriteLine(NumberToString(n));
                }
                catch (Exception ex)
                {
                    PrintError(ex.Message);
                }
            }
        }

        //[Test]
        static async Task ParserTest()
        {
            var texts = new[]
            {
                //"20","2.30","-30","aa","2x"
                "ifactors","ifactors 3"
            };

            foreach (var t in texts)
            {
                Write($"{t}: ");

                try
                {

                    var expr = await Core.Syntax.Parser.Parse(t, CancellationToken.None);

                    WriteLine(expr);
                }
                catch (Core.Syntax.STException ex)
                {
                    PrintError($"{ex.Message} en line {ex.Line} col {ex.Column} pos {ex.Position}");
                    PrintError(new string(' ', ex.Column - 1) + '^');
                    PrintError(t.Split('\n')[ex.Line - 1]);
                }
                catch (Exception ex)
                {
                    PrintError(ex.Message);
                }
            }
        }

        //[Test]
        static void BigDecimalRoundTest()
        {
            var numbers = new (BigDecimal n, int numdec)[]
            {
                (3.1415927,5),
                (3.1415927,10),
                (3.1415927,4),
            };

            foreach (var n in numbers)
            {
                Write($"{n}: ");

                try
                {
                    var r = n.n.Round(n.numdec);

                    WriteLine(NumberToString(r));
                }
                catch (Exception ex)
                {
                    PrintError(ex.Message);
                }
            }
        }

        [Test]
        static void BigIntegerIFactorsTest()
        {
            var numbers = new BigInteger[]
            {
                120,898,999999999999999999,9223372036854775809
            };

            foreach (var n in numbers)
            {
                var len = n.ToString().Length;

                Write($"{NumberToString(n)}: ");

                try
                {
                    var stopwatch = Stopwatch.StartNew();
                    var factors = Ifactors(n, CancellationToken.None);

                    stopwatch.Stop();

                    WriteLine(stopwatch.Elapsed);

                    foreach (var f in factors)
                    {
                        Write(f.n.ToString().PadLeft(len));
                        Write(" | ");
                        WriteLine(f.i);
                    }
                }
                catch (Exception ex)
                {
                    PrintError(ex.Message);
                }
            }
        }

        public static void PrintError(string argError)
        {
            var pForeColor = ForegroundColor;

            try
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine(argError);
            }
            finally
            {
                ForegroundColor = pForeColor;
            }
        }
    }
}
