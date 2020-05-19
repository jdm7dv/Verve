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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Bio.Util;
using MBT.Escience.Parse;
using System.IO.Compression;

namespace MBT.Escience
{
    [Flags]
    public enum FileOperator
    {
        Default = 0, DeleteFiles = 1, PrintProgress = 2, ContinueOnError = 4, Append = 8, RelaxIntegrityChecks = 16
    }

    
    public class FileAccessComparer : IEqualityComparer<FileInfo>
    {
        #region IEqualityComparer<FileInfo> Members

        public bool Equals(FileInfo x, FileInfo y)
        {
            return x.Name.Equals(y.Name) && x.LastWriteTime.Equals(y.LastWriteTime);
        }

        public int GetHashCode(FileInfo obj)
        {
            return obj.Name.GetHashCode();
        }

        #endregion
    }


    public static class FileUtils
    {
        public const string DEFAULT_COMMENT_TOKEN = "//";
        public static void AddCommentHeader(string filename, IEnumerable<string> commentLines)
        {
            string tempFileName = Path.GetTempFileName();
            using (TextWriter writer = File.CreateText(tempFileName))
            {
                writer.WriteLine(Bio.Util.FileUtils.CommentHeader + DEFAULT_COMMENT_TOKEN);
                foreach (string s in commentLines)
                {
                    writer.WriteLine("{0}{1}", s.StartsWith(DEFAULT_COMMENT_TOKEN) ? "" : DEFAULT_COMMENT_TOKEN + " ", s);
                }

                using (TextReader reader = Bio.Util.FileUtils.OpenTextStripComments(filename))
                {
                    string line;
                    while (null != (line = reader.ReadLine()))
                    {
                        writer.WriteLine(line);
                    }
                }
            }
            File.Delete(filename);
            File.Move(tempFileName, filename);
        }

        /// <summary>
        /// </summary>
        /// <param name="fileName">The name of the file from which to read.</param>
        /// <returns>a sequence of lines from a file</returns>
        public static IEnumerable<string> ReadEachLineAllowGzip(string fileName)
        {
            FileInfo file = new FileInfo(fileName);
            return ReadEachLineAllowGzip(file);
        }
        
        /// <summary>
        /// Returns a sequence of lines from a file.
        /// </summary>
        /// <param name="file">A FileInfo from which to read lines.</param>
        /// <returns>a sequence of lines from a file</returns>
        public static IEnumerable<string> ReadEachLineAllowGzip(this FileInfo file)
        {
            using (TextReader textReader = SpecialFunctions.OpenTextOrZippedText(file))
            {
                string line;
                while (null != (line = textReader.ReadLine()))
                {
                    yield return line;
                }
            }
        }

        public static IEnumerable<string> GetFiles(string tabFilePattern)
        {
            string directoryName = Path.GetDirectoryName(tabFilePattern);
            return Directory.EnumerateFiles(directoryName, Path.GetFileName(tabFilePattern));
        }
        
        public static void WriteComment(this TextWriter writer, string commentLine, bool includeCommentHeader = false)
        {
            if (includeCommentHeader)
                writer.WriteLine(Bio.Util.FileUtils.CommentHeader + DEFAULT_COMMENT_TOKEN);

            writer.WriteLine("{0}{1}", commentLine.StartsWith(DEFAULT_COMMENT_TOKEN) ? "" : DEFAULT_COMMENT_TOKEN + " ", commentLine);
        }

        public static void WriteComments(this TextWriter writer, IEnumerable<string> commentLines, bool includeCommentHeader = false)
        {
            if (includeCommentHeader)
                writer.WriteLine(Bio.Util.FileUtils.CommentHeader + DEFAULT_COMMENT_TOKEN);

            foreach (string s in commentLines)
            {
                WriteComment(writer, s);
                //writer.WriteLine("{0}{1}", s.StartsWith(DEFAULT_COMMENT_TOKEN) ? "" : DEFAULT_COMMENT_TOKEN + " ", s);
            }
        }

        //public static StreamReader OpenTextStripComments(this FileInfo file)
        //{
        //    return new CommentedStreamReader(file);
        //}

        //public static StreamReader OpenTextStripComments(string filename)
        //{
        //    return new CommentedStreamReader(filename);
        //}

        //public static StreamReader StripComments(Stream stream)
        //{
        //    return new CommentedStreamReader(stream);
        //}

        //This shouldn't be used because it hides multiple unneeded ToList's. Instead use Skip(1) and ToList()
        public static List<string> ReadEachLine(string file, bool skipHeaderLine)
        {
            List<string> myList = Bio.Util.FileUtils.ReadEachLine(file).ToList();
            if (skipHeaderLine)
            {
                myList = myList.SubSequence(1, myList.Count - 1).ToList();
            }
            return myList;
        }

        public static void ArchiveExes(string archivePath)
        {
            ArchiveExes(archivePath, true);
        }

        public static void ArchiveExes(string archivePath, bool overwriteExistingExesWithSameBuildNumber)
        {
            AssemblyName assemblyName = SpecialFunctions.GetEntryOrCallingAssembly().GetName();
            string exeName = Path.GetFileNameWithoutExtension(assemblyName.Name);

            string fullArchivePath = string.Format(@"{0}\{1}\{2}.{3}\{4}", archivePath, exeName, assemblyName.Version.Major, assemblyName.Version.Minor, assemblyName.Version);

            bool alreadyThere = true;
            if (!Directory.Exists(fullArchivePath))
            {
                Directory.CreateDirectory(fullArchivePath);
                alreadyThere = false;
            }

            if (!alreadyThere || overwriteExistingExesWithSameBuildNumber)
            {
                Console.WriteLine("Archiving exes to {0}", fullArchivePath);

                string srcDirectoryName = Path.GetDirectoryName(SpecialFunctions.GetEntryOrCallingAssembly().Location);
                SpecialFunctions.CopyDirectory(srcDirectoryName, fullArchivePath, /*recursive*/ true);
            }
        }

