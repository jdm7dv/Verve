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
using System.IO;
using System.Runtime.Serialization;
#if !SILVERLIGHT
using System.Runtime.Serialization.Formatters.Binary;
#endif
using System.Collections;
using Bio.Util;

namespace MBT.Escience.Parse
{
    /// <summary>
    /// Specifies how the result of IRunnable.Run() is serialized.
    /// </summary>
    public interface IResultFormatter : IDisposable
    {
        FileInfo OutputFile { get; set; }
        void WriteResult(object obj);
        void WriteComments(IEnumerable<string> comments);
        void WriteComment(string comments);
    }

    /// <summary>
    /// Base class for RunResultSerializers that write to files or standard out.
    /// </summary>
    public abstract class RunResultFormatter : IResultFormatter, IParsable
    {
        public RunResultFormatter()
        {
            TextWriter = Console.Out;
        }

        private FileInfo _outputFile = new FileInfo("-");

        /// <summary>
        /// The file that the result will be saved to. Use - for standard out.
        /// </summary>
        [Parse(ParseAction.Required, typeof(OutputFile))]
        public FileInfo OutputFile { get { return _outputFile; } set { _outputFile = value; } }

        protected TextWriter TextWriter { get; private set; }

        public virtual void FinalizeParse()
        {
            try
            {
                TextWriter = OutputFile.CreateTextOrUseConsole();
                AddCommentHeader();
            }
            catch (Exception e)
            {
                throw new ParseException(e.Message);
            }
        }

        public abstract void WriteResult(object obj);

        public void WriteComments(IEnumerable<string> comments)
        {
            foreach (string comment in comments)
            {
                WriteComment(comment);
            }
            TextWriter.Flush();
        }

        public virtual void WriteComment(string comment)
        {
            TextWriter.WriteLine(TurnStringIntoComment(comment));
        }

        protected virtual void AddCommentHeader()
        {
            TextWriter.WriteLine(Bio.Util.FileUtils.CommentHeader + MBT.Escience.FileUtils.DEFAULT_COMMENT_TOKEN);
        }

        protected virtual string TurnStringIntoComment(string comment)
        {
            return comment.StartsWith(MBT.Escience.FileUtils.DEFAULT_COMMENT_TOKEN) ? comment : MBT.Escience.FileUtils.DEFAULT_COMMENT_TOKEN + " " + comment;
        }

        bool _disposed = false;
        public void Dispose()
        {
            if (!_disposed)
            {
                TextWriter.Dispose();
                _disposed = true;
            }
        }
    }


#if !SILVERLIGHT
    /// <summary>
    /// Will serialize the result object to file using binary serialization. Throws a Serialization exception if the object is not serializable.
    /// </summary>
    public class WriteBinary : RunResultFormatter
    {
        public override void FinalizeParse()
        {
            // don't initialize TextWriter or AddCommentHeader()
        }

        public override void WriteResult(object obj)
        {
            IFormatter formatter = new BinaryFormatter();
            if (OutputFile.Name == "-")
            {
                formatter.Serialize(Console.OpenStandardOutput(), obj);
            }
            else
            {
                using (Stream stream = OutputFile.Create()) //new FileStream(, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(stream, obj);
                }
            }
        }


        public override void WriteComment(string comments)
        {
            // do nothing. we don't support writing comments in this format
        }

        protected override void AddCommentHeader()
        {
            // do nothing. we don't support writing comments in this format
        }
    }

    /// <summary>
    /// Will serialize the result object to file using Xml serialization. Throws a Serialization exception if the object is not serializable.
    /// </summary>
    public class WriteXml : RunResultFormatter
    {
        public override void WriteResult(object obj)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());

            serializer.Serialize(TextWriter, obj);

        }

        protected override void AddCommentHeader()
        {
            // do nothing. There is no header for Xml comments.
        }

        protected override string TurnStringIntoComment(string comment)
        {
            return "<!-- " + comment + " -->";
        }
    }

    /// <summary>
    /// Creates a table, where the header is the Property names, and the values are the property values.
    /// If IEnumerable, will write each line to the table. The header will be derived from the first value.
    /// If a non-enumerable object, then the table will consist of two lines (one header and one value).
    /// </summary>
    public class WriteTable : RunResultFormatter
    {
        /// <summary>
        /// The string used to delimit each field in the resulting file.
        /// </summary>
        public string Separator = "\t";

        public override void WriteResult(object obj)
        {
            var asEnumerable = obj as IEnumerable;

            if (asEnumerable != null)
                asEnumerable.WriteDelimitedFile(TextWriter, true, Separator);
            else
            {
                string header = FileUtils.GetHeaderFromProperties(obj, Separator);
                string line = FileUtils.GetValueLineFromProperties(obj, Separator);
                TextWriter.WriteLine(header);
                TextWriter.WriteLine(line);
            }
            TextWriter.Flush();
        }
    }

    /// <summary>
    /// Writes the result of ToString() to OutputFile.
    /// If IEnumerable, will write each line to the file. 
    /// </summary>
    public class WriteList : RunResultFormatter
    {

        public override void WriteResult(object obj)
        {
            var asEnum = obj as IEnumerable;

            if (asEnum != null)
            {
                foreach (object line in asEnum)
                {
                    TextWriter.WriteLine(line.ToString());
                    TextWriter.Flush();
                }
            }
            else
            {
                TextWriter.WriteLine(obj.ToString());
                TextWriter.Flush();
            }
        }
    }

#endif

}
