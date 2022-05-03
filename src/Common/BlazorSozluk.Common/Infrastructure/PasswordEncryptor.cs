using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Common.Infrastructure
{
    public class PasswordEncryptor
    {
        public static string Encrypt(string password)
        {
             var Md5=MD5.Create();
            byte[]inputeBytes=Encoding.ASCII.GetBytes(password);
            byte[]HashBytes=Md5.ComputeHash(inputeBytes);
            return Convert.ToHexString(HashBytes);
        }
    }
}
