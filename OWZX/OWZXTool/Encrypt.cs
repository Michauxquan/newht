using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OWZXTool
{
    public class Encrypt
    {
        /// <summary>
        ///  通过用户名加密密码
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static string GetEncryptPwd(string pwd, string loginname)
        {
            var code = "Sj2yF98jUhg8874G";
            for (int i = 0; i < code.Length; i++)
            {
                if (i % 3 == 1)
                {
                    pwd = code[i] + pwd;
                }
            }
            for (int i = 0; i < code.Length; i++)
            {
                if (i % 2 == 0)
                {
                    pwd += code[i];
                }
            }
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "MD5");//System.Web.Configuration.FormsAuthPasswordFormat;
        }
        /// <summary>
        /// MD5散列
        /// </summary>
        public static string MD5(string inputStr)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashByte = md5.ComputeHash(Encoding.UTF8.GetBytes(inputStr));
            StringBuilder sb = new StringBuilder();
            foreach (byte item in hashByte)
                sb.Append(item.ToString("x").PadLeft(2, '0'));
            return sb.ToString();
        }
        private static Random _random = new Random();//随机发生器
        private static char[] _randomlibrary = (new StringBuilder("123456789abcdefghjkmnpqrstuvwxy")).ToString().ToCharArray();//随机库
         
        /// <summary>
        /// 创建随机值
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="onlyNumber">是否只包含数字</param>
        /// <returns>随机值</returns>
        public static string CreateRandomValue(int length, bool onlyNumber)
        {
            int index;
            StringBuilder randomValue = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                if (onlyNumber)
                    index = _random.Next(0, 9);
                else
                    index = _random.Next(0, _randomlibrary.Length);

                randomValue.Append(_randomlibrary[index]);
            }

            return randomValue.ToString();
        }

        /// <summary>
        /// 创建随机对
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="onlyNumber">是否只包含数字</param>
        /// <param name="randomKey">随机键</param>
        /// <param name="randomValue">随机值</param>
        public static void CreateRandomPair(int length, bool onlyNumber, out string randomKey, out string randomValue)
        {
            StringBuilder randomKeySB = new StringBuilder();
            StringBuilder randomValueSB = new StringBuilder();

            int index1;
            int index2;
            for (int i = 0; i < length; i++)
            {
                if (onlyNumber)
                {
                    index1 = _random.Next(0, 10);
                    index2 = _random.Next(0, 10);
                }
                else
                {
                    index1 = _random.Next(0, _randomlibrary.Length);
                    index2 = _random.Next(0, _randomlibrary.Length);
                }

                randomKeySB.Append(_randomlibrary[index1]);
                randomValueSB.Append(_randomlibrary[index2]);
            }
            randomKey = randomKeySB.ToString();
            randomValue = randomValueSB.ToString();
        }
    }
}
