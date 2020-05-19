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
using System.Security.Cryptography;
using System.Text;

namespace MBT.Escience
{
    public class CryptoProvider
    {
        RC2CryptoServiceProvider _rc2CSP;
        ICryptoTransform _encryptor;
        ICryptoTransform _decryptor;


        private CryptoProvider(int seed)
        {
            _rc2CSP = new RC2CryptoServiceProvider();

            // Get the key and IV.
            Random rand = new Random(seed);
            byte[] key = new byte[_rc2CSP.Key.Length];
            rand.NextBytes(key);

            byte[] iv = new byte[_rc2CSP.IV.Length];
            rand.NextBytes(iv);

            _rc2CSP.Key = key;
            _rc2CSP.IV = iv;
        }

        public static CryptoProvider GetInstance()
        {
            return GetInstance(0);
        }

        public static CryptoProvider GetInstance(int key)
        {
            return new CryptoProvider(key);
        }


        private ICryptoTransform Encryptor
        {
            get
            {
                if (_encryptor == null)
                {
                    _encryptor = _rc2CSP.CreateEncryptor();
                }
                return _encryptor;
            }
        }

        private ICryptoTransform Decryptor
        {
            get
            {
                if (_decryptor == null)
                {
                    _decryptor = _rc2CSP.CreateDecryptor();
                }
                return _decryptor;
            }
        }

        public byte[] Encrypt(byte[] original)
        {
            MemoryStream msEncrypt = new MemoryStream();
            CryptoStream csEncrypt = new CryptoStream(msEncrypt, Encryptor, CryptoStreamMode.Write);

            csEncrypt.Write(original, 0, original.Length);
            csEncrypt.FlushFinalBlock();

            // Get the encrypted array of bytes.
            byte[] encrypted = msEncrypt.ToArray();
            return encrypted;
        }

        public string Encrypt(string original)
        {
            byte[] toEncrypt = Encoding.ASCII.GetBytes(original);
            byte[] encrypted = Encrypt(toEncrypt);
            string encryptedString = EncodeBytesAsString(encrypted);

            return encryptedString;
        }

        public byte[] Decrypt(byte[] encrypted)
        {
            MemoryStream msDecrypt = new MemoryStream(encrypted);
            CryptoStream csDecrypt = new CryptoStream(msDecrypt, Decryptor, CryptoStreamMode.Read);

            byte[] decrypted = new byte[encrypted.Length];
            csDecrypt.Read(decrypted, 0, encrypted.Length);

            return decrypted;
        }

        public string Decrypt(string encrypted)
        {
            try
            {
                byte[] encryptedBytes = DecodeBytesFromString(encrypted);
                byte[] decryptedBytes = Decrypt(encryptedBytes);
                string decrypted = Encoding.ASCII.GetString(decryptedBytes);
                return decrypted;
            }
            catch (CryptographicException e)
            {
                throw new CryptographicException("Could not decrypt " + encrypted, e);
            }
        }

        private static byte[] DecodeBytesFromString(string s)
        {
            List<byte> result = new List<byte>(s.Length * 2);
            for (int i = 0; i < s.Length; )
            {
                int b1 = (((int)s[i++]) - 65) << 4;
                int b2 = (((int)s[i++]) - 65);
                byte b = (byte)(b1 | b2);
                result.Add(b);
            }
            return result.ToArray();
        }

        private static string EncodeBytesAsString(byte[] barr)
        {
            StringBuilder sb = new StringBuilder(barr.Length);
            foreach (byte b in barr)
            {
                byte b1 = (byte)(b >> 4);
                byte b2 = (byte)(b & 15);
                sb.Append((char)(b1 + 65));
                sb.Append((char)(b2 + 65));
            }
            return sb.ToString();
        }
    }
}
