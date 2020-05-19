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

//using System.Collections.Generic;
//using System.Diagnostics;

//namespace MBT.Escience
//{
//    /// <summary>
//    /// Assigns serial number of objects
//    /// </summary>
//    public class SerialNumbers<T>
//    {
//        private SerialNumbers()
//        {
//        }

//        static public SerialNumbers<T> GetInstance()
//        {
//            SerialNumbers<T> serialNumbers = new SerialNumbers<T>();
//            serialNumbers.ItemToSerialNumber = new Dictionary<T, int>();
//            serialNumbers.ItemList = new List<T>();
//            return serialNumbers;
//        }

//        static public SerialNumbers<T> GetInstance(IEnumerable<T> list)
//        {
//            SerialNumbers<T> serialNumbers = GetInstance();
//            foreach (T item in list)
//            {
//                serialNumbers.GetNewOrOld(item);
//            }
//            return serialNumbers;
//        }


//        public Dictionary<T, int> ItemToSerialNumber;
//        public List<T> ItemList;

//        public int GetNewOrOld(T item)
//        {
//            if (!ItemToSerialNumber.ContainsKey(item))
//            {
//                Debug.Assert(ItemToSerialNumber.Count == ItemList.Count); // real assert
//                int serialNumber = ItemToSerialNumber.Count;
//                Debug.Assert(serialNumber == ItemList.Count); // real assert
//                ItemToSerialNumber.Add(item, serialNumber);
//                ItemList.Add(item);
//                return serialNumber;
//            }
//            else
//            {
//                int serialNumber = ItemToSerialNumber[item];
//                return serialNumber;
//            }
//        }

//        public int GetNew(T item)
//        {
//            Helper.CheckCondition(!ItemToSerialNumber.ContainsKey(item), "item seen more than once. " + item.ToString());
//            Debug.Assert(ItemToSerialNumber.Count == ItemList.Count); // real assert
//            int serialNumber = ItemToSerialNumber.Count;
//            Debug.Assert(serialNumber == ItemList.Count); // real assert
//            ItemToSerialNumber.Add(item, serialNumber);
//            ItemList.Add(item);
//            return serialNumber;
//        }


//        public int GetOld(T item)
//        {
//            Helper.CheckCondition(ItemToSerialNumber.ContainsKey(item), "Expected to have seen " + item + " before.");
//            return ItemToSerialNumber[item];
//        }


//        public int Last
//        {
//            get
//            {
//                return ItemList.Count - 1;
//            }
//        }

//        public bool TryGetOld(T item, out int serialNumber)
//        {
//            return ItemToSerialNumber.TryGetValue(item, out serialNumber);
//        }

//        public int Count
//        {
//            get
//            {
//                return ItemToSerialNumber.Count;
//            }
//        }

//        public T GetItem(int serialNumber)
//        {
//            return ItemList[serialNumber];
//        }



//        public static SerialNumbers<string> ReadStringFeaturesFromFile(string featureFileName)
//        {
//            SerialNumbers<string> featureSerialNumbers = new SerialNumbers<string>();
//            foreach (string feature in Bio.Util.FileUtils.ReadEachLine(featureFileName))
//            {
//                featureSerialNumbers.GetNew(feature);
//            }
//            return featureSerialNumbers;
//        }

//        /// <summary>
//        /// Write the items in order to a file
//        /// </summary>
//        public void Save(string fileName)
//        {
//            SpecialFunctions.WriteEachLine(ItemList, fileName);
//        }

//    }
//}
