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
using System.Timers;
using Bio.Util;

namespace MBT.Escience
{
    public class FunctionTimeout : IDisposable
    {
        readonly object _lockObj = new object();
        readonly bool _printErrors;
        volatile bool _disposed = false;
        volatile bool _executing = false;
        volatile bool _finishedSuccessfully;
        ManualResetEvent _workerThreadIsSetUp = new ManualResetEvent(false);	// use manual so that the gate is open forever with one set.
        Action _functionToExecute;
        System.Timers.Timer _timer;
        Thread _workerThread;

        public FunctionTimeout(bool printErrors)
        {
            _printErrors = printErrors;
            InitializerWorker();
            _timer = new System.Timers.Timer();
            _timer.Elapsed += KillWorker;
            _timer.AutoReset = false;
        }

        private void InitializerWorker()
        {
            _workerThread = new Thread(Wait);
            _workerThread.IsBackground = true; // if this is the last thread running, shut down.
            _workerThread.Start();
        }

        /// <summary>
        /// Make sure worker thread is not in middle of operation when we close. This causes a Flush(), and abort.
        /// </summary>
        public void Dispose()
        {
            lock (_lockObj)
            {
                _timer.Dispose();
                _disposed = true;
                if (_executing)
                {
                    _workerThread.Abort();
                }
                Monitor.PulseAll(_lockObj);
            }
        }

        ~FunctionTimeout()
        {
            if (!_disposed)
            {
                Dispose();
            }
        }

        public bool TryExecuteFunction(Action FunctionToExecute, TimeSpan timeTillTimeout)
        {
            _workerThreadIsSetUp.WaitOne(); // wait for the worker to be ready.
            lock (_lockObj)
            {
                Helper.CheckCondition(!_executing, "Already executing. Cannot queue another function.");
                _functionToExecute = FunctionToExecute;
                _timer.Interval = timeTillTimeout.TotalMilliseconds;
                _timer.Enabled = true;
                Monitor.Pulse(_lockObj);
                Monitor.Wait(_lockObj);	 // release lock and wait for pulse.
                return _finishedSuccessfully;
            }
        }

        private void Wait()
        {
            try
            {
                lock (_lockObj)
                {
                    _workerThreadIsSetUp.Set(); // once we're here, thread safety is ok. but make everyone wait till we get here.
                    while (!_disposed)
                    {
                        Monitor.Wait(_lockObj);			 // release lock and wait for pulse
                        if (!_disposed)
                        {
                            _finishedSuccessfully = false;
                            _executing = true;
                            Monitor.Exit(_lockObj);		 // can't be in lock when function is executing

                            _functionToExecute();           // !!! not sure if this should be locked or not. For now, let's be safe and keep locked.

                            Monitor.Enter(_lockObj);		// reobtain lock to allow us to finish up the work
                            _executing = false;
                            _finishedSuccessfully = true;
                            _timer.Enabled = false;
                            Monitor.Pulse(_lockObj);	// let anyone waiting know we're done.
                        }
                    }
                }
            }
            catch (SynchronizationLockException)
            {
                PrintError("Function failed to finish but successfully aborted.");
            }
            catch (Exception e)
            {
                PrintError(string.Format("Failure in FunctionTimeout: {0}", e.Message));
                PrintError(e.StackTrace);
            }
            finally
            {
                lock (_lockObj)
                {
                    _executing = false;
                    Monitor.PulseAll(_lockObj); // let all waiting threads (timer and the main thread) know the abortion is complete
                }
            }
        }

        private void PrintError(string message)
        {
            if (_printErrors)
                Console.WriteLine(message);
        }

        private void KillWorker(object source, ElapsedEventArgs e)
        {
            lock (_lockObj)
            {
                if (_executing)
                {
                    _workerThreadIsSetUp.Reset();   // we're breaking the worker thread here, so we'll have to reset this. Not sure if this is generally thread safe, but will work with 1 thread that can call the Execute function.
                    _workerThread.Abort();
                    Monitor.Wait(_lockObj);
                    InitializerWorker();
                }
            }
        }
    }
}
