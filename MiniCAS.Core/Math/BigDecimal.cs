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
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace System.Numerics
{

    public struct BigDecimal : IConvertible, IFormattable, IComparable, IComparable<BigDecimal>, IEquatable<BigDecimal>
    {
        private static Regex regParseNumber = new(@"^(?<v1>[+-]{0,1})((?<v2>\d*)(?<sep>\.))?(?<v3>\d+)$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        public static readonly BigDecimal MinusOne = new BigDecimal(BigInteger.MinusOne, 0);
        public static readonly BigDecimal Zero = new BigDecimal(BigInteger.Zero, 0);
        public static readonly BigDecimal One = new BigDecimal(BigInteger.One, 0);

        private readonly BigInteger _unscaledValue;
        private readonly int _scale;

        public BigDecimal(double value)
            : this((decimal)value) { }

        public BigDecimal(float value)
            : this((decimal)value) { }

        public BigDecimal(decimal value)
        {
            var bytes = FromDecimal(value);

            var unscaledValueBytes = new byte[12];
            Array.Copy(bytes, unscaledValueBytes, unscaledValueBytes.Length);

            var unscaledValue = new BigInteger(unscaledValueBytes);
            var scale = bytes[14];

            if (bytes[15] == 128)
                unscaledValue *= BigInteger.MinusOne;

            _unscaledValue = unscaledValue;
            _scale = scale;
        }

        public BigDecimal(int value)
            : this(new BigInteger(value), 0) { }

        public BigDecimal(long value)
            : this(new BigInteger(value), 0) { }

        public BigDecimal(uint value)
            : this(new BigInteger(value), 0) { }

        public BigDecimal(ulong value)
            : this(new BigInteger(value), 0) { }

        public BigDecimal(BigInteger unscaledValue, int scale)
        {
            _unscaledValue = unscaledValue;
            _scale = scale;
        }

        public BigDecimal(byte[] value)
        {
            byte[] number = new byte[value.Length - 4];
            byte[] flags = new byte[4];

            Array.Copy(value, 0, number, 0, number.Length);
            Array.Copy(value, value.Length - 4, flags, 0, 4);

            _unscaledValue = new BigInteger(number);
            _scale = BitConverter.ToInt32(flags, 0);
        }

        public bool IsEven { get { return _unscaledValue.IsEven; } }
        public bool IsOne { get { return _unscaledValue.IsOne; } }
        public bool IsPowerOfTwo { get { return _unscaledValue.IsPowerOfTwo; } }
        public bool IsZero { get { return _unscaledValue.IsZero; } }
        public int Sign { get { return _unscaledValue.Sign; } }

        /// <summary>
		/// Gets the scale value of this <see cref="BigDecimal"/> instance
		/// </summary>
		/// <remarks>
		/// The scale is the number of digits behind the decimal point. The value of 
		/// this <see cref="BigDecimal"/> is the <c>unsignedValue * 10^(-scale)</c>. 
		/// If the scale is negative, then this <see cref="BigDecimal"/> represents a big integer.
		/// </remarks>
		public int Scale => _scale;

        public override string ToString() => ToString(null, null);

        public byte[] ToByteArray()
        {
            var unscaledValue = _unscaledValue.ToByteArray();
            var scale = BitConverter.GetBytes(_scale);

            var bytes = new byte[unscaledValue.Length + scale.Length];
            Array.Copy(unscaledValue, 0, bytes, 0, unscaledValue.Length);
            Array.Copy(scale, 0, bytes, unscaledValue.Length, scale.Length);

            return bytes;
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

        public static bool TryParse(string str, out BigDecimal? value) => TryParse(str, out value, out Exception ex);

        public static BigDecimal Parse(string str)
        {
            if (TryParse(str, out BigDecimal? value, out Exception ex))
                return value.Value;

            throw ex;
        }

        private static bool TryParse(string str, out BigDecimal? value, out Exception ex)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                ex = new FormatException("Cannot parse an empty string.");
                value = null;

                return false;
            }

            var match = regParseNumber.Match(str);

            if (!match.Success)
            {
                ex = new FormatException();
                value = null;

                return false;
            }

            var unscaledValue = string.Empty;
            var scale = 0;
            var grv = match.Groups["v1"];

            if (grv.Success)
                unscaledValue += grv.Value;

            grv = match.Groups["v2"];

            if (grv.Success)
                unscaledValue += grv.Value;

            grv = match.Groups["v3"];

            if (grv.Success)
                unscaledValue += grv.Value;

            if (grv.Success)
            {
                var grsep = match.Groups["sep"];

                if (grsep.Success)
                    scale = grv.Length;
            }

            try
            {
                value = new BigDecimal(BigInteger.Parse(unscaledValue), scale);
                ex = null;

                return true;
            }
            catch (Exception ex2)
            {
                ex = ex2;
                value = null;

                return false;
            }
        }

        public BigDecimal Round(int numdec)
        {
            if (numdec < 0)
                throw new ArgumentException("numdec negative");

            if (_scale == numdec)
                return this;

            var unscaledValue = _unscaledValue;

            if (_scale < numdec)  // n*(numdec-Scale)^10
                unscaledValue *= BigInteger.Pow(10, numdec - _scale);
            else
            {
                /*
                 * Round(3.1415927,4)
                 * 31415927 / 10^(7-4)=31415
                 * 31415927 mod 10^(7-4)=927
                 * 500>=927 => 31415+1=31416
                 * 3.1416
                 */
                var n10 = BigInteger.Pow(10, _scale - numdec);
                var n10_2 = n10 / 2;

                unscaledValue = BigInteger.DivRem(unscaledValue, n10, out BigInteger rem);
                if (rem >= n10_2)
                    unscaledValue++;
            }

            return new BigDecimal(unscaledValue, numdec);
        }

        #region Operators

        public static bool operator ==(BigDecimal left, BigDecimal right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BigDecimal left, BigDecimal right)
        {
            return !left.Equals(right);
        }

        public static bool operator >(BigDecimal left, BigDecimal right)
        {
            return (left.CompareTo(right) > 0);
        }

        public static bool operator >=(BigDecimal left, BigDecimal right)
        {
            return (left.CompareTo(right) >= 0);
        }

        public static bool operator <(BigDecimal left, BigDecimal right)
        {
            return (left.CompareTo(right) < 0);
        }

        public static bool operator <=(BigDecimal left, BigDecimal right)
        {
            return (left.CompareTo(right) <= 0);
        }

        public static bool operator ==(BigDecimal left, decimal right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BigDecimal left, decimal right)
        {
            return !left.Equals(right);
        }

        public static bool operator >(BigDecimal left, decimal right)
        {
            return (left.CompareTo(right) > 0);
        }

        public static bool operator >=(BigDecimal left, decimal right)
        {
            return (left.CompareTo(right) >= 0);
        }

        public static bool operator <(BigDecimal left, decimal right)
        {
            return (left.CompareTo(right) < 0);
        }

        public static bool operator <=(BigDecimal left, decimal right)
        {
            return (left.CompareTo(right) <= 0);
        }

        public static bool operator ==(decimal left, BigDecimal right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(decimal left, BigDecimal right)
        {
            return !left.Equals(right);
        }

        public static bool operator >(decimal left, BigDecimal right)
        {
            return (left.CompareTo(right) > 0);
        }

        public static bool operator >=(decimal left, BigDecimal right)
        {
            return (left.CompareTo(right) >= 0);
        }

        public static bool operator <(decimal left, BigDecimal right)
        {
            return (left.CompareTo(right) < 0);
        }

        public static bool operator <=(decimal left, BigDecimal right)
        {
            return (left.CompareTo(right) <= 0);
        }

        #endregion

        #region Explicity and Implicit Casts

        public static explicit operator byte(BigDecimal value) { return value.ToType<byte>(); }
        public static explicit operator sbyte(BigDecimal value) { return value.ToType<sbyte>(); }
        public static explicit operator short(BigDecimal value) { return value.ToType<short>(); }
        public static explicit operator int(BigDecimal value) { return value.ToType<int>(); }
        public static explicit operator long(BigDecimal value) { return value.ToType<long>(); }
        public static explicit operator ushort(BigDecimal value) { return value.ToType<ushort>(); }
        public static explicit operator uint(BigDecimal value) { return value.ToType<uint>(); }
        public static explicit operator ulong(BigDecimal value) { return value.ToType<ulong>(); }
        public static explicit operator float(BigDecimal value) { return value.ToType<float>(); }
        public static explicit operator double(BigDecimal value) { return value.ToType<double>(); }
        public static explicit operator decimal(BigDecimal value) { return value.ToType<decimal>(); }
        public static explicit operator BigInteger(BigDecimal value)
        {
            var scaleDivisor = BigInteger.Pow(new BigInteger(10), value._scale);
            var scaledValue = BigInteger.Divide(value._unscaledValue, scaleDivisor);
            return scaledValue;
        }

        public static implicit operator BigDecimal(byte value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(sbyte value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(short value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(int value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(long value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(ushort value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(uint value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(ulong value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(float value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(double value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(decimal value) { return new BigDecimal(value); }
        public static implicit operator BigDecimal(BigInteger value) { return new BigDecimal(value, 0); }

        #endregion

        public T ToType<T>() where T : struct
        {
            return (T)((IConvertible)this).ToType(typeof(T), null);
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            var scaleDivisor = BigInteger.Pow(new BigInteger(10), this._scale);
            var remainder = BigInteger.Remainder(this._unscaledValue, scaleDivisor);
            var scaledValue = BigInteger.Divide(this._unscaledValue, scaleDivisor);

            if (scaledValue > new BigInteger(Decimal.MaxValue))
                throw new ArgumentOutOfRangeException("value", "The value " + this._unscaledValue + " cannot fit into " + conversionType.Name + ".");

            var leftOfDecimal = (decimal)scaledValue;
            var rightOfDecimal = ((decimal)remainder) / ((decimal)scaleDivisor);

            var value = leftOfDecimal + rightOfDecimal;
            return Convert.ChangeType(value, conversionType);
        }

        public override bool Equals(object obj)
        {
            return ((obj is BigDecimal) && Equals((BigDecimal)obj));
        }

        public override int GetHashCode()
        {
            return _unscaledValue.GetHashCode() ^ _scale.GetHashCode();
        }

        #region IConvertible Members

        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Object;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(this);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(this);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException("Cannot cast BigDecimal to Char");
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException("Cannot cast BigDecimal to DateTime");
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(this);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(this);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(this);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(this);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(this);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(this);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(this);
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            // return Convert.ToString(this);
            return ToString();
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(this);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(this);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(this);
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (!string.IsNullOrWhiteSpace(format) || formatProvider != null)
                throw new NotImplementedException();

            //var numberformatInfo = formatProvider?.GetFormat(typeof(NumberFormatInfo)) as NumberFormatInfo ?? NumberFormatInfo.CurrentInfo;
            var number = _unscaledValue.ToString("G");

            if (_scale > 0)
                return number.Insert(number.Length - _scale, /*numberformatInfo.NumberDecimalSeparator*/".");

            return number;
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (!(obj is BigDecimal))
                throw new ArgumentException("Compare to object must be a BigDecimal", "obj");

            return CompareTo((BigDecimal)obj);
        }

        #endregion

        #region IComparable<BigDecimal> Members

        public int CompareTo(BigDecimal other)
        {
            var unscaledValueCompare = this._unscaledValue.CompareTo(other._unscaledValue);
            var scaleCompare = this._scale.CompareTo(other._scale);

            // if both are the same value, return the value
            if (unscaledValueCompare == scaleCompare)
                return unscaledValueCompare;

            // if the scales are both the same return unscaled value
            if (scaleCompare == 0)
                return unscaledValueCompare;

            var scaledValue = BigInteger.Divide(this._unscaledValue, BigInteger.Pow(new BigInteger(10), this._scale));
            var otherScaledValue = BigInteger.Divide(other._unscaledValue, BigInteger.Pow(new BigInteger(10), other._scale));

            return scaledValue.CompareTo(otherScaledValue);
        }

        #endregion

        #region IEquatable<BigDecimal> Members

        public bool Equals(BigDecimal other)
        {
            return this._scale == other._scale && this._unscaledValue == other._unscaledValue;
        }

        #endregion
    }
}
