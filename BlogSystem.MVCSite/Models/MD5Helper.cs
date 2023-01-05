using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BlogSystem.MVCSite.Models
{
    public class MD5Helper
    {
        /// <summary>
        /// 密码加密（MD5)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5String(string str)
        {

            StringBuilder sb = new StringBuilder();
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {

                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(str));


                int length = data.Length;
                for (int i = 0; i < length; i++)
                    sb.Append(data[i].ToString("X2"));

            }
            return sb.ToString();
        }
    }
}