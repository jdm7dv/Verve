// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.IO;
using MBF.Properties;

namespace MBF.Util
{
    /// <summary>
    /// Provides methods for encoding URLs when processing Web requests.
    /// </summary>
    public static class HttpUtility
    {
        #region Private Constants
        /// <summary>
        /// Holds hexa characters.
        /// </summary>
        private static char[] hexChars = "0123456789abcdef".ToCharArray();

        /// <summary>
        /// Holds nonencoded characters.
        /// </summary>
        private const string notEncodedChars = "!'()*-._";
        #endregion

        #region Public Methods
        /// <summary>
        /// Encodes a URL string.
        /// </summary>
        /// <param name="str">The text to encode.</param>
        /// <returns>An encoded string.</returns>
        public static string UrlEncode(string str)
        {
            return UrlEncode(str, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// Encodes a URL string using the specified encoding object.
        /// </summary>
        /// <param name="str">The text to encode.</param>
        /// <param name="Enc">The System.Text.Encoding object that specifies the encoding scheme.</param>
        /// <returns>An encoded string.</returns>
        public static string UrlEncode(string str, System.Text.Encoding Enc)
        {
            if (str == null)
                return null;

            if (str == "")
                return "";

            byte[] bytes = Enc.GetBytes(str);
            return System.Text.Encoding.ASCII.GetString(UrlEncodeToBytes(bytes, 0, bytes.Length));
        }
        #endregion

        #region Private Methods
        // Encodes specified string to a byte array.
        private static byte[] UrlEncodeToBytes(string str)
        {
            return UrlEncodeToBytes(str, System.Text.Encoding.UTF8);
        }

        // Encodes specified string using specified Encoding to a byte array.
        private static byte[] UrlEncodeToBytes(string str, System.Text.Encoding e)
        {
            if (str == null)
                return null;

            if (str == "")
                return new byte[0];

            byte[] bytes = e.GetBytes(str);
            return UrlEncodeToBytes(bytes, 0, bytes.Length);
        }

        // encodes the specified bytes array.
        private static byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count)
        {
            if (bytes == null)
                return null;

            int len = bytes.Length;
            if (len == 0)
                return new byte[0];

            if (offset < 0 || offset >= len)
                throw new ArgumentOutOfRangeException("offset");

            if (count < 0 || count > len - offset)
                throw new ArgumentOutOfRangeException("count");

            MemoryStream result = new MemoryStream(count);
            int end = offset + count;
            for (int i = offset; i < end; i++)
                UrlEncodeChar((char)bytes[i], result, false);

            return result.ToArray();
        }

        // Encodes specified char and stores the result in specified stream.
        private static void UrlEncodeChar(char ch, Stream result, bool isUnicode)
        {
            if (ch > 255)
            {
                if (!isUnicode)
                    throw new ArgumentOutOfRangeException("ch", ch, Resource.ParamCHmustbeLessThan256);
                int idx;
                int i = (int)ch;

                result.WriteByte((byte)'%');
                result.WriteByte((byte)'u');
                idx = i >> 12;
                result.WriteByte((byte)hexChars[idx]);
                idx = (i >> 8) & 0x0F;
                result.WriteByte((byte)hexChars[idx]);
                idx = (i >> 4) & 0x0F;
                result.WriteByte((byte)hexChars[idx]);
                idx = i & 0x0F;
                result.WriteByte((byte)hexChars[idx]);
                return;
            }

            if (ch > ' ' && notEncodedChars.IndexOf(ch) != -1)
            {
                result.WriteByte((byte)ch);
                return;
            }
            if (ch == ' ')
            {
                result.WriteByte((byte)'+');
                return;
            }
            if ((ch < '0') ||
                (ch < 'A' && ch > '9') ||
                (ch > 'Z' && ch < 'a') ||
                (ch > 'z'))
            {
                if (isUnicode && ch > 127)
                {
                    result.WriteByte((byte)'%');
                    result.WriteByte((byte)'u');
                    result.WriteByte((byte)'0');
                    result.WriteByte((byte)'0');
                }
                else
                    result.WriteByte((byte)'%');

                int idx = ((int)ch) >> 4;
                result.WriteByte((byte)hexChars[idx]);
                idx = ((int)ch) & 0x0F;
                result.WriteByte((byte)hexChars[idx]);
            }
            else
                result.WriteByte((byte)ch);
        }
        #endregion
    }
}
