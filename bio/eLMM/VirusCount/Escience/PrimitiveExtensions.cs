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
using Bio.Util;

namespace MBT.Escience
{
    public static class PrimitiveExtensions
    {
        public static bool Enforce(this bool value)
        {
            return value.Enforce("Value was expected to be true, but is false");
        }

        public static bool Enforce(this bool value, string errorMsg)
        {
            if (!value)
                throw new Exception(errorMsg);
            return value;
        }

        public static void Enforce(this bool condition, string messageToFormat, params object[] formatValues)
        {
            Helper.CheckCondition(condition, messageToFormat, formatValues);
        }

        /// <summary>
        /// Confirms that a condition is true. Raise an exception of type T if it is not.
        /// </summary>
        /// <param name="condition">The condition to check</param>
        /// <typeparam name="T">The type of exception that will be raised.</typeparam>
        public static void Enforce<T>(this bool condition) where T : Exception
        {
            Enforce<T>(condition, "A condition failed.");
        }

        /// <summary>
        /// Confirms that a condition is true. Raise an exception of type T if it is not.
        /// </summary>
        /// <remarks>
        /// Warning: The message with be evaluated even if the condition is true, so don't make it's calculation slow.
        ///           Avoid this with the "messageFunction" version.
        /// </remarks>
        /// <param name="condition">The condition to check</param>
        /// <param name="message">A message for the exception</param>
        /// <typeparam name="T">The type of exception that will be raised.</typeparam>
        public static void Enforce<T>(this bool condition, string message) where T : Exception
        {
            if (!condition)
            {
                Type t = typeof(T);
                System.Reflection.ConstructorInfo constructor = t.GetConstructor(new Type[] { typeof(string) });
                T exception = (T)constructor.Invoke(new object[] { message });
                throw exception;
            }
        }

        /// <summary>
        /// Confirms that a condition is true. Raise an exception if it is not.
        /// </summary>
        /// <remarks>
        /// Warning: The message with be evaluated even if the condition is true, so don't make it's calculation slow.
        ///           Avoid this with the "messageFunction" version.
        /// </remarks>
        /// <param name="condition">The condition to check</param>
        /// <param name="messageToFormat">A message for the exception</param>
        /// <param name="formatValues">Values for the exception's message.</param>
        /// <typeparam name="T">The type of exception that will be raised.</typeparam>
        public static void Enforce<T>(this bool condition, string messageToFormat, params object[] formatValues) where T : Exception
        {
            if (!condition)
            {
                Enforce<T>(condition, string.Format(messageToFormat, formatValues));
            }
        }
    }
}
