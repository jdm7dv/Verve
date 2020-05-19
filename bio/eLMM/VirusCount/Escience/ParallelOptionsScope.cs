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
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;

namespace MBT.Escience
{

    public sealed class ParallelOptionsScope : IDisposable
    {
#if !SILVERLIGHT
        /// <summary>
        /// A ParallelOptions instance in which MaxDegreeOfParallelism is 1.
        /// </summary>
        public readonly static ParallelOptions SingleThreadedOptions = new ParallelOptions { MaxDegreeOfParallelism = 1 };
        /// <summary>
        /// A ParallelOptions instance in which MaxDegreeOfParallelism is equal to the number of cores in the current environment.
        /// </summary>
        public readonly static ParallelOptions FullyParallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

        const string KEY_NAME = "ParallelOptionsScope";
        //private static ParallelOptions _default = new ParallelOptions() { MaxDegreeOfParallelism = -1 };
        private bool _disposed;

        //public static void Using(ParallelOptions options, Action body)
        //{
        //    if (options == null) throw new ArgumentNullException("options");
        //    if (body == null) throw new ArgumentNullException("body");
        //    using (new ParallelOptionsScope(options)) body();
        //}

        //public static void Suspend(Action body)
        //{
        //    if (Exists)
        //    {
        //        var stack = SuspendParallelOptionsScope();
        //        try
        //        {
        //            body();
        //        }
        //        finally
        //        {
        //            RestoreParallelOptionsScope(stack);
        //        }
        //    }
        //    else
        //    {
        //        body();
        //    }
        //}

        /// <summary>
        /// Creates a new ParallelOptionsScope using the specified options. Must be used in a using statement.
        /// </summary>
        public static ParallelOptionsScope Create(ParallelOptions options)
        {
            return new ParallelOptionsScope(options);
        }

        /// <summary>
        /// Creates a new ParallelOptionsScope that specified that a single thread should be used. Must be used in a using statement.
        /// </summary>
        public static ParallelOptionsScope CreateSingleThreaded()
        {
            return new ParallelOptionsScope(SingleThreadedOptions);
        }

        /// <summary>
        /// Creates a new ParallelOptionsScope that sets the number of threads to be equal to the number of cores in the current environment. Must be used in a using statement.
        /// </summary>
        public static ParallelOptionsScope CreateFullyParallel()
        {
            return new ParallelOptionsScope(FullyParallelOptions);
        }

        public static IDisposable Suspend()
        {
            return new SuspendScope();
        }


        private ParallelOptionsScope(ParallelOptions options)
        {
            if (options == null) throw new ArgumentNullException("options");
            var stack = CallContext.LogicalGetData(KEY_NAME) as StackNode;
            stack = new StackNode { Options = options, Next = stack };
            CallContext.LogicalSetData(KEY_NAME, stack);
        }

        public static bool Exists
        {
            get
            {
                var stack = CallContext.LogicalGetData(KEY_NAME) as StackNode;
                return (stack != null);
            }
        }

        public static ParallelOptions Current
        {
            get
            {
                var stack = CallContext.LogicalGetData(KEY_NAME) as StackNode;
                if (stack == null)
                {
                    throw new InvalidOperationException("This method must be called inside a using(ParallelOptionsScope) statement.");
                }
                return stack.Options;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                var stack = CallContext.LogicalGetData(KEY_NAME) as StackNode;
                if (stack != null) CallContext.LogicalSetData(KEY_NAME, stack.Next);
            }
        }

        private class StackNode
        {
            public ParallelOptions Options;
            public StackNode Next;
        }

        private class SuspendScope : IDisposable
        {
            private bool _disposed = false;
            private StackNode _suspendedStackNode;

            public SuspendScope()
            {
                _suspendedStackNode = SuspendParallelOptionsScope();
            }

            public void Dispose()
            {
                if (!_disposed)
                {
                    _disposed = true;
                    RestoreParallelOptionsScope(_suspendedStackNode);
                }
            }

            private static StackNode SuspendParallelOptionsScope()
            {
                var stack = CallContext.LogicalGetData(KEY_NAME) as StackNode;
                if (stack != null)
                {
                    CallContext.FreeNamedDataSlot(KEY_NAME);
                    if (CallContext.LogicalGetData(KEY_NAME) != null)
                    {
                        throw new NotImplementedException("Not working how I thought it would");
                    }
                }
                return stack;
            }

            private static void RestoreParallelOptionsScope(StackNode stack)
            {
                if (CallContext.LogicalGetData(KEY_NAME) != null)
                {
                    throw new InvalidOperationException("Cannot replace an existing ParallelOptionsScope stack. Remove the existing stack first with SuspendParallelOptionsScope()");
                }
                CallContext.LogicalSetData(KEY_NAME, stack);
            }
        }
#else
        public static IDisposable Create(ParallelOptions options)
        {
            return new ParallelOptionsScope();
        }

        /// <summary>
        /// Creates a new ParallelOptionsScope that specified that a single thread should be used. Must be used in a using statement.
        /// </summary>
        public static ParallelOptionsScope CreateSingleThreaded()
        {
            return new ParallelOptionsScope();
        }

        /// <summary>
        /// Creates a new ParallelOptionsScope that sets the number of threads to be equal to the number of cores in the current environment. Must be used in a using statement.
        /// </summary>
        public static ParallelOptionsScope CreateFullyParallel()
        {
            return new ParallelOptionsScope();
        }

        public static IDisposable Suspend()
        {
            return new ParallelOptionsScope();
        }
        public void Dispose()
        {
        }
#endif




    }



    public static class ParallelQueryExtensions
    {
#if !SILVERLIGHT
        public static ParallelQuery<TSource> WithParallelOptionsScope<TSource>(this ParallelQuery<TSource> source)
        {
            return source.WithDegreeOfParallelism(ParallelOptionsScope.Current.MaxDegreeOfParallelism);
        }
#else
        public static IEnumerable<TSource> WithParallelOptionsScope<TSource>(this IEnumerable<TSource> source)
        {
            return source;
        }
#endif
    }

}
