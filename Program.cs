using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace zycrack
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\tZYCrack by WILL_V\n\thttps://github.com/WWILLV\n");
            Console.WriteLine("This crack file is for learning only, please do not distribute it.\n\n");
            string jifenNum;
            string years;
            string evalue;
            string logindata;
            Console.WriteLine("Current Login Data:");
            Console.WriteLine(ClassM.DecryptValue());
            Console.WriteLine("\n\n1-> Get current login data\n2-> Generate login data\n3-> Decrypt login data\n4-> Encrypt login data\n5-> Exit\n");
            while (true)
            {
                Console.Write("\nPlease input your choice: ");
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.WriteLine(ClassM.DecryptValue());
                        break;
                    case "2":
                        Console.Write("Please input the points: ");
                        jifenNum = Console.ReadLine();
                        Console.Write("Please enter an expiration time (year): ");
                        years = Console.ReadLine();
                        evalue = jifenNum + "|" + years + "-12-31 23:59:59|zhanghw1q2w";
                        logindata = ClassM.EncryptValue(evalue);
                        Console.WriteLine("The login data generated is: " + logindata);
                        Console.Write("Do you need to replace the existing login file? (y/n): ");
                        while (true)
                        {
                            switch (Console.ReadLine().ToLower())
                            {
                                case "y":
                                    ClassM.writestr(logindata);
                                    break;
                                case "n":
                                    break;
                                default:
                                    Console.Write("Do you need to replace the existing login file? (y/n)");
                                    continue;
                            }
                            break;
                        }
                        break;
                    case "3":
                        Console.WriteLine("Please enter encrypted login data:");
                        logindata = Console.ReadLine();
                        Console.WriteLine("Decrypt: ");
                        Console.WriteLine(ClassM.DecryptValue(logindata));
                        break;
                    case "4":
                        Console.WriteLine("Please input the login data:");
                        logindata = Console.ReadLine();
                        Console.WriteLine("Encrypt: ");
                        Console.WriteLine(ClassM.EncryptValue(logindata));
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Error! Please input your choice again.");
                        continue;
                }
            }
        }
    }
    class ClassM
    {
        private static string sKey;
        private static string sIV;
        private static SymmetricAlgorithm mCSP;
        static ClassM()
        {
            ClassM.sKey = "qJzGEh6hESZDVJeCnFPGuxzaiB7NLQM3";
            ClassM.sIV = "qcDY6X+aPLw=";
            ClassM.mCSP = new TripleDESCryptoServiceProvider();
        }
        public static string EncryptValue(string strencrypt)
        {
            string text = ClassM.EncryptString(strencrypt);
            string str = text.Substring(0, text.Length - 10);
            string str2 = text.Substring(text.Length - 10, 8);
            string text2 = str2 + str;
            //ClassM.writestr(text2);
            return text2;
        }
        public static string DecryptValue()
        {
            return DecryptValue(ClassM.readstr());
        }
        public static string DecryptValue(string text)
        {
            string result;
            try
            {
                //string text = ClassM.readstr();
                if (string.IsNullOrEmpty(text))
                {
                    result = "";
                }
                else
                {
                    string str = text.Substring(0, 8);
                    string str2 = text.Substring(8, text.Length - 8);
                    text = str2 + str + "==";
                    string text2 = ClassM.DecryptString(text);
                    result = text2;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = "";
            }
            return result;
        }
        private static string EncryptString(string Value)
        {
            ClassM.mCSP.Key = Convert.FromBase64String(ClassM.sKey);
            ClassM.mCSP.IV = Convert.FromBase64String(ClassM.sIV);
            ClassM.mCSP.Mode = CipherMode.ECB;
            ClassM.mCSP.Padding = PaddingMode.PKCS7;
            ICryptoTransform transform = ClassM.mCSP.CreateEncryptor(ClassM.mCSP.Key, ClassM.mCSP.IV);
            byte[] bytes = Encoding.UTF8.GetBytes(Value);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            cryptoStream.Close();
            return Convert.ToBase64String(memoryStream.ToArray());
        }
        private static string DecryptString(string Value)
        {
            try
            {
                ClassM.mCSP.Key = Convert.FromBase64String(ClassM.sKey);
                ClassM.mCSP.IV = Convert.FromBase64String(ClassM.sIV);
                ClassM.mCSP.Mode = CipherMode.ECB;
                ClassM.mCSP.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = ClassM.mCSP.CreateDecryptor(ClassM.mCSP.Key, ClassM.mCSP.IV);
                byte[] array = Convert.FromBase64String(Value);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
                cryptoStream.Write(array, 0, array.Length);
                cryptoStream.FlushFinalBlock();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return "";
        }
        public static void writestr(string data)
        {
            try
            {
                string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ZhiYunTranslator";
                DirectoryInfo directoryInfo = new DirectoryInfo(text);
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
                StreamWriter streamWriter = new StreamWriter(text + "\\info.txt", false);
                streamWriter.WriteLine(data);
                streamWriter.Flush();
                streamWriter.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private static string readstr()
        {
            string result = "";
            try
            {
                string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ZhiYunTranslator";
                DirectoryInfo directoryInfo = new DirectoryInfo(text);
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
                FileStream fileStream = new FileStream(text + "\\info.txt", FileMode.Open);
                StreamReader streamReader = new StreamReader(fileStream);
                result = streamReader.ReadToEnd();
                streamReader.Close();
                fileStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return result;
        }
    }
}
