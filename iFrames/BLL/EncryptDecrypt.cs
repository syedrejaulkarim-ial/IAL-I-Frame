using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace iFrames.BLL
{
    public static class EncryptDecrypt
    {
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
        public static string ICRANumericEncode(long Value)
        {
            IList<short> Numlist = new List<short>();
            while (Value > 9)
            {

                Numlist.Add((short)(Value % 10));
                Value = Value / 10;
            }
            Numlist.Add((short)Value);
            var newList = Numlist.Select((i, v) => (char)((v % 2 == 0) ? (74 + i) : (74 - i))).ToArray<char>();
            return new string(newList);
        }
        public static long ICRANumericDecode(string Value)
        {
            var Num = Value.ToCharArray().Select((i, v) => ((short)((v % 2 == 0) ? (i - 74) : (74 - i))).
                ToString().ToCharArray().First()).Reverse().
                ToArray<char>();
            var Val = new string(Num);
            return Convert.ToInt64(Val);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value">Value should be 64 bit encode string</param>
        /// <returns></returns>
        public static string ICRABase64Decode(string Value)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(Value);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}