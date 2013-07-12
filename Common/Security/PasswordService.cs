
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Avantech.Common.Security
{
    public static class PasswordService
    {
        // Define supported password characters.
        // You can add (or remove) characters to (from) this string.

        #region PasswordStrength enum

        public enum PasswordStrength
        {
            Low = 1,
            Medium = 2,
            Strong = 3,
            SuperStrong = 4
        }

        #endregion

        private const string PasswordChars = @"abcdefgijkmnopqrstwxyzABCDEFGHJKLMNPQRSTWXYZ23456789!@#$%^&*?_~";

        /// <summary>
        ///   Hashes a password string.
        /// </summary>
        /// <param name = "password">The password.</param>
        /// <param name = "salt">The salt.</param>
        /// <returns></returns>
        public static string HashPassword(string password, int salt)
        {
            var sha = new SHA256Managed();
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var saltBytes = BitConverter.GetBytes(salt);
            var combinedBytes = passwordBytes.Concat(saltBytes).ToArray();
            var passwordHash = sha.ComputeHash(combinedBytes);
            return Convert.ToBase64String(passwordHash);
        }

        /// <summary>
        /// Generates a salt value for use with hashing passwords.
        /// </summary>
        /// <returns></returns>
        public static int GeneratePasswordSalt()
        {
            var rnd = new Random();
            return rnd.Next();
        }

        /// <summary>
        /// Generates a password dependent upon the strength setting passed.
        /// </summary>
        /// <param name="passwordStrength">The required password strength.</param>
        /// <returns></returns>
        public static string GeneratePassword(PasswordStrength passwordStrength)
        {
            int passwordLength;
            switch (passwordStrength)
            {
                case PasswordStrength.Low:
                    passwordLength = 4;
                    break;
                case PasswordStrength.Medium:
                    passwordLength = 6;
                    break;
                case PasswordStrength.Strong:
                    passwordLength = 8;
                    break;
                case PasswordStrength.SuperStrong:
                    passwordLength = 10;
                    break;
                default:
                    passwordLength = 10;
                    break;
            }


            var random = new Random();

            var chars = new char[passwordLength];
            for (var i = 0; i < passwordLength; i++)
            {
                chars[i] = PasswordChars[random.Next(0, PasswordChars.Length)];
            }
            var password = new string(chars);

            if (GetPasswordStrength(password) == (int) passwordStrength)
                return password;

            return GeneratePassword(passwordStrength);
        }

        /// <summary>
        ///   Calculate the password strength
        /// </summary>
        /// <param name = "password">The password to check</param>
        /// <returns>An integer score value 0 - 4</returns>
        public static int GetPasswordStrength(string password)
        {
            var score = 0;

            // Low
            if (password.Length > 0)
                score = 1;

            // Medium
            if (password.Length > 4 && Regex.Match(password, @"([a-zA-Z])").Success && Regex.Match(password, @"([0-9])").Success)
                score = 2;

            // Strong
            if (password.Length > 7 && Regex.Match(password, @"([a-z].*[A-Z])|([A-Z].*[a-z])").Success && Regex.Match(password, @"([0-9])").Success)
                score = 3;

            // Super Strong
            if (password.Length > 9 && Regex.Match(password, @"([a-z].*[A-Z])|([A-Z].*[a-z])").Success && Regex.Match(password, @"([0-9])").Success && Regex.Match(password, @"([!,@,#,$,%,^,&,*,?,_,~])").Success)
                score = 4;

            return score;
        }

        /// <summary>
        /// Gets the default password.
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultPassword()
        {
            //TODO: Get value from settings
            return "Password123";
        }

        /// <summary>
        /// Gets the is required password change value for a user.
        /// </summary>
        /// <returns></returns>
        public static bool GetIsRequiredPasswordChange()
        {
            //TODO: Get value from settings
            return true;
        }

        /// <summary>
        /// Gets the default salt.
        /// </summary>
        /// <returns></returns>
        public static int GetDefaultSalt()
        {
            return int.MaxValue;
        }
    }

}