        public static IEnumerable<string> ReadAllComments(string filename)
        {
            return new FileInfo(filename).ReadAllComments();
            //using (CommentedStreamReader reader = new CommentedStreamReader(filename))
            //{
            //    foreach (string line in reader.ReadAllComments())
            //        yield return line;
            //}
        }

        public static IEnumerable<string> ReadAllComments(this FileInfo file)
        {
            using (CommentedStreamReader reader = new CommentedStreamReader(file))
            {
                foreach (string line in reader.ReadAllComments())
                {
                    if (!line.Equals(Bio.Util.FileUtils.CommentHeader + DEFAULT_COMMENT_TOKEN))
                    {
                        //string lineNoCommentToken = line.Substring(DEFAULT_COMMENT_TOKEN.Length);
                        yield return line;
                    }
                }
            }
        }

        public static string ReadLine(FileInfo file)
        {
            using (StreamReader streamReader = file.OpenTextStripComments())
            {
                return streamReader.ReadLine();
            }
        }

        public static string ReadLine(string filename)
        {
            FileInfo file = new FileInfo(filename);
            if (!file.Exists)
                throw new FileNotFoundException(filename + " does not exist.");
            return ReadLine(file);
            //using (StreamReader streamReader = Bio.Util.FileUtils.OpenTextStripComments(filename))
            //{
            //    return streamReader.ReadLine();
            //}
        }

        //public static IEnumerable<string> ReadEachLine(string fileName)
        //{
        //    FileInfo file = new FileInfo(fileName);
        //    return file.ReadEachLine();
        //}

        //public static IEnumerable<string> ReadEachLine(TextReader textReader)
        //{
        //    string line;
        //    while (null != (line = textReader.ReadLine()))
        //    {
        //        yield return line;
        //    }
        //}

        //public static IEnumerable<string> ReadEachLine(this FileInfo file)
        //{
        //    using (TextReader textReader = file.OpenTextStripComments())
        //    {
        //        string line;
        //        while (null != (line = textReader.ReadLine()))
        //        {
        //            yield return line;
        //        }
        //    }
        //}

        //        public static IEnumerable<KeyValuePair<string, int>> ReadEachIndexedLine(string fileName)
        //        {
        //            FileInfo file = new FileInfo(fileName);
        //            return file.ReadEachIndexedLine();
        //        }

        //        public static IEnumerable<KeyValuePair<string, int>> ReadEachIndexedLine(TextReader textReader)
        //        {
        //            string line;
        //            int i = 0;
        //            while (null != (line = textReader.ReadLine()))
        //            {
        //                yield return new KeyValuePair<string, int>(line, i);
        //                ++i;
        //            }
        //        }

        //        public static IEnumerable<KeyValuePair<string, int>> ReadEachIndexedLine(this FileInfo file)
        //        {
        //            using (TextReader textReader = file.OpenTextStripComments())
        //            {
        //                string line;
        //                int i = 0;
        //                while (null != (line = textReader.ReadLine()))
        //                {
        //                    yield return new KeyValuePair<string, int>(line, i);
        //                    ++i;
        //                }
        //            }
        //        }



        public static void DeleteFilePatiently(string file)
        {
            var myQueue = new Queue<FileInfo>();
            myQueue.Enqueue(new FileInfo(file));
            SpecialFunctions.DeleteFilesPatiently(myQueue, 2, new TimeSpan(0, 5, 0));
        }

        public static void DeleteFilesPatiently(IEnumerable<FileInfo> filesToDelete, int numberOfTimesToTry, TimeSpan timeToWaitBetweenRetries)
        {
            Queue<FileInfo> filesNotDeleted = new Queue<FileInfo>();

            foreach (FileInfo fileInfo in filesToDelete)
            {
                try
                {
                    fileInfo.Delete();
                }
                catch (System.IO.IOException)
                {
                    filesNotDeleted.Enqueue(fileInfo);
                }
            }

            if (filesNotDeleted.Count > 0)
            {
                if (numberOfTimesToTry > 0)
                {
                    Console.WriteLine("Could not delete all files. Will retry after a short pause.");
                    System.Threading.Thread.Sleep(timeToWaitBetweenRetries);
                    DeleteFilesPatiently(filesNotDeleted, numberOfTimesToTry - 1, timeToWaitBetweenRetries);
                }
                else
                {
                    StringBuilder sb = new StringBuilder("Could not delete the following files: ");
                    sb.Append(filesNotDeleted.StringJoin(","));
                    throw new Exception(sb.ToString());
                }
            }
        }

        public static void ConcatenateFiles(DirectoryInfo dirinfo, string inputFilePattern, string outputFileName, FileOperator fileFlags)
        {
            bool deleteFilesOnSuccessfullMerge = (fileFlags & FileOperator.DeleteFiles) == FileOperator.DeleteFiles;
            bool continueOnFailure = (fileFlags & FileOperator.ContinueOnError) == FileOperator.ContinueOnError;
            bool printProgress = (fileFlags & FileOperator.PrintProgress) == FileOperator.PrintProgress;
            bool appendToExistingFile = (fileFlags & FileOperator.Append) == FileOperator.Append;

            List<FileInfo> filesInMerge = new List<FileInfo>();

            if (printProgress)
                Console.WriteLine();

            using (TextWriter textWriter = outputFileName == "-" ? Console.Out : (appendToExistingFile ? File.AppendText(outputFileName) : File.CreateText(outputFileName))) // Do this early so that if it fails, well know
            {
                foreach (FileInfo fileinfo in dirinfo.EnumerateFiles(inputFilePattern))
                {
                    if (printProgress)
                        Console.Write("\rConcatenating {0}", fileinfo.Name);

                    filesInMerge.Add(fileinfo);
                    fileinfo.ReadEachLine().ForEach(line => textWriter.WriteLine(line));
                }
            }

            if (printProgress)
                Console.WriteLine();

            if (deleteFilesOnSuccessfullMerge)
            {
                FileInfo outputFileInfo = new FileInfo(outputFileName);
                var filesToDelete = filesInMerge.Where(file => !file.FullName.Equals(outputFileInfo.FullName));

                DeleteFilesPatiently(filesToDelete, 5, new TimeSpan(0, 1, 0)); // will try deleting for up to ~5 min before bailing.
            }
        }

