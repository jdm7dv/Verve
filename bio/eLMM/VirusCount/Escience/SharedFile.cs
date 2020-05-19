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
using System.IO;
using System.Linq;
using Bio.Util;
using MBT.Escience.Parse;

namespace MBT.Escience
{
    public class SharedFile : IDisposable
    {
        //readonly FileInfo _sharedFileInfo;
        string _filename;
        //string _uniqFilename;
        FileStream _filestream;
        StreamWriter _writer;
        StreamReader _reader;
        object _lockObj = new object();
        Random _rand;
        //long _locklength;
        volatile bool _disposed = false;
        volatile bool _haveLock = false;


        public SharedFile(string filename)
        {
            //Helper.CheckCondition(!uniqID.Contains("\\"), "uniqID cannot contain backslashes");
            //_sharedFileInfo = new FileInfo(filename);
            _filename = filename;
            CreateFileIfNotCreated(filename);

            //_uniqFilename = string.Format("{0}_{1}_{2}.txt", filename, uniqID, System.Guid.NewGuid().ToString());
            _rand = new Random(System.Guid.NewGuid().GetHashCode());
        }

        [Obsolete("uniqID is no longer used. You should switch to the other constructor.")]
        public SharedFile(string filename, string uniqID) : this(filename) { }

        public bool ObtainLock()
        {
            if (!_haveLock)
            {
                //Console.WriteLine("Lock requested.");
                _filestream = ObtainLockInternal();

                if (_filestream != null)    // will be null if disposed while obtaining lock. checking if(_disposed) is not threadsafe. so check for null.
                {
                    //Console.WriteLine("Lock obtained.");
                }
            }
            return _haveLock;
        }

