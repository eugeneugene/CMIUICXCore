using System;
using System.Collections.Generic;
using System.Linq;

namespace CMIUICXCore.Code
{
    /// <summary>
    /// Defines the <see cref="IcxHelper" />.
    /// </summary>
    public static class IcxHelper
    {
        /// <summary>
        /// The ToICXArray.
        /// </summary>
        /// <param name="Str">The Str<see cref="string"/>.</param>
        /// <returns>The <see cref="byte"/> array.</returns>
        private static byte[] ToICXArray(this string Str)
        {
            var ret = new List<byte>();
            ret.AddRange(Str.Select(c => char.ToUpperInvariant(c) switch
            {
                '0' => Convert.ToByte('0'),
                '1' => Convert.ToByte('1'),
                '2' => Convert.ToByte('2'),
                '3' => Convert.ToByte('3'),
                '4' => Convert.ToByte('4'),
                '5' => Convert.ToByte('5'),
                '6' => Convert.ToByte('6'),
                '7' => Convert.ToByte('7'),
                '8' => Convert.ToByte('8'),
                '9' => Convert.ToByte('9'),
                'A' => Convert.ToByte('A'),
                'B' => Convert.ToByte('B'),
                'C' => Convert.ToByte('C'),
                'D' => Convert.ToByte('D'),
                'E' => Convert.ToByte('E'),
                'F' => Convert.ToByte('F'),
                _ => throw new ArgumentException("Неверный символ, допустимый набор: (0-9a-f)")
            }));

            return ret.ToArray();
        }

        /// <summary>
        /// The MakeConnectSeq.
        /// </summary>
        /// <param name="Para1">The Para1<see cref="string"/>.</param>
        /// <param name="Para2">The Para2<see cref="string"/>.</param>
        /// <returns>The <see cref="byte"/>array.</returns>
        public static byte[] MakeConnectSeq(string Para1, string Para2)
        {
            if (Para1 == null)
                throw new ArgumentNullException(nameof(Para1));
            if (Para2 == null)
                throw new ArgumentNullException(nameof(Para2));

            var b1 = Para1.ToICXArray();
            var b2 = Para2.ToICXArray();

            if (b1.Length <= 4)
            {
                if (b2.Length <= 4)
                {
                    List<byte> ret = new() { };
                    ret.AddRange(new byte[]
                    {
                        0xF2, 0,
                        Convert.ToByte('0'), Convert.ToByte('0'),
                        Convert.ToByte('4'), Convert.ToByte('0'),
                        Convert.ToByte('0'), Convert.ToByte('0')
                    });
                    ret.AddRange(Trim4(b1));
                    ret.AddRange(Trim4(b2));
                    ret.AddRange(new byte[]
                    {
                        Convert.ToByte('8'), Convert.ToByte('0'), 0xFF
                    });
                    return ret.ToArray();
                }
                else
                {
                    List<byte> ret = new() { };
                    ret.AddRange(new byte[]
                    {
                        0xF2, 0,
                        Convert.ToByte('0'), Convert.ToByte('0'),
                        Convert.ToByte('6'), Convert.ToByte('0'),
                        Convert.ToByte('0'), Convert.ToByte('0'),
                        Convert.ToByte('8'), Convert.ToByte('0')
                    });
                    ret.AddRange(Trim4(b1));
                    ret.AddRange(TrimX(b2));
                    ret.Add(0xFF);
                    return ret.ToArray();
                }
            }
            if (b1.Length <= 8)
            {
                List<byte> ret = new() { };
                ret.AddRange(new byte[]
                {
                    0xF2, 0,
                    Convert.ToByte('0'), Convert.ToByte('0'),
                    Convert.ToByte('8'), Convert.ToByte('0'),
                    Convert.ToByte('0'), Convert.ToByte('0'),
                    Convert.ToByte('8'), Convert.ToByte('0')
                });
                ret.AddRange(Trim8(b1));
                ret.AddRange(TrimX(b2));
                ret.Add(0xFF);
                return ret.ToArray();
            }
            throw new ArgumentException("Длина параметра не может превышать 8 байт", nameof(Para1));
        }

        /// <summary>
        /// The MakeDisconnectSeq.
        /// </summary>
        /// <param name="Para1">The Para1<see cref="string"/>.</param>
        /// <returns>The <see cref="byte"/>array.</returns>
        public static byte[] MakeDisconnectSeq(string Para1)
        {
            if (Para1 == null)
                throw new ArgumentNullException(nameof(Para1));

            var b1 = Para1.ToICXArray();

            if (b1.Length <= 4)
            {
                List<byte> ret = new() { };
                ret.AddRange(new byte[]
                {
                        0xF2, 0,
                        Convert.ToByte('0'), Convert.ToByte('0'),
                        Convert.ToByte('4'), Convert.ToByte('0'),
                        Convert.ToByte('0'), Convert.ToByte('0')
                });
                ret.AddRange(Trim4(b1));
                ret.AddRange(new byte[]
                {
                        Convert.ToByte('F'), Convert.ToByte('F'), Convert.ToByte('F'), Convert.ToByte('F'),
                        Convert.ToByte('8'), Convert.ToByte('0'), 0xFF
                });
                return ret.ToArray();
            }
            if (b1.Length <= 8)
            {
                List<byte> ret = new() { };
                ret.AddRange(new byte[]
                {
                    0xF2, 0,
                    Convert.ToByte('0'), Convert.ToByte('0'),
                    Convert.ToByte('8'), Convert.ToByte('0'),
                    Convert.ToByte('0'), Convert.ToByte('0'),
                    Convert.ToByte('8'), Convert.ToByte('0')
                });
                ret.AddRange(Trim8(b1));
                ret.AddRange(new byte[]
                {
                        Convert.ToByte('F'), Convert.ToByte('F'), Convert.ToByte('F'), Convert.ToByte('F'),
                        Convert.ToByte('F'), Convert.ToByte('F'), Convert.ToByte('F'), Convert.ToByte('F'),
                        Convert.ToByte('8'), Convert.ToByte('0'), 0xFF
                });
                return ret.ToArray();
            }
            throw new ArgumentException("Длина параметра не может превышать 8 байт", nameof(Para1));
        }

