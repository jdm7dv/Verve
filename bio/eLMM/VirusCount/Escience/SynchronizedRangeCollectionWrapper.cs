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
using System.Threading;
using Bio.Util;

namespace MBT.Escience
{
    public class SynchronizedRangeCollectionWrapper : IDisposable
    {
        readonly int _bufferSize = 100;
        SharedFile _sharedRangeFile;
        //readonly FileInfo _sharedRangeFileInfo;
        readonly RangeCollection _rangePendingCheckin;
        readonly object _lockObj = new object();
        readonly int _numberOfItemsToAdd;
        //readonly ManualResetEvent _timeToFlushWaitEvent;
        //readonly AutoResetEvent _flushCompleteWaitEvent;
        volatile bool _disposed = false;
        ManualResetEvent _workerThreadIsSetUp = new ManualResetEvent(false);	// use manual so that the gate is open forever with one set.

        bool _rangeCompletedByThisProcess = false;

        private SynchronizedRangeCollectionWrapper(string filename, int itemCount)
        {


            Console.WriteLine("CompletedRows filename: " + filename);

            //_sharedRangeFileInfo = new FileInfo(filename);
            _sharedRangeFile = new SharedFile(filename);
            _rangePendingCheckin = new RangeCollection();
            _numberOfItemsToAdd = itemCount;

            //_timeToFlushWaitEvent = new ManualResetEvent(false);
            //_flushCompleteWaitEvent = new AutoResetEvent(false);

            Thread thread = new Thread(Wait);
            thread.IsBackground = true; // if this is the last thread running, shut down.
            thread.Start();
        }

        public static SynchronizedRangeCollectionWrapper GetInstance(string filename, int itemCount)
        {
            return new SynchronizedRangeCollectionWrapper(filename, itemCount);

        }
        [Obsolete("uniqIdentifier and createFileForFirstUse are no longer used. You should switch to the new initializer.")]
        public static SynchronizedRangeCollectionWrapper GetInstance(string filename, string uniqIdentifier, int itemCount, bool createFileForFirstUse)
        {
            return GetInstance(filename, itemCount);
            //return new SynchronizedRangeCollectionWrapper(filename, uniqIdentifier, itemCount, createFileForFirstUse);
        }

        /// <summary>
        /// Make sure worker thread is not in middle of operation when we close. This causes a Flush(), and abort.
        /// </summary>
        public void Dispose()
        {
            Console.WriteLine("Disposing.");
            _disposed = true;
            BlockingFlush();

            lock (_lockObj)
            {
                _sharedRangeFile.Dispose();
            }

            Console.WriteLine("Disposed.");
        }

        ~SynchronizedRangeCollectionWrapper()
        {
            if (!_disposed)
            {
                Dispose();
            }
        }

        public void Add(RangeCollection rowCollection)
        {
            lock (_lockObj)
            {
                _rangePendingCheckin.AddRangeCollection(rowCollection);
                if (_rangePendingCheckin.Count() >= _bufferSize)
                {
                    NonBlockingFlush();
                }
            }
        }

        public void Add(int item)
        {
            lock (_lockObj)
            {
                _rangePendingCheckin.Add(item);
                if (_rangePendingCheckin.Count() >= _bufferSize)
                {
                    NonBlockingFlush();
                }
            }
        }

        public bool RangeCompletedByThisProcess()
        {
            Console.WriteLine("Checking if I'm the last.");
            BlockingFlush();
            Console.WriteLine("I know that I {0} the last one.", _rangeCompletedByThisProcess ? "am" : "am not");
            return _rangeCompletedByThisProcess;
        }

        public void BlockingFlushOld()
        {
            //int count;
            //Console.WriteLine("Blocking for flush.");
            //lock (_lockObj)
            //{
            //	count = _rangePendingCheckin.Count();
            //	if (_rangePendingCheckin.Count() == 0)	// nothing to do! don't bother fighting for file lock.
            //	{
            //		Console.WriteLine("Nothing to do in flush.");
            //		return;
            //	}
            //}
            //Console.WriteLine("Need to write {0} items. Calling non-blocking flush.", count);
            //NonBlockingFlush();
            //Console.WriteLine("Waiting for flush to finish.");
            //_flushCompleteWaitEvent.WaitOne();   // wait for the flush process to finish
            //Console.WriteLine("Flush complete.");
        }

        private void BlockingFlush()
        {
            _workerThreadIsSetUp.WaitOne(); // for really small jobs, there's a chance we could get here before the worker thread is even set up. better wait or we've got a race condition.
            lock (_lockObj)
            {
                Monitor.Pulse(_lockObj);
                Monitor.Wait(_lockObj);	 // release lock and wait for pulse.
            }
        }

        private void NonBlockingFlush()
        {
            lock (_lockObj)
            {
                Monitor.Pulse(_lockObj);
            }
        }


