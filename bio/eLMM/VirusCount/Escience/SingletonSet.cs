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


using Bio.Util;
namespace MBT.Escience
{
    public class SingletonSet<T>
    {
        private SingletonSet()
        {
        }

        T element;
        string ErrorMessageFormatString;

        public bool IsEmpty { get; private set; }

        static public SingletonSet<T> GetInstance(string errorMessageFormatString)
        {
            SingletonSet<T> singletonSet = new SingletonSet<T>();
            singletonSet.IsEmpty = true;
            singletonSet.ErrorMessageFormatString = errorMessageFormatString;
            return singletonSet;
        }

        public void Add(T t)
        {
            if (IsEmpty)
            {
                element = t;
                IsEmpty = false;
            }
            else
            {
                Helper.CheckCondition(element.Equals(t), ErrorMessageFormatString, element, t);
            }
        }

        public T First()
        {
            Helper.CheckCondition(!IsEmpty, "Must have an elememnt");
            return element;
        }
    }
}
