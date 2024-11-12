using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace iVeew.Core.Helpers
{
    public class PasswordHelper
    {
        private PasswordHelper()
        {

        }
        public  static string GenerateSalt()
        {
            byte[] buf = new byte[16];
            (new RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
        }
        public  static string EncodePassword(string pass, int passwordFormat, string salt)
        {
            if (passwordFormat == 0) // MembershipPasswordFormat.Clear
                return pass;

            byte[] bIn = Encoding.Unicode.GetBytes(pass);
            byte[] bSalt = Convert.FromBase64String(salt);
            
            byte[] bRet = null;

            bRet = GenerateSaltedHash(bIn, bSalt);
           
            return Convert.ToBase64String(bRet);
        }

        public static string GeneratePassword(int passwordLength)
        {
            throw new Exception("Require implementation");
            //string password = Membership.GeneratePassword(passwordLength, 1);
            //password = Regex.Replace(password, @"[^a-zA-Z0-9]", m => new Random().Next(9).ToString());
            //return password;
        }

        public static string GenerateNumericPassword(int passwordLength)
        {
            //string password = Membership.GeneratePassword(passwordLength, 1);
            var chars = "0123456789";
            var random = new Random();
            string password = new string(
                Enumerable.Repeat(chars, passwordLength)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return password;
        }

        static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

    }
}