        private void Wait()
        {
            try
            {
                lock (_lockObj)
                {
                    _workerThreadIsSetUp.Set(); // once we're here, thread safety is ok. but make everyone wait till we get here.
                    while (true)
                    {
                        Monitor.Wait(_lockObj);			 // release lock and wait for pulse

                        if (_rangePendingCheckin.Count() > 0)
                        {
                            Console.WriteLine("Wait: {0} items to check in. Requesting file.", _rangePendingCheckin.Count());
                            Monitor.Exit(_lockObj);		 // release lock while we block for file lock
                            _sharedRangeFile.ObtainLock();  // block until we get a lock on the shared file or sharedFile is disposed (haveLock = false)
                            Monitor.Enter(_lockObj);		// reobtain lock to allow us to finish up the work
                            Console.WriteLine("Wait: Checking in {0} items covering {1}.", _rangePendingCheckin.Count(), _rangePendingCheckin);

                            // note: it is very important that _rangeComplete ONLY be modified under these conditions; namely,
                            // 1) you have the lock on the shared file. 
                            // 2) you have the lock on the _rangePendingCheckin. and 
                            // 3) you just added items to the file.
                            // Failure to do so may result in multiple jobs thinking they were the last to add items.
                            _rangeCompletedByThisProcess = AsynchronousAddRangeToStream(_rangePendingCheckin, _numberOfItemsToAdd);
                            _rangePendingCheckin.Clear();

                            _sharedRangeFile.ReleaseLock();

                        }

                        Monitor.Pulse(_lockObj);	// let anyone waiting know we're done.
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failure in SynchronizedRangeCollectionWrapper worker thread: {0}", e.Message);
                Console.WriteLine(e.StackTrace);
                throw;
            }
            finally
            {
                _sharedRangeFile.ReleaseLock(); // no matter what, we MUST release the file or the other processes may hang.
            }
        }

        private void WaitOld()
        {
            //try
            //{
            //	while (true)//!_disposed || _rangePendingCheckin.Count() > 0)
            //	{
            //		_timeToFlushWaitEvent.WaitOne();  // block until signaled to proceed
            //		//if (!_disposed)
            //			_timeToFlushWaitEvent.Reset();  // once we're disposed, leave the gate open until everything is dumped

            //		// if another set is given before we reset, we'll still write it below, if set is given after, 
            //		// we'll simply not wait next time around.
            //		try
            //		{
            //			bool haveLock = _sharedRangeFile.ObtainLock();  // block until we get a lock on the shared file or sharedFile is disposed (haveLock = false)
            //			lock (_lockObj)
            //			{
            //				//Console.WriteLine("Checking if anything to write to file. Range: {0}", _rangePendingCheckin);
            //				if (haveLock && _rangePendingCheckin.Count() > 0) // make sure we weren't disposed sometime between the while and the lock.
            //				{
            //					//Console.WriteLine("Writing range {0} to file", _rangePendingCheckin);
            //					// note: it is very important that _rangeComplete ONLY be modified under these conditions; namely,
            //					// 1) you have the lock on the shared file. 2) you have the lock on the _itemsToAdd. and 3) you just added items.
            //					// Failure to do so may result in multiple jobs thinking they were the last to add items.
            //					_rangeCompletedByThisProcess = AsynchronousAddRangeToStream(_rangePendingCheckin, _numberOfItemsToAdd);
            //					Console.WriteLine("RangeCompleted by this process ? {0}", _rangeCompletedByThisProcess);
            //					_rangePendingCheckin.Clear();
            //				}
            //			}
            //		}
            //		finally
            //		{
            //			_sharedRangeFile.ReleaseLock(); // no matter what, we MUST release the file or the other processes may hang.
            //		}
            //		_flushCompleteWaitEvent.Set();	// let anyone waiting know that we've completed the flush.
            //	}
            //}
            //catch (Exception e)
            //{
            //	Console.WriteLine("Failure in SynchronizedRangeCollectionWrapper worker thread: {0}", e.Message);
            //	Console.WriteLine(e.StackTrace);
            //	throw;
            //}
            //_flushCompleteWaitEvent.Set();	// let anyone waiting know that we've completed the flush.
            //Console.WriteLine("SynchronizedRangeCollectionWrapper Worker thread is complete.");
        }



        /// <summary>
        /// Only call this in a lock...
        /// </summary>
        private bool AsynchronousAddRangeToStream(RangeCollection itemsToAdd, int itemCount)
        {
            string completedRanges = null;
            RangeCollection completedRangeCollection = null;
            //StreamReader reader = null;
            //StreamWriter writer = null;
            try
            {
                //reader = new StreamReader(rangeFileStream);
                //completedRanges = reader.ReadToEnd();
                completedRanges = _sharedRangeFile.ReadAll().Trim();
                completedRangeCollection = completedRanges.Length == 0 ? new RangeCollection() : RangeCollection.Parse(completedRanges);
                Console.WriteLine("Range read from file: {0}", completedRangeCollection);

                completedRangeCollection.AddRangeCollection(itemsToAdd);

                //rangeFileStream.SetLength(0);
                //writer = new StreamWriter(rangeFileStream);
                //writer.Write(completedRangeCollection);
                //writer.Flush();

                _sharedRangeFile.Clear();
                _sharedRangeFile.Write(completedRangeCollection);

                Console.WriteLine("Range {0} complete to {1} items? {2}", completedRangeCollection, itemCount, completedRangeCollection.IsComplete(itemCount));
                return completedRangeCollection.IsComplete(itemCount);
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR: Trouble adding range.");
                Console.WriteLine("read from file: {0}", completedRanges);
                Console.WriteLine("new range: {0}", itemsToAdd);
                Console.WriteLine("combined: {0}", completedRangeCollection);

                throw;
            }
        }


    }
}
