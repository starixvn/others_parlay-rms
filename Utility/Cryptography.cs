namespace ParlayRMS.Utility
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using Microsoft.VisualBasic;

    public static class Cryptography
    {
        private const string CfsKey = "&％#@?,:*!~^.";

        /// <summary>
        /// One-way encryption
        /// </summary>
        /// <param name="codeStr"></param>
        /// <returns></returns>
        public static string CfsEnCode(string codeStr)
        {
            int num = 30;
            int num2 = num - codeStr.Length;
            if (num2 > 1)
            {
                for (int m = 1; m <= num2; m++)
                {
                    codeStr = codeStr + Strings.Chr(0x15);
                }
            }

            double num3 = 1.0;
            for (int i = 1; i <= num; i++)
            {
                int num5 = num + (Strings.Asc(Strings.Mid(codeStr, i, 1)) * i);
                num3 *= num5;
            }

            codeStr = num3.ToString();
            string str = string.Empty;
            for (int j = 1; j <= codeStr.Length; j++)
            {
                str = str + CfsCode(Strings.Mid(codeStr, j, 3));
            }

            string str2 = string.Empty;
            for (int k = 20; k <= (str.Length - 0x12); k += 2)
            {
                str2 = str2 + Strings.Mid(str, k, 1);
            }

            return str2;
        }

        public static string GetDeCryptConnectionString(string connStr)
        {
            var decryptPwd = GetDecryptPwd(connStr, "=");
            var str = CfsDeCrypt(decryptPwd[1].Trim());
            return decryptPwd[0].Trim() + str + ";";
        }

        public static string CfsDeCrypt(string strText)
        {
            try
            {
                var rgbKey = new byte[] { 0x21, 0x63, 0x54, 0x5a, 0x91, 0xac, 110, 0xcc };
                var rgbIv = Encoding.UTF8.GetBytes(CfsKey);
                var provider = new DESCryptoServiceProvider();
                var buffer = Convert.FromBase64String(strText);
                var stream = new MemoryStream();
                var stream2 = new CryptoStream(stream, provider.CreateDecryptor(rgbKey, rgbIv), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                return Encoding.UTF8.GetString(stream.ToArray());
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        public static string CfsEnCrypt(string strText)
        {
            try
            {
                var rgbKey = new byte[] { 0x21, 0x63, 0x54, 0x5a, 0x91, 0xac, 110, 0xcc };
                byte[] rgbIv = Encoding.UTF8.GetBytes(CfsKey);
                var provider = new DESCryptoServiceProvider();
                byte[] bytes = Encoding.UTF8.GetBytes(strText);
                var stream = new MemoryStream();
                var stream2 = new CryptoStream(stream, provider.CreateEncryptor(rgbKey, rgbIv), CryptoStreamMode.Write);
                stream2.Write(bytes, 0, bytes.Length);
                stream2.FlushFinalBlock();
                return Convert.ToBase64String(stream.ToArray());
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        public static string RemoveSpecialChars(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            return str.Replace("\n", "\\n").Replace("\"", "\\\"").Replace("'", "\\'");
        }

        /// <summary>
        /// Generate a random string
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            return RandomString(length, false);
        }

        public static string RandomString(int length, bool noZeroO)
        {
            string chars;
            chars = noZeroO
                ? "qwertyuipasdfghjklzxcvbnm123456789"
                : "qwertyuiopasdfghjklzxcvbnm0123456789";

            string rndStr = string.Empty;
            Random rnd = new Random(DateTime.Now.Second + DateTime.Now.Millisecond);

            for (int i = 0; i < length; ++i)
            {
                rndStr += chars[rnd.Next() % chars.Length];
            }

            return rndStr;
        }

        public static string RandomNumberString(int length)
        {
            string chars = "0123456789";
            string rndStr = string.Empty;
            Random rnd = new Random();

            for (int i = 0; i < length; ++i)
            {
                rndStr += chars[rnd.Next() % chars.Length];
            }

            return rndStr;
        }

        private static string CfsCode(string word)
        {
            string number = string.Empty;
            for (int i = 1; i <= word.Length; i++)
            {
                number = number + Strings.Asc(Strings.Mid(word, i, 1));
            }

            return Conversion.Hex(number);
        }

        private static string[] GetDecryptPwd(string sourceString, string searchString)
        {
            int length = 1;
            string str = string.Empty;
            string str2 = string.Empty;
            string str3 = string.Empty;
            string str4;

            for (int i = sourceString.Length - 1; i >= 0; i--)
            {
                str = str + sourceString.Substring(i, 1);
            }

            for (int j = searchString.Length - 1; j >= 0; j--)
            {
                str2 = str2 + searchString.Substring(j, 1);
            }

            if (str.Substring(1, 2) != "==")
            {
                if (str.Substring(1, 1) != "=")
                {
                    for (int n = 1; n <= str.Length; n++)
                    {
                        if (str.Substring(n, 1) == "=")
                        {
                            break;
                        }

                        length++;
                    }

                    str4 = str.Substring(0, length);
                    for (int num9 = str4.Length; num9 > 1; num9--)
                    {
                        str3 = str3 + str4.Substring(num9 - 1, 1);
                    }

                    return new[] { sourceString.Substring(0, sourceString.Length - length), str3 };
                }

                length++;
                for (int m = 2; m <= str.Length; m++)
                {
                    if (str.Substring(m, 1) == "=")
                    {
                        break;
                    }

                    length++;
                }
            }
            else
            {
                length += 2;
                for (int num4 = 3; num4 <= str.Length; num4++)
                {
                    if (str.Substring(num4, 1) == "=")
                    {
                        break;
                    }

                    length++;
                }

                str4 = str.Substring(0, length);
                for (int num5 = str4.Length; num5 > 1; num5--)
                {
                    str3 = str3 + str4.Substring(num5 - 1, 1);
                }

                return new[] { sourceString.Substring(0, sourceString.Length - length), str3 };
            }

            str4 = str.Substring(0, length);
            for (int k = str4.Length; k > 1; k--)
            {
                str3 = str3 + str4.Substring(k - 1, 1);
            }

            return new[] { sourceString.Substring(0, sourceString.Length - length), str3 };
        }
    }
}