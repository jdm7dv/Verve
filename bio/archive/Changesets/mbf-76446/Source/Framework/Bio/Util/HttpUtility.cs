// *********************************************************
// 
//     Copyright (c) Microsoft. All rights reserved.
//     This code is licensed under the Apache License, Version 2.0.
//     THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//     ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//     IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//     PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// 
// *********************************************************
using System;
using System.IO;

namespace Bio.Util
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
        /// <param name="enc">The System.Text.Encoding object that specifies the encoding scheme.</param>
        /// <returns>An encoded string.</returns>
        public static string UrlEncode(string str, System.Text.Encoding enc)
        {
            if (enc == null)
            {
                throw new ArgumentNullException("enc");
            }

            if(string.IsNullOrEmpty(str))
            {
                return str;
            }

            byte[] bytes = enc.GetBytes(str);
            return System.Text.Encoding.ASCII.GetString(UrlEncodeToBytes(bytes, 0, bytes.Length));
        }
        #endregion

        #region Private Methods
        
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

            using (MemoryStream result = new MemoryStream(count))
            {
                int end = offset + count;
                for (int i = offset; i < end; i++)
                    UrlEncodeChar((char)bytes[i], result, false);

                return result.ToArray();
            }
        }

        // Encodes specified char and stores the result in specified stream.
        private static void UrlEncodeChar(char ch, Stream result, bool isUnicode)
        {
            if (ch > 255)
            {
                if (!isUnicode)
                    throw new ArgumentOutOfRangeException("ch", ch, Properties.Resource.ParamCHmustbeLessThan256);
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
