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
using Deveel.Math;

namespace MiniCAS.Core.Math
{
    public static class MathEx
    {
        public static bool IsInteger(BigDecimal n) => n.Scale == 0;
        public static bool IsInteger(decimal n) => n % 1 == 0;
        public static BigDecimal ToBigDecimal(decimal n)
        {
            var bytes = FromDecimal(n);

            var unscaledValueBytes = new byte[12];
            Array.Copy(bytes, unscaledValueBytes, unscaledValueBytes.Length);

            var unscaledValue = new BigInteger(unscaledValueBytes);
            var scale = bytes[14];

            if (bytes[15] == 128)
                unscaledValue *= BigInteger.MinusOne;

            return new BigDecimal(unscaledValue, scale);
        }

        private static byte[] FromDecimal(decimal d)
        {
            byte[] bytes = new byte[16];

            int[] bits = decimal.GetBits(d);
            int lo = bits[0];
            int mid = bits[1];
            int hi = bits[2];
            int flags = bits[3];

            bytes[0] = (byte)lo;
            bytes[1] = (byte)(lo >> 8);
            bytes[2] = (byte)(lo >> 0x10);
            bytes[3] = (byte)(lo >> 0x18);
            bytes[4] = (byte)mid;
            bytes[5] = (byte)(mid >> 8);
            bytes[6] = (byte)(mid >> 0x10);
            bytes[7] = (byte)(mid >> 0x18);
            bytes[8] = (byte)hi;
            bytes[9] = (byte)(hi >> 8);
            bytes[10] = (byte)(hi >> 0x10);
            bytes[11] = (byte)(hi >> 0x18);
            bytes[12] = (byte)flags;
            bytes[13] = (byte)(flags >> 8);
            bytes[14] = (byte)(flags >> 0x10);
            bytes[15] = (byte)(flags >> 0x18);

            return bytes;
        }
    }
}