        public static void MergeFiles(string inputFilePattern, string[] columnNamesToAdd, string outputFileName, FileOperator fileFlags)
        {
            var files = Bio.Util.FileUtils.GetFiles(inputFilePattern, /*zeroIsOK*/ false).ToList();
            string dirName = Path.GetDirectoryName(files[0]);
            var inputFilePatternCollection =
            files.Select(fullPath =>
            {
                Helper.CheckCondition(dirName == Path.GetDirectoryName(fullPath), "all files in the pattern must be in the same directory");
                return Path.GetFileName(fullPath);
            });

            var dirInfo = new DirectoryInfo(dirName);

            MergeFiles(dirInfo, inputFilePatternCollection, columnNamesToAdd, outputFileName, fileFlags);
        }

        public static int MergeFiles(DirectoryInfo dirinfo, string inputFilePattern, string outputFileName, FileOperator fileFlags)
        {
            return MergeFiles(dirinfo, SpecialFunctions.CreateSingletonList(inputFilePattern), new string[] { }, outputFileName, fileFlags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirinfo"></param>
        /// <param name="inputFilePatternCollection"></param>
        /// <param name="columnNamesToAdd"></param>
        /// <param name="outputFileName"></param>
        /// <param name="fileFlags"></param>
        /// <returns># of files merged</returns>
        public static int MergeFiles(DirectoryInfo dirinfo, IEnumerable<string> inputFilePatternCollection, string[] columnNamesToAdd, string outputFileName, FileOperator fileFlags)
        {
            bool deleteFilesOnSuccessfullMerge = (fileFlags & FileOperator.DeleteFiles) == FileOperator.DeleteFiles;
            bool continueOnFailure = (fileFlags & FileOperator.ContinueOnError) == FileOperator.ContinueOnError;
            bool printProgress = (fileFlags & FileOperator.PrintProgress) == FileOperator.PrintProgress;
            bool appendToExistingFile = (fileFlags & FileOperator.Append) == FileOperator.Append;
            bool relaxIntegrityChecks = (fileFlags & FileOperator.RelaxIntegrityChecks) == FileOperator.RelaxIntegrityChecks;

            int numFilesMerged = 0;

            //string tmpFile = outputFileName + new Random().Next(int.MaxValue);
            List<Tuple<string, string[]>> filePatternsAndColumnNames = new List<Tuple<string, string[]>>();
            foreach (string filePattern in inputFilePatternCollection)
            {
                string[] filePatternAndColValues = filePattern.Split(':');
                Helper.CheckCondition(filePatternAndColValues.Length == 1 + columnNamesToAdd.Length, "{0} does not have the right number of added columns specified. Expected {1}; found {2}.", filePattern, columnNamesToAdd.Length, filePatternAndColValues.Length - 1);
                string[] colVals = columnNamesToAdd.Length == 0 ? new string[0] : SpecialFunctions.SubArray(filePatternAndColValues, 1);
                filePatternsAndColumnNames.Add(Tuple.Create(filePatternAndColValues[0], colVals));
            }

            string universalHeader = null;
            int headerColCount = -1;

            if (appendToExistingFile && File.Exists(outputFileName))
            {
                universalHeader = Bio.Util.FileUtils.ReadLine(outputFileName);
                Helper.CheckCondition(universalHeader != null, "header is null");
                headerColCount = CountCols(universalHeader, '\t');
            }

            List<FileInfo> filesInMerge = new List<FileInfo>();
            //FileInfo outputFileInfo = new FileInfo(outputFileName);
            using (TextWriter textWriter = outputFileName == "-" ? Console.Out : (appendToExistingFile ? File.AppendText(outputFileName) : File.CreateText(outputFileName))) // Do this early so that if it fails, well know
            {

                foreach (Tuple<string, string[]> filePatternAndColVals in filePatternsAndColumnNames)
                {
                    if (printProgress)
                        Console.WriteLine("\nMerging files matching pattern {0}", filePatternAndColVals.Item1);

                    string addedCols = columnNamesToAdd.Length > 0 ? Helper.CreateTabString(filePatternAndColVals.Item2) + '\t' : "";

                    foreach (FileInfo fileinfo in dirinfo.EnumerateFiles(filePatternAndColVals.Item1)) //EnumerateFiles(dirinfo, inputFilePatternCollection))
                    {
                        if (fileinfo.Name.Equals(outputFileName, StringComparison.CurrentCultureIgnoreCase)) continue;
                        if (printProgress)
                            Console.Write("\rMerging {0}", fileinfo.Name);
                        filesInMerge.Add(fileinfo);
                        numFilesMerged++;

                        if (fileinfo.Length == 0) continue; // merging with an empty file has no effect
                        string headerOnFile;
                        int lineNum = 0;
                        //using (TextReader reader = SpecialFunctions.GetTextReaderWithExternalReadWriteAccess(fileinfo.FullName))
                        //this might lead to sharing violoations
                        using (TextReader reader = Bio.Util.FileUtils.OpenTextStripComments(fileinfo.FullName))
                        {
                            headerOnFile = reader.ReadLine();
                            lineNum++;
                            if (universalHeader == null)
                            {
                                universalHeader = headerOnFile;
                                if (columnNamesToAdd.Length > 0)
                                {
                                    textWriter.WriteLine(Helper.CreateTabString(columnNamesToAdd) + '\t' + universalHeader);
                                }
                                else
                                {
                                    textWriter.WriteLine(universalHeader);
                                }
                                headerColCount = CountCols(universalHeader, '\t');
                            }
                            else if (universalHeader != headerOnFile)
                            {
                                //File.Delete(tmpFile);
                                throw new ArgumentException(string.Format("ERROR: The header for file {0} is different from the 1st file read in. \nCurrent header: {1}\nFirst header: {2}\nAborting file merge.",
                                    fileinfo.Name, headerOnFile, universalHeader), fileinfo.Name);
                            }



                            //using (TextReader reader = SpecialFunctions.GetTextReaderWithExternalReadWriteAccess(fileinfo.FullName))
                            //{
                            try
                            {
                                string line;
                                while (null != (line = reader.ReadLine()))
                                {
                                    lineNum++;
                                    if (line.Length == 0)
                                        continue;


                                    if (!relaxIntegrityChecks)
                                    {
                                        int colCount = CountCols(line, '\t');
                                        Helper.CheckCondition(colCount == headerColCount, "line {0} has {1} columns. Expected {2}. Line reads {3}", lineNum, colCount, headerColCount, line);
                                    }

                                    if (columnNamesToAdd.Length > 0)
                                    {
                                        textWriter.WriteLine(addedCols + line);  // write the whole line
                                    }
                                    else
                                    {
                                        textWriter.WriteLine(line);  // write the whole line
                                    }
                                }

                                //foreach (Dictionary<string, string> row in SpecialFunctions.TabFileTable(reader, headerOnFile, /*includeWholeLine*/ true))
                                //{
                                //    if (columnNamesToAdd.Length > 0)
                                //    {
                                //        textWriter.WriteLine(Helper.CreateTabString(filePatternAndColVals.Second) + '\t' + row[""]);  // write the whole line
                                //    }
                                //    else
                                //    {
                                //        textWriter.WriteLine(row[""]);  // write the whole line
                                //    }
                                //}
                            }
                            catch (Exception e)
                            {
                                if (!continueOnFailure)
                                {
                                    Console.WriteLine("TEST: Caught an error, so rethrowing.");
                                    throw;
                                }
                                else
                                {
                                    if (printProgress)
                                    {
                                        Console.Error.WriteLine("Error processing " + fileinfo.Name + ". " + e.Message);
                                    }
                                }
                            }
                        }
                    }
                }
                if (printProgress)
                    Console.WriteLine();
            }

            //if (outputFileName != "-")
            //{
            //    SpecialFunctions.MoveAndReplace(tmpFile, outputFileName);
            //}

            if (deleteFilesOnSuccessfullMerge)
            {
                FileInfo outputFileInfo = new FileInfo(outputFileName);
                var filesToDelete = filesInMerge.Where(file => !file.FullName.Equals(outputFileInfo.FullName));

                Console.WriteLine("Trying patiently to delete merged files");
                DeleteFilesPatiently(filesToDelete, 5, new TimeSpan(0, 1, 0)); // will try deleting for up to ~5 min before bailing.
            }
            return numFilesMerged;
        }

        private static int CountCols(string line, char delim)
        {
            int idx = -1;
            int count = 1;  // if one delim, then 2 cols...
            while (0 <= (idx = line.IndexOf(delim, idx + 1)))
            {
                count++;
            }
            return count;
        }

        public static bool TryToOpenFile(string filename, TimeSpan timeout, FileMode fileMode, FileAccess fileAcces, FileShare fileShare, out FileStream filestream)
        {
            filestream = null;
            //int i = 0;
            long start = DateTime.Now.Ticks;
            long timeoutTicks = timeout.Ticks;
            int breakTime = 50;
            while (true)
            {
                try
                {
                    filestream = File.Open(filename, fileMode, fileAcces, fileShare);
                    return true;
                }
                catch
                {
                    if (DateTime.Now.Ticks - start < timeoutTicks)
                    {
                        Thread.Sleep(breakTime);
                        breakTime *= 2;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }



        public static string GetDirectoryName(string exampleFileToCopy)
        {
            return GetDirectoryName(exampleFileToCopy, "");
        }
        public static string GetDirectoryName(string exampleFileToCopy, string workingDirectory)
        {
            string fullPathToExample = Path.Combine(workingDirectory, exampleFileToCopy);
            bool illegalCharactersInPath = Path.GetInvalidPathChars().Any(c => exampleFileToCopy.Contains(c));
            if (!illegalCharactersInPath && Directory.Exists(fullPathToExample))
                return exampleFileToCopy;
            return Path.GetDirectoryName(exampleFileToCopy);
        }


#if !SILVERLIGHT
        public static string GetNetworkPathNameToLocalComputer(string localPathName)
        {
            string pathName = Path.GetFullPath(localPathName);
            string computerName = Environment.GetEnvironmentVariable("computername");
            string rootDir = Path.GetPathRoot(pathName);
            string networkRootDir = rootDir.Replace(Path.VolumeSeparatorChar, '$');
            string networkRelativePathName = pathName.Replace(rootDir, networkRootDir);

            string networkPathName = string.Format(@"\\{0}\{1}", computerName, networkRelativePathName);
            Console.WriteLine("Network path name: " + networkPathName);
            return networkPathName;
        }
#endif



        public static void DeleteFiles(string directoryName, string filepattern)
        {
            DirectoryInfo dir = new DirectoryInfo(directoryName);
            Helper.CheckCondition(dir.Exists, "Directory " + directoryName + " does not exist.");
            foreach (FileInfo file in dir.EnumerateFiles(filepattern))
            {
                if (file.Exists)
                {
                    file.Delete();
                }
            }
        }

        /// <summary>
        /// Creates a unique directory starting the the given path. If the directory already exists, then tries creating a new version of 
        /// the form baseDirName_x, where x is an ever incrementing number. If includDateStamp, then the directory will be
        /// baseDirName_date[_x]. 
        /// </summary>
        /// <param name="baseDirName">The full path to the new directory.</param>
        /// <param name="includeDateStamp"></param>
        /// <returns></returns>
        public static string CreateUniqDirectory(string baseDirName, bool includeDateStamp)
        {
            string newDirNameBase = includeDateStamp ?
                newDirNameBase = baseDirName + "_" + DateTime.Now.ToShortDateString().Replace("/", "_") :
                newDirNameBase = baseDirName;

            string newDirName = null;
            for (int suffixIndex = 0; ; ++suffixIndex)
            {
                string suffix = suffixIndex == 0 ? "" : "_" + suffixIndex.ToString();
                newDirName = newDirNameBase + suffix;

                //!!!Two instances of this program could (it is possible) create the same directory.
                if (!Directory.Exists(newDirName))
                {
                    try
                    {
                        Directory.CreateDirectory(newDirName);
                    }
                    catch (IOException)
                    {
                        Console.Error.WriteLine("Another process seems to have created the directory. Moving on.");
                    }
                    break;
                }
            }
            return newDirName;
        }

        public static IEnumerable<T> ReadDelimitedFile<T>(string filePattern, T sample, char[] separatorList, bool hasHeader)
        {
            return ReadDelimitedFile(filePattern, sample, hasHeader, separatorList);
        }

        /// <summary>
        /// A method to read delimited, formatted, text files (e.g. csv, or tab-delimited) into objects of the correct type (without needing a futher cast).
        /// This is slower than TabFileTable, which has similar funcitonality, but is faster, doesn't parse into the correct type, but whos output can
        /// be passed on to other functions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePattern">name of file to open</param>
        /// <param name="sample">An anonymous tuple where you define the column names and their types. e.g., new { var = "", cid = "", val = 0 },
        /// Two strings, and one integer. Note these are case sensitive</param>
        /// <param name="separatorList">List of the delimiters that you accept. e.g. new char[] {'\t'}</param>
        /// <param name="hasHeader">if true assumes a one line header, and will throw exception if doesn't match info above</param>
        /// <returns></returns>
        public static IEnumerable<T> ReadDelimitedFile<T>(string filePattern, T sample, bool hasHeader, params char[] separatorList)
        {
            foreach (string fileName in Bio.Util.FileUtils.GetFiles(filePattern, /*zeroIsOK*/ false))
            {
                using (TextReader textReader = Bio.Util.FileUtils.OpenTextStripComments(fileName))
                {
                    foreach (T t in ReadDelimitedFile(textReader, sample, hasHeader, separatorList))
                    {
                        yield return t;
                    }
                }
            }
        }

        //public static IEnumerable<T> ReadDelimitedFile<T>(TextReader textReader, T sample, char[] separatorList, bool hasHeader)
        //{
        //    return ReadDelimitedFile(textReader, sample, hasHeader, separatorList);
        //}
        public static IEnumerable<T> ReadDelimitedFile<T>(this FileInfo file, T sample, bool hasHeader, params char[] separatorList)
        {
            using (TextReader reader = file.OpenTextStripComments())
            {
                foreach (T line in reader.ReadDelimitedFile(sample, hasHeader, separatorList))
                    yield return line;
            }
        }

        public static IEnumerable<T> ReadDelimitedFile<T>(this TextReader textReader, T sample, bool hasHeader, params char[] separatorList)
        {
            int columnCount = sample.GetType().GetProperties().Count();


            if (hasHeader)
            {
                string header = textReader.ReadLine();
                Helper.CheckCondition(header != null, "File is empty so can't read header.");
                string[] columns = header.Split(separatorList);
                Helper.CheckCondition(columns.Length == columnCount,
                    string.Format("Expected {0} {3}columns, but found {1}. '{2}'", columnCount, columns.Length, header,
                    (separatorList.Length == 1 && separatorList[0] == '\t') ? "tab-delimited " : ""
                    ));


                for (int iColumn = 0; iColumn < columnCount; ++iColumn)
                {
#if SILVERLIGHT
                    string inputColumn = columns[iColumn].ToLower();
                    string sampleColumn = sample.GetType().GetProperties()[iColumn].Name.ToLower();
#else
                    string inputColumn = columns[iColumn].ToLowerInvariant();
                    string sampleColumn = sample.GetType().GetProperties()[iColumn].Name.ToLowerInvariant();
#endif
                    Helper.CheckCondition(sampleColumn == inputColumn, "Expected header not found. Expected column #{0} to be '{1}', but instead found '{2}'", iColumn + 1, sampleColumn, inputColumn);
                }
            }


            //Check that the columns match the fields in the sample
            object[] parameters = new object[columnCount];
            Type[] types = new Type[columnCount];

            string line;
            while (null != (line = textReader.ReadLine()))
            {
                string[] fields = line.Split(separatorList);
                Helper.CheckCondition(fields.Length == columnCount, "Expected {0} {3}fields, but found {1}. '{2}'", columnCount, fields.Length, line,
                    (separatorList.Length == 1 && separatorList[0] == '\t') ? "tab-delimited " : "");
                for (int fieldIndex = 0; fieldIndex < fields.Length; ++fieldIndex)
                {
                    //string column = columns[fieldIndex].ToLowerInvariant();
                    string field = fields[fieldIndex];
                    PropertyInfo propertyInfo = sample.GetType().GetProperties()[fieldIndex]; //!!! could move propertyInfo look up out of the loop
                    types[fieldIndex] = propertyInfo.PropertyType;
                    parameters[fieldIndex] = MBT.Escience.Parse.Parser.Parse(field, propertyInfo.PropertyType);
                }

                T t = (T)sample.GetType().GetConstructor(types).Invoke(parameters);

                yield return t;
            }
        }

        /// <summary>
        /// A method to read some of the columns from delimited, formatted, text files (e.g. csv, or tab-delimited) into objects of the correct type (without needing a futher cast).
        /// Differs from ReadDelimitedFile in that only some of fields are parsed, and they field order doesn't have to match the order in the header. 
        /// The header must exist, however, and all fields in sample must be in the header. This is case insensitive to the header. 
        /// *** To get the original header back, include a property string[] Header. To get the full line, include string[] Line.***
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePattern">name of file to open</param>
        /// <param name="sample">An anonymous tuple where you define the column names and their types. e.g., new { var = "", cid = "", val = 0, Header = new string[0], Line = string[0] },
        /// Two strings, and one integer, in addition to the original header and line. Note these are NOT case sensitive</param>
        /// <param name="separatorList">List of the delimiters that you accept. e.g. new char[] {'\t'}. If empty, defaults to '\t'.</param>
        /// <returns></returns>
        public static IEnumerable<T> ReadDelimitedFilePartial<T>(string filePattern, T sample, params char[] separatorList)
        {
            foreach (string fileName in Bio.Util.FileUtils.GetFiles(filePattern, /*zeroIsOK*/ false))
            {
                using (TextReader textReader = Bio.Util.FileUtils.OpenTextStripComments(fileName))
                {
                    foreach (T t in ReadDelimitedFilePartial(textReader, sample, separatorList))
                    {
                        yield return t;
                    }
                }
            }
        }
        public static IEnumerable<T> ReadDelimitedFilePartial<T>(FileInfo file, T sample, params char[] separatorList)
        {
            using (TextReader textReader = Bio.Util.FileUtils.OpenTextStripComments(file))
            {
                foreach (T t in ReadDelimitedFilePartial(textReader, sample, separatorList))
                {
                    yield return t;
                }
            }
        }
        public static IEnumerable<T> ReadDelimitedFilePartial<T>(TextReader textReader, T sample, params char[] separatorList)
        {
            if (separatorList.Length == 0) separatorList = new char[] { '\t' };

            Type emitType = sample.GetType();

            Dictionary<string, KeyValuePair<PropertyInfo, int>> propNamesToPropAndIdx = emitType.GetProperties().Select((p, idx) =>
                new KeyValuePair<PropertyInfo, int>(p, idx)).ToDictionary(pAndInt => pAndInt.Key.Name, StringComparer.CurrentCultureIgnoreCase);

            Type[] propertyTypes = emitType.GetProperties().Select(p => p.PropertyType).ToArray();

            string header = textReader.ReadLine();
            Helper.CheckCondition(header != null, "File is empty so can't read header.");
            string[] columns = header.Split(separatorList);
            PropertyInfo[] properties = new PropertyInfo[columns.Length];

            for (int iColumn = 0; iColumn < columns.Length; ++iColumn)
            {
                if (propNamesToPropAndIdx.ContainsKey(columns[iColumn]))
                    properties[iColumn] = propNamesToPropAndIdx[columns[iColumn]].Key;
                else
                    properties[iColumn] = null;
            }



            string line;
            while (null != (line = textReader.ReadLine()))
            {
                string[] fields = line.Split(separatorList);
                Helper.CheckCondition(fields.Length == columns.Length, "Expected {0} {3}fields, but found {1}. '{2}'", columns.Length, fields.Length, line,
    (separatorList.Length == 1 && separatorList[0] == '\t') ? "tab-delimited " : "");

                object[] parameters = new object[propertyTypes.Length];

                if (propNamesToPropAndIdx.ContainsKey("header"))
                    parameters[propNamesToPropAndIdx["header"].Value] = columns;
                //headerProp.SetValue(t, columns, null);

                if (propNamesToPropAndIdx.ContainsKey("line"))
                    parameters[propNamesToPropAndIdx["line"].Value] = fields;
                //if (lineProp != null)
                //    lineProp.SetValue(t, fields, null);

                for (int fieldIndex = 0; fieldIndex < fields.Length; ++fieldIndex)
                {
                    string field = fields[fieldIndex];
                    string colName = columns[fieldIndex];
                    if (propNamesToPropAndIdx.ContainsKey(colName))
                        parameters[propNamesToPropAndIdx[colName].Value] = MBT.Escience.Parse.Parser.Parse(field, propNamesToPropAndIdx[colName].Key.PropertyType);

                    //PropertyInfo propertyInfo = properties[fieldIndex];
                    //if (propertyInfo != null)
                    //{
                    //    object value = Parser.Parse(field, propertyInfo.PropertyType);
                    //    propertyInfo.SetValue(t, value, null);
                    //}
                }

                T t = (T)emitType.GetConstructor(propertyTypes).Invoke(parameters);

                yield return t;
            }
        }

        public static string GetHeaderFromProperties(object obj, string separator)
        {
            Type tType = obj.GetType();//typeof(T);
            string header;

            var tProps = tType.GetProperties().ToDictionary(p => p.Name, StringComparer.CurrentCultureIgnoreCase);

            if (tProps.ContainsKey("header") && tProps["header"].PropertyType == typeof(string[]) &&
                tProps.ContainsKey("line") && tProps["line"].PropertyType == typeof(string[]))
            {
                string[] headerProp = (string[])tProps["header"].GetValue(obj, null);

                headerProp.ForEach(col => tProps.Remove(col));

                header = headerProp.StringJoin(separator);
                // add columns for any remaining properties.
                if (tProps.Count > 2)
                    header += "\t" + tProps.Keys.Where(p => !p.Equals("Header") && !p.Equals("Line")).StringJoin("\t");
            }
            else
            {
                header = tType.GetProperties().Select(p => p.Name).StringJoin(separator);
            }

            return header;
        }

        public static string GetValueLineFromProperties(object obj, string separator)
        {
            Type tType = obj.GetType();//typeof(T);
            string line;

            var tProps = tType.GetProperties().ToDictionary(p => p.Name, StringComparer.CurrentCultureIgnoreCase);
            if (tProps.ContainsKey("header") && tProps["header"].PropertyType == typeof(string[]) &&
                tProps.ContainsKey("line") && tProps["line"].PropertyType == typeof(string[]))
            {
                string[] cols = (string[])tProps["header"].GetValue(obj, null);
                string[] fields = (string[])tProps["line"].GetValue(obj, null);
                Helper.CheckCondition(cols.Length == fields.Length, "Line and Header are not the same length.");
                //HashSet<string> propsRemaining = tProps.Keys.toha

                line = fields.Select((f, idx) =>
                {
                    if (tProps.ContainsKey(cols[idx]))
                    {
                        string propVal = PropertyValueAsString(obj, tProps[cols[idx]]);
                        tProps.Remove(cols[idx]);
                        return propVal;
                        //object propVal = tProps[cols[idx]].GetValue(obj, null);
                        //IEnumerable asEnum = propVal as IEnumerable;
                        //if (!(propVal is string) && asEnum != null)
                        //    propVal = asEnum.StringJoin(",");
                        //return propVal;
                    }
                    else
                        return fields[idx];
                }).StringJoin(separator);

                if (tProps.Count > 2)
                {
                    line += "\t" + tProps.Where(p => !p.Key.Equals("Header") && !p.Key.Equals("Line")).Select(p => PropertyValueAsString(obj, p.Value)).StringJoin("\t");
                }
            }
            else
            {
                line = tType.GetProperties().Select(p => PropertyValueAsString(obj, p)).StringJoin(separator);
            }

            return line;
        }

        private static string PropertyValueAsString(object obj, PropertyInfo p)
        {
            object propVal = p.GetValue(obj, null);
            IEnumerable asEnum = propVal as IEnumerable;
            if (!(propVal is string) && asEnum != null)
                propVal = asEnum.StringJoin(",");
            return propVal.ToString();
        }

        /// <summary>
        /// Calls writer.WriteLine() on each line in lines.
        /// </summary>
        /// <param name="writer">An open text writer</param>
        /// <param name="lines">Each line will be written to the writer</param>
        public static void WriteEachLine(this TextWriter writer, IEnumerable<string> lines)
        {
            lines.ForEach(line => writer.WriteLine(line));
        }

        /// <summary>
        /// Writes the values to file. The header and each line are constructed from the properties of T. 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="filename"></param>
        /// <param name="writeHeader"></param>
        /// <param name="separator"></param>
        public static void WriteDelimitedFile(this IEnumerable values, string filename, bool writeHeader = true, string separator = "\t")
        {
            using (TextWriter writer = File.CreateText(filename))
            {
                values.WriteDelimitedFile(writer, writeHeader, separator);
            }
        }

        public static void WriteDelimitedFile(this IEnumerable values, TextWriter textWriter, bool writeHeader = true, string separator = "\t")
        {
            //if (writeHeader)
            //    textWriter.WriteLine(GetHeaderFromProperties(values.First(), separator));

            bool headerWritten = !writeHeader;
            foreach (object value in values)
            {
                if (!headerWritten)
                {
                    textWriter.WriteLine(GetHeaderFromProperties(value, separator));
                    headerWritten = true;
                }

                string line = GetValueLineFromProperties(value, separator);
                textWriter.WriteLine(line);
                textWriter.Flush();
            }

            //Type tType = typeof(T);
            //var tProps = tType.GetProperties();
            //if (writeHeader)
            //{
            //    string header = tProps.Select(p => p.Name).StringJoin(separator);
            //    textWriter.WriteLine(header);
            //}

            //foreach (T value in values)
            //{
            //    string line = tProps.Select(p =>
            //    {
            //        object propVal = p.GetValue(value, null);
            //        IEnumerable asEnum = propVal as IEnumerable;
            //        if (!(propVal is string) && asEnum != null)
            //            propVal = asEnum.StringJoin(",");
            //        return propVal;
            //    }).StringJoin(separator);

            //    textWriter.WriteLine(line);
            //}
        }


        /// <summary>
        /// Write's the contents of stdin (up to the first null character) to a temp file and returns that file's name. Useful for allowing 
        /// commandline utilities to read input off a pipe when the available functions only take filenames.
        /// </summary>
        /// <returns>The name of the temp file to which StdIn was written</returns>
        public static string WriteStdInToTempFile()
        {
            return WriteTextReaderToTempFile(Console.In);
        }

        /// <summary>
        /// Write's the contents of reader (up to the first null character) to a temp file and returns that file's name. 
        /// </summary>
        /// <returns>The name of the temp file to which reader was written</returns>
        public static string WriteTextReaderToTempFile(TextReader reader)
        {
            string tempFileName = Path.GetTempFileName();
            using (TextWriter writer = File.CreateText(tempFileName))
            {
                string line;
                while (null != (line = reader.ReadLine()))
                {
                    writer.WriteLine(line);
                }
            }
            return tempFileName;
        }


        /// <summary>
        /// Returns console if fileInfo.Name == "-". Otherwise, creates a new TextWriter. 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="autoFlush">Specifies whether the TextWriter should flush after every write call (ignored if using Console, which always autoflushes)</param>
        /// <returns></returns>
        public static TextWriter CreateTextOrUseConsole(this FileInfo fileInfo, bool autoFlush = true)
        {
            if (fileInfo.Name == "-")
                return Console.Out;

            StreamWriter sw = fileInfo.CreateText();
            sw.AutoFlush = autoFlush;
            return sw;
        }

        public static TextReader OpenTextOrUseConsole(this FileInfo fileInfo, bool stripComments = true)
        {
            if (fileInfo.Name == "-")
            {
                if (stripComments)
                    return new CommentedStreamReader(new MemoryStream(Encoding.UTF8.GetBytes(Console.In.ReadToEnd())));
                else
                    return Console.In;
            }
            else
            {
                if (stripComments)
                    return fileInfo.OpenTextStripComments();
                else
                    return fileInfo.OpenText();
            }
        }

        //    }


        //    public class CommentedStreamReader : StreamReader
        //    {

        //        bool _haveReadFirstLine = false;
        //        bool _isCommented = false;
        //        public string CommentToken
        //        {
        //            get;
        //            private set;
        //        }

        //        public CommentedStreamReader(FileInfo file) : base(file.OpenRead()) { }
        //        public CommentedStreamReader(string filename) : base(filename) { }
        //        public CommentedStreamReader(Stream stream) : base(stream) { }

        //        public override string ReadLine()
        //        {
        //            return ReadCommentOrNonCommentLine(false);
        //        }

        //        public string ReadCommentLine()
        //        {
        //            return ReadCommentOrNonCommentLine(true);
        //        }

        //        protected string ReadCommentOrNonCommentLine(bool returnComment)
        //        {
        //            string line = base.ReadLine();

        //            if (line == null)
        //            {
        //                return null;
        //            }
        //            else if (!_haveReadFirstLine)
        //            {
        //                _haveReadFirstLine = true;
        //                if (line.StartsWith(FileUtils.COMMENT_HEADER))
        //                {
        //                    CommentToken = line.Substring(FileUtils.COMMENT_HEADER.Length);
        //                    _isCommented = true;
        //                    Helper.CheckCondition(CommentToken.Length > 0, "Comment token cannot be 0 length.");
        //                    if (returnComment)
        //                        return line;
        //                    else
        //                        return ReadCommentOrNonCommentLine(returnComment);
        //                }
        //                else
        //                {
        //                    if (returnComment)
        //                        return ReadCommentOrNonCommentLine(returnComment);
        //                    else
        //                        return line;
        //                }

        //            }
        //            else if (_isCommented && line.StartsWith(CommentToken))
        //            {
        //                if (returnComment)
        //                    return line;
        //                else
        //                    return ReadCommentOrNonCommentLine(returnComment);
        //            }
        //            else
        //            {
        //                if (returnComment)
        //                    return ReadCommentOrNonCommentLine(returnComment);
        //                else
        //                    return line;
        //            }
        //        }

        //        public override int Read()
        //        {
        //            throw new NotImplementedException("Not bothering to implement this. Do you want to?");
        //        }

        //        public override int Read(char[] buffer, int index, int count)
        //        {
        //            throw new NotImplementedException("Not bothering to implement this. Do you want to?");
        //        }

        //        public override int Peek()
        //        {
        //            throw new NotImplementedException("Not bothering to implement this. Do you want to?");
        //        }

        //        public override int ReadBlock(char[] buffer, int index, int count)
        //        {
        //            throw new NotImplementedException("Not bothering to implement this. Do you want to?");
        //        }

        //        public override string ReadToEnd()
        //        {
        //            StringBuilder sb = new StringBuilder();
        //            string line;
        //            while (null != (line = ReadLine()))
        //            {
        //                sb.AppendLine(line);
        //            }
        //            return sb.ToString();
        //        }

        //        public IEnumerable<string> ReadAllComments()
        //        {
        //            string line;
        //            while (null != (line = ReadCommentLine()))
        //            {
        //                yield return line;
        //            }
        //        }

#if !SILVERLIGHT

        /// <summary>
        /// The return code from Robocopy is a bit map, defined as follows:
        ///http://ss64.com/nt/robocopy-exit.html
        ///Hex   Decimal  Meaning if set
        ///0×10  16       Serious error. Robocopy did not copy any files.
        ///               Either a usage error or an error due to insufficient access privileges
        ///               on the source or destination directories.
        ///
        ///0×08   8       Some files or directories could not be copied
        ///               (copy errors occurred and the retry limit was exceeded).
        ///               Check these errors further.
        ///
        ///0×04   4       Some Mismatched files or directories were detected.
        ///               Examine the output log. Some housekeeping may be needed.
        ///
        ///0×02   2       Some Extra files or directories were detected.
        ///               Examine the output log for details. 
        ///
        ///0×01   1       One or more files were copied successfully (that is, new files have arrived).
        ///0×00   0       No errors occurred, and no copying was done.
        ///               The source and destination directory trees are completely synchronized. 
        /// </summary>
        [Flags]
        public enum RoboCopyExitCode
        {
            NoCopy = 0,
            OneOrMoreCopySuccessfull = 1,
            SomeExtraDetected = 2,
            SomeMismatchedDetected = 4,
            CopyError = 8,
            SeriousError = 16
        }

        public static RoboCopyExitCode RoboCopyXOR0(TimeSpan timeOut, string copyFromDir, string copyToDir, string fileNameToCopy, bool verbose)
        {
            ProcessStartInfo aProcessStartInfo = new ProcessStartInfo();
            aProcessStartInfo.FileName = "Robocopy";
            aProcessStartInfo.Arguments = string.Format("{0} {1} {2} /XO /R:0", copyFromDir, copyToDir, fileNameToCopy);
            //            /XO :: eXclude Older files
            //            /R:0 :: number of Retries is 0


            aProcessStartInfo.RedirectStandardError = true;
            aProcessStartInfo.RedirectStandardOutput = true;
            aProcessStartInfo.UseShellExecute = false;
            aProcessStartInfo.CreateNoWindow = true;
            Process processExe = Process.Start(aProcessStartInfo);//!!!gets stuck with *.exe crashes
            string sStdOut = processExe.StandardOutput.ReadToEnd(); //must read first to avoid deadlock (for details: search Google or MSDN for: StandardError.ReadToEnd deadlock
            string stdErrorMessage = processExe.StandardError.ReadToEnd();
            processExe.WaitForExit(timeOut.Milliseconds);
            if (verbose)
            {
                Console.WriteLine(sStdOut);
                Console.Error.WriteLine(stdErrorMessage);
            }
            Helper.CheckCondition(processExe.HasExited); //!!!handle the timeout case
            //Helper.CheckCondition(processExe.ExitCode == 0, sStdOut + "\n" + stdErrorMessage); //!!!handle error from the *.exe
            return (RoboCopyExitCode)processExe.ExitCode;
        }

        //!!!will this leave many things open?  
        public static StreamReader UnGZip(this FileInfo file)
        {
            FileStream fileStream = file.OpenRead();
            GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Decompress);
            StreamReader streamReader = new StreamReader(gzipStream);
            return streamReader;
        }
#endif
    }
}
