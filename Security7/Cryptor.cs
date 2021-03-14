using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Security7
{
    public class Cryptor
    {
        static string IV = "qo1lc3sjd8zpt9cx";
        static string Key = "owejehavhelkjhfjsjghrydkg57k7jf3";
        TripleDESCryptoServiceProvider tripleDES;
        AesCryptoServiceProvider aes;
        MD5CryptoServiceProvider MD5Crypto;
        List<CustomFile> results;
        string hash = "f0xle@rn";
        public Cryptor()
        {
            tripleDES = new TripleDESCryptoServiceProvider();
            MD5Crypto = new MD5CryptoServiceProvider();
            results = new List<CustomFile>();
            aes = new AesCryptoServiceProvider();
            //tripleDES.Key = MD5Crypto.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            //tripleDES.Mode = CipherMode.ECB;
            //tripleDES.Padding = PaddingMode.PKCS7;

            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = ASCIIEncoding.ASCII.GetBytes(Key);
            aes.IV = ASCIIEncoding.ASCII.GetBytes(IV);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
        }

        //public string Encrypt(string file)
        //{
        //    var info = new FileInfo(file);
        //    string text = File.ReadAllText(file);
        //    var data = UTF8Encoding.UTF8.GetBytes(text);
        //    ICryptoTransform transform = tripleDES.CreateEncryptor();
        //    byte[] encryptedBytes = transform.TransformFinalBlock(data, 0, data.Length);

        //    string str = Convert.ToBase64String(encryptedBytes, 0, data.Length);
        //    results.Add(new CustomFile { 
        //        Name = Path.GetFileNameWithoutExtension(info.Name),
        //        Text = str, 
        //        Extension = info.Extension, 
        //        Type = TextType.Encrypted 
        //    });
        //    return str;
        //}

        //public string Decrypt(string file)
        //{
        //    var info = new FileInfo(file);
        //    string text = File.ReadAllText(file);
        //    var data = Convert.FromBase64String(text);
        //    ICryptoTransform transform = tripleDES.CreateDecryptor();
        //    byte[] encryptedBytes = transform.TransformFinalBlock(data, 0, data.Length);

        //    string str = UTF8Encoding.UTF8.GetString(encryptedBytes);
        //    results.Add(new CustomFile
        //    {
        //        Name = Path.GetFileNameWithoutExtension(info.Name),
        //        Text = str,
        //        Extension = info.Extension,
        //        Type = TextType.Crear
        //    });
        //    return str;
        //}

        public string Encrypt(string file)
        {
            var info = new FileInfo(file);
            string text = TextReader.getText(file);
            ICryptoTransform transform = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] encryptedBytes = transform.TransformFinalBlock(ASCIIEncoding.ASCII.GetBytes(text), 0, text.Length);

            string str = Convert.ToBase64String(encryptedBytes);
            results.Add(new CustomFile { Name = Path.GetFileNameWithoutExtension(info.Name), Text = str, Extension = info.Extension, Type = TextType.Encrypted });
            return str;
        }

        public string Decrypt(string file)
        {
            var info = new FileInfo(file);
            string text = TextReader.getText(file);
            ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] encryptedBytes = Convert.FromBase64String(text);
            byte[] decryptedBytes = transform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            string str = ASCIIEncoding.ASCII.GetString(decryptedBytes);
            results.Add(new CustomFile { Name = Path.GetFileNameWithoutExtension(info.Name), Text = str, Extension = info.Extension, Type = TextType.Crear });
            return str;
        }

        public CustomFile[] getResult()
        {
            var res = results.ToArray();
            results.Clear();
            return res;
        }
    }
}