        private const int _slowest = 45 * 1000;
        private const int _threshold = 20 * 1000;
        private const int _increment = 1000;
        private const int _fastest = 250;
        private int _sleepTimer = _slowest;
        private FileStream ObtainLockInternal()
        {
            while (!_disposed)
            {
                try
                {
                    lock (_lockObj)
                    {
                        if (!_disposed)
                        {
                            //File.Move(_filename, _uniqFilename);
                            _filestream = File.Open(_filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                            //_locklength = _filestream.Length;
                            //_filestream.Lock(0, _locklength);    // this should be unnecessary, but whatever.
                            _sleepTimer = Math.Max(_sleepTimer < _threshold ? _sleepTimer - _increment : _sleepTimer / 2, _fastest);

                            _reader = new StreamReader(_filestream);
                            _writer = new StreamWriter(_filestream);
                            _haveLock = true;
                            return _filestream;
                        }
                    }
                }
                catch (UnauthorizedAccessException) { }
                catch (FileLoadException) { }
                catch (IOException) { }
                catch (Exception e)
                {
                    Console.WriteLine("Error obtaining lock: " + e.Message);
                    Console.WriteLine(e.StackTrace);
                    throw;
                }

                // if we get here, it's because one of the lock-based exceptions was thrown. Reset clock and try again.
                //Console.WriteLine("File {0} is already locked. Sleeping and will try again.", _filename);
                System.Threading.Thread.Sleep(_rand.Next(_sleepTimer));
                _sleepTimer = (int)Math.Min(2 * _sleepTimer, _slowest);
            }
            Console.WriteLine("SharedFile is disposed. Cannot obtain another lock on disposed object. Returning null.");
            return null;
        }

        private static void CreateFileIfNotCreated(string filename)
        {
            Random rand = new Random(System.Guid.NewGuid().GetHashCode());
            for (int i = 0; !File.Exists(filename); i++)
            {
                try
                {
                    using (FileStream fs = File.Create(filename))
                    {
                        return;
                    }
                }
                catch (IOException e)
                {
                    if (i >= 20)
                        throw new IOException("Could not create SharedFile. Tried 20 times to no avail.", e);
                    else
                        System.Threading.Thread.Sleep(rand.Next(1000, 5000));
                }
            }
        }

        public void ReleaseLock()
        {
            lock (_lockObj)
            {
                if (_haveLock)
                {
                    _writer.Flush();

                    //_filestream.Dispose();
                    ReleaseLockInternal();

                    _filestream = null;
                    _writer = null;
                    _reader = null; ;
                    _haveLock = false;
                    //Console.WriteLine("Lock released.");
                }
                else
                {
                    //Console.WriteLine("Release lock called, but we don't have a lock.");
                }
            }
        }

        private void ReleaseLockInternal()
        {
            //_filestream.Unlock(0, _locklength);    // this should be unnecessary, but whatever.
            _filestream.Dispose();
            //Console.WriteLine("Releasing file lock. Does {0} still exist? {1}", _filename, File.Exists(_filename));
            //File.Move(_uniqFilename, _filename);
        }

        public string ReadAll()
        {
            lock (_lockObj)
            {
                Helper.CheckCondition(_haveLock, "You must obtain a lock before reading the file.");
                _filestream.Position = 0;
                string completeStream = _reader.ReadToEnd();
                return completeStream;
            }
        }

        public IEnumerable<string> ReadLines()
        {
            lock (_lockObj)
            {
                Helper.CheckCondition(_haveLock, "You must obtain a lock before reading the file.");
                _filestream.Position = 0;

                string line;
                while (null != (line = _reader.ReadLine()))
                {
                    yield return line;
                }
            }
        }

        //public List<string> ReadLines()
        //{
        //    lock (_lockObj)
        //    {
        //        Helper.CheckCondition(_haveLock, "You must obtain a lock before reading the file.");
        //        _filestream.Position = 0;
        //        List<string> lines = new List<string>();
        //        string line;
        //        while (null != (line = _reader.ReadLine()))
        //        {
        //            lines.Add(line);
        //        }
        //        return lines;
        //    }
        //}

        public void Clear()
        {
            lock (_lockObj)
            {
                Helper.CheckCondition(_haveLock, "You must obtain a lock before clearing the file.");
                _filestream.SetLength(0);
                _filestream.Position = 0;
            }
        }

        public void Write(object o)
        {
            Write(o.ToString());
        }

        public void Write(string formatString, params object[] args)
        {
            Write(string.Format(formatString, args));
        }

        public void WriteLine(object o)
        {
            WriteLine(o.ToString());
        }

        public void WriteLine(string formatString, params object[] args)
        {
            WriteLine(string.Format(formatString, args));
        }

        public void WriteLine(string s)
        {
            Write(s + "\n");
        }

        public void WriteAggregate(string key, object value)
        {
            WriteLine("AGGREGATE_{0}: {1}", key, value.ToString());
        }


        public void Write(string s)
        {
            //!!! THIS WAS COMMENTED OUT. I DON'T THINK IT SHOULD HAVE BEEN, IF YOU HAVE TROUBLE, COMMENT OUT THE LOCK
            lock (_lockObj)
            {
                Helper.CheckCondition(_haveLock, "You must obtain a lock before writing to the file.");
                _filestream.Position = _filestream.Length;
                _writer.Write(s);
                _writer.Flush();
            }
        }

        public List<T> ExtractAggregate<T>(string key)
        {
            var lines = ReadLines().ToList();
            Clear();
            List<T> values = new List<T>();
            string keyc = "AGGREGATE_" + key + ": ";

            foreach (string line in lines)
            {
                if (line.StartsWith(keyc))
                {
                    T value = MBT.Escience.Parse.Parser.Parse<T>(line.Substring(keyc.Length));
                    values.Add(value);
                }
                else
                {
                    WriteLine(line);
                }
            }
            return values;
        }

        /// <summary>
        /// Will try really hard to be the next one to get a lock on the file and delete it. If currently has the lock, it must release it, in which case someone else could grab it before
        /// we get the chance to delete it.
        /// </summary>
        public void Delete()
        {
            lock (_lockObj)
            {
                if (_haveLock)
                    ReleaseLock();
                while (true)
                {
                    try
                    {
                        File.Delete(_filename);
                        Dispose();
                        return;
                    }
                    catch (IOException) { }
                }
            }
        }

        public void Dispose()
        {
            lock (_lockObj)
            {
                if (!_disposed)
                {
                    _disposed = true;
                    ReleaseLock();
                }
            }
        }


    }
}
