using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RaptorTCP3.Methods.Utilities
{
    class Utilities
    {

        /// <summary>
        /// Converts A byte array to a string
        /// </summary>
        /// <param name="data">
        /// Byte[]: The Byte Array to convert
        /// </param>
        /// <returns>
        /// String: The results of the conversion
        /// </returns>
        internal string GetString(byte[] data)
        {
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        /// <summary>
        /// Converts a string to a Byte Array
        /// </summary>
        /// <param name="data">
        /// String: The text to convert
        /// </param>
        /// <returns>
        /// Byte[]: The result of the conversion
        /// </returns>
        internal byte[] GetBytes(string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }

        /// <summary>
        /// The SHA512 Method for hashing passwords before they are stored in the Database
        /// </summary>
        /// <param name="Password">
        /// string: The string representation of the password
        /// </param>
        /// <returns>
        /// String: A Hexadecimal string of 128 characters which represent the password.
        /// </returns>
        /// <remarks>
        /// The Client should send a Hashed Password instead of a plain text password.
        /// </remarks>
        internal string HashPassword(string Password)
        {
            using (SHA512 shaM = new SHA512Managed())
            {
                byte[] hash = shaM.ComputeHash(GetBytes(Password));
                return GetHex(hash);
            }

        }

        /// <summary>
        /// Converts a Byte Array to a Hexadecimal string
        /// </summary>
        /// <param name="hash">
        /// Byte[]: The byte array of the password hash to convert to a Hexadecimal string
        /// </param>
        /// <returns>
        /// String: The Hexadecimal string that results from the conversion. 128 characters long.
        /// </returns>
        private string GetHex(byte[] hash)
        {
            string hex = BitConverter.ToString(hash);
            return hex.Replace("-", "");
        }

        internal  string DecodeUrlString(string url)
        {
            string newUrl;
            while ((newUrl = Uri.UnescapeDataString(url)) != url)
                url = newUrl;
            return newUrl;
        }
    }
}
