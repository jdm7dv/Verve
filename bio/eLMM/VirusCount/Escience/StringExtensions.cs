//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bio.Util;

namespace MBT.Escience
{
    public static class StringExtensions
    {
        public static string ToMixedInvariant(this string s)
        {
            if ("" == s)
            {
                return s;
            }

            return char.ToUpperInvariant(s[0]).ToString() + s.Substring(1).ToLowerInvariant();

        }

        public static string Reverse(this string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            string result = new string(charArray);
            return result;
        }

        /// <summary>
        /// Splits a string, but allows you to protect using, for example, balanced parentheses.
        /// </summary>
        /// <param name="s">String to split</param>
        /// <param name="openChar">The open paren character</param>
        /// <param name="closeChar">The close paren character</param>
        /// <param name="removeEmptyItems">If true, the empty string will never by emitted.</param>
        /// <param name="splitChars">List of characters on which to split</param>
        /// <returns>Strings between split characters that are not wrapped in protecting parens.</returns>
        public static IEnumerable<string> ProtectedSplit(this string s, char openChar, char closeChar, bool removeEmptyItems, params char[] splitChars)
        {
            int depth = 0;
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
            {
                if (c == openChar)
                {
                    depth++;
                    sb.Append(c);
                }
                else if (c == closeChar)
                {
                    depth--;
                    sb.Append(c);
                    Helper.CheckCondition(depth >= 0, "Unbalanced parentheses.");
                }
                else if (splitChars.Contains(c))
                {
                    if (depth > 0)
                        sb.Append(c);
                    else
                    {
                        if (!removeEmptyItems || sb.Length > 0)
                            yield return sb.ToString();
                        sb = new StringBuilder();
                    }
                }
                else
                    sb.Append(c);
            }

            if (!removeEmptyItems || sb.Length > 0)
                yield return sb.ToString();
        }
    }
}
