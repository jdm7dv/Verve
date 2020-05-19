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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Bio.Util;

namespace MBT.Escience.Parse
{
    public class HelpException : Exception
    {
        public string RawMessage { get; private set; }

        public HelpException(string message)
            : base(FormatMessage(message))
        {
            RawMessage = message;
        }

        public HelpException(string messageFormat, params object[] args) : this(string.Format(messageFormat, args)) { }


        private static string FormatMessage(string message)
        {
            int windowWidth =
#if !SILVERLIGHT
                Console.BufferWidth;
#else
                80;
#endif
            int indentWidth = 5;
            string indentString = Enumerable.Repeat(' ', 5).StringJoin("");

            StringBuilder result = new StringBuilder();

            StringBuilder line = new StringBuilder(windowWidth);
            int indents = 0;

            foreach (string word in Tokens(message))
            {
                switch (word)
                {
                    case "<br>":
                        result.AppendLine(line.ToString());
                        line = new StringBuilder(windowWidth);
                        line.Append(IndentString(indents, indentWidth));
                        break;
                    case "<indent>":
                        indents++;
                        line.Append(IndentString(1, indentWidth));  // add one indent to the current line.
                        break;
                    case "</indent>":
                        indents--; break;
                    default:
                        if (line.Length + word.Length + 1 < windowWidth)
                            line.Append(word + " ");
                        else
                        {
                            result.AppendLine(line.ToString());
                            line = new StringBuilder(windowWidth);
                            line.Append(IndentString(indents, indentWidth) + word + " ");
                        }
                        break;
                }
            }
            result.AppendLine(line.ToString());
            result.Append(GetDateCompiledString());
            return result.ToString();
        }

        private static string GetDateCompiledString()
        {
            DateTime compileDate = SpecialFunctions.DateProgramWasCompiled();
            if (compileDate.Ticks > 0)
                return "Program last modified " + compileDate;
            else
                return "";
        }

        private static string IndentString(int indents, int indentWidth)
        {
            return Enumerable.Repeat(' ', indents * indentWidth).StringJoin("");
        }

        private static IEnumerable<string> Tokens(string str)
        {
            str = str.Replace("\n\n", "<br><br>");
            string wordBeforeTag = "";
            StringBuilder word = new StringBuilder();

            int tagDepth = 0;
            foreach (var c in str)
            {
                if (c == '<')
                {
                    tagDepth++;
                    if (word.Length > 0 && tagDepth == 1 /*is start of new tag*/)
                    {
                        wordBeforeTag = word.ToString();
                        word = new StringBuilder();
                    }
                    word.Append(c);
                }
                else if (tagDepth > 0)
                {
                    word.Append(c);
                    if (c == '>')
                    {
                        tagDepth--;
                        if (tagDepth == 0)
                        {
                            string tag = word.ToString();
                            if (KnownTag(tag))
                            {
                                if (wordBeforeTag.Length > 0)
                                    yield return wordBeforeTag;
                                yield return tag.ToLower();
                            }
                            else
                            {
                                yield return wordBeforeTag + tag;
                            }
                            wordBeforeTag = "";
                            word = new StringBuilder();
                        }
                    }
                }
                else if (Char.IsWhiteSpace(c))
                {
                    if (word.Length > 0)
                    {
                        yield return word.ToString();
                        word = new StringBuilder();
                    }
                }
                else
                {
                    word.Append(c);
                }
            }
            if (word.Length > 0)
                yield return word.ToString();
        }

        private static bool KnownTag(string tag)
        {
            return tag.Equals("<indent>", StringComparison.CurrentCultureIgnoreCase) ||
                tag.Equals("</indent>", StringComparison.CurrentCultureIgnoreCase) ||
                tag.Equals("<br>", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