        /// <summary>
        /// The Trim4.
        /// </summary>
        /// <param name="ar">The ar<see cref="byte"/>array.</param>
        /// <returns>The <see cref="byte"/>array.</returns>
        private static byte[] Trim4(byte[] ar)
        {
            if (ar.Length < 4)
            {
                var ret = new List<byte> { };
                int l = 4 - ar.Length;
                for (int i = 0; i < l; i++)
                    ret.Add(Convert.ToByte('F'));
                ret.AddRange(ar);
                return ret.ToArray();
            }
            if (ar.Length == 4)
                return ar;
            return ar.Skip(ar.Length - 4).Take(4).ToArray();
        }

        /// <summary>
        /// The Trim8.
        /// </summary>
        /// <param name="ar">The ar<see cref="byte"/>array.</param>
        /// <returns>The <see cref="byte"/>array.</returns>
        private static byte[] Trim8(byte[] ar)
        {
            if (ar.Length < 8)
            {
                var ret = new List<byte> { };
                int l = 8 - ar.Length;
                for (int i = 0; i < l; i++)
                    ret.Add(Convert.ToByte('F'));
                ret.AddRange(ar);
                return ret.ToArray();
            }
            if (ar.Length == 8)
                return ar;
            return ar.Skip(ar.Length - 8).Take(8).ToArray();
        }

        /// <summary>
        /// The TrimX.
        /// </summary>
        /// <param name="ar">The ar<see cref="byte"/>array.</param>
        /// <returns>The <see cref="byte"/>array.</returns>
        private static byte[] TrimX(byte[] ar)
        {
            int lr = ar.Length % 4;
            var ret = new List<byte> { };
            for (int i = 4; i > lr; i--)
                ret.Add(Convert.ToByte('F'));
            ret.AddRange(ar);
            return ret.ToArray();
        }

        /// <summary>
        /// The ParseExt4.
        /// </summary>
        /// <param name="Ext">The Ext<see cref="string"/>.</param>
        /// <returns>The <see cref="byte"/>array.</returns>
        public static byte[] ParseExt4(string Ext)
        {
            if (Ext == null)
                throw new ArgumentNullException(nameof(Ext));

            char[] ext;

            if (Ext.Length == 4)
                ext = Ext.ToCharArray();
            else if (Ext.Length > 4)
                ext = Ext.ToCharArray(Ext.Length - 4, 4);
            else // Ext.Length < 4
                ext = Ext.PadLeft(4, 'F').ToCharArray();

            var ret = new List<byte>();
            ret.AddRange(ext.Select(c => char.ToUpperInvariant(c) switch
            {
                '0' => Convert.ToByte('0'),
                '1' => Convert.ToByte('1'),
                '2' => Convert.ToByte('2'),
                '3' => Convert.ToByte('3'),
                '4' => Convert.ToByte('4'),
                '5' => Convert.ToByte('5'),
                '6' => Convert.ToByte('6'),
                '7' => Convert.ToByte('7'),
                '8' => Convert.ToByte('8'),
                '9' => Convert.ToByte('9'),
                'A' => Convert.ToByte('A'),
                'B' => Convert.ToByte('B'),
                'C' => Convert.ToByte('C'),
                'D' => Convert.ToByte('D'),
                'E' => Convert.ToByte('E'),
                'F' => Convert.ToByte('F'),
                _ => throw new ArgumentException("Неверный символ, допустимый набор: (0-9a-f)")
            }));

            return ret.ToArray();
        }

        /// <summary>
        /// The ParseExt8.
        /// </summary>
        /// <param name="Ext">The Ext<see cref="string"/>.</param>
        /// <returns>The <see cref="byte"/>array.</returns>
        public static byte[] ParseExt8(string Ext)
        {
            if (Ext == null)
                throw new ArgumentNullException(nameof(Ext));

            char[] ext;

            if (Ext.Length == 8)
                ext = Ext.ToCharArray();
            else if (Ext.Length > 8)
                ext = Ext.ToCharArray(Ext.Length - 8, 8);
            else // Ext.Length < 8
                ext = Ext.PadLeft(8, 'F').ToCharArray();

            var ret = new List<byte>();
            ret.AddRange(ext.Select(c => char.ToUpperInvariant(c) switch
            {
                '0' => Convert.ToByte('0'),
                '1' => Convert.ToByte('1'),
                '2' => Convert.ToByte('2'),
                '3' => Convert.ToByte('3'),
                '4' => Convert.ToByte('4'),
                '5' => Convert.ToByte('5'),
                '6' => Convert.ToByte('6'),
                '7' => Convert.ToByte('7'),
                '8' => Convert.ToByte('8'),
                '9' => Convert.ToByte('9'),
                'A' => Convert.ToByte('A'),
                'B' => Convert.ToByte('B'),
                'C' => Convert.ToByte('C'),
                'D' => Convert.ToByte('D'),
                'E' => Convert.ToByte('E'),
                'F' => Convert.ToByte('F'),
                _ => throw new ArgumentException("Неверный символ, допустимый набор: (0-9a-f)")
            }));

            return ret.ToArray();
        }
    }
}
