using iFrames.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace iFrames
{
    public static class Utilities
    {
        //public static string XIRR(System.Object array1_comma_sep, System.Object array2_comma_sep)
        //{
        public static double XIRR(double[] ValueXIRR, DateTime[] DtXIRR)
        {
            return System.Numeric.Financial.XIrr(ValueXIRR.Reverse(), DtXIRR.Reverse());
        }

        private static string sKey = "~!@#$%^&";//"!#$a54?3"

        public static string DESEnCode(string pToEncrypt)
        {

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.GetEncoding("UTF-8").GetBytes(pToEncrypt);

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();

        }

        public static string DESDeCode(string pToDecrypt)
        {

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x != inputByteArray.Length; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());

        }
    }

    public sealed class CachingRepo
    {
        // The Singleton's constructor should always be private to prevent
        // direct construction calls with the `new` operator.
        private CachingRepo() { }

        public MemoryCacheRepository MemoryCacheRepo { get; private set; }

        // The Singleton's instance is stored in a static field. There there are
        // multiple ways to initialize this field, all of them have various pros
        // and cons. In this example we'll show the simplest of these ways,
        // which, however, doesn't work really well in multithreaded program.
        private static CachingRepo _instance;

        // This is the static method that controls the access to the singleton
        // instance. On the first run, it creates a singleton object and places
        // it into the static field. On subsequent runs, it returns the client
        // existing object stored in the static field.
        public static CachingRepo GetCacheRepository()
        {
            if (_instance == null)
            {
                _instance = new CachingRepo();
                _instance.someBusinessLogic();
            }
            return _instance;
        }

        // Finally, any singleton should define some business logic, which can
        // be executed on its instance.
        public void someBusinessLogic()
        {
            MemoryCacheRepo = new MemoryCacheRepository() { CacheDefaultTimeOut = new TimeSpan(3, 1, 1) };
        }
    }
}