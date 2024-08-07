﻿using System.Security.Cryptography;
using System.Text;

namespace csSymmetricEncryption;

internal class Program
{
    static void Main(string[] args)
    {
        SymmetricEncryption JimBobConversion = new SymmetricEncryption();
        Console.WriteLine($"此次使用的加解密金鑰 : {JimBobConversion.Key}");
        string plainText = "Hello, World!";
        Console.WriteLine($"Jim 準備要送出的未加密明碼文字 : {plainText}");
        string cipherText = JimBobConversion.Encrypt(plainText);
        Console.WriteLine($"Jim 加密後的密文文字 : {cipherText}");
        string decryptedText = JimBobConversion.Decrypt(cipherText);
        Console.WriteLine($"Bob 解密後的明碼文字 : {decryptedText}");
        Console.WriteLine(); Console.WriteLine();

        plainText = "What happened to you?  你怎麼了? 123";
        Console.WriteLine($"Jim 準備要送出的未加密明碼文字 : {plainText}");
        cipherText = JimBobConversion.Encrypt(plainText);
        Console.WriteLine($"Jim 加密後的密文文字 : {cipherText}");
        decryptedText = JimBobConversion.Decrypt(cipherText);
        Console.WriteLine($"Helen 解密後的明碼文字 : {decryptedText}");
        Console.WriteLine(); Console.WriteLine();

        SymmetricEncryption JimBobNewConversion = 
            new SymmetricEncryption("這裡指定需要使用的加解密金鑰 Key。代表抽象基底類別，進階加密標準 (AES) 的所有實作都必須從它繼承。");
        Console.WriteLine($"此次使用的加解密金鑰 : {JimBobNewConversion.Key}");
        plainText = "Hello, World!";
        Console.WriteLine($"Jim 準備要送出的未加密明碼文字 : {plainText}");
        cipherText = JimBobNewConversion.Encrypt(plainText);
        Console.WriteLine($"Jim 加密後的密文文字 : {cipherText}");
        decryptedText = JimBobNewConversion.Decrypt(cipherText);
        Console.WriteLine($"Bob 解密後的明碼文字 : {decryptedText}");
        Console.WriteLine(); Console.WriteLine();

    }
}

class SymmetricEncryption
{
    public SymmetricEncryption()
    {
        using (Aes aesAlgorithm = Aes.Create())
        {
            aesAlgorithm.KeySize = 256;
            aesAlgorithm.GenerateKey();
            this.Key = Convert.ToBase64String(aesAlgorithm.Key);
        }
    }

    public SymmetricEncryption(string key)
    {
        byte[] sourceBytes = Encoding.UTF8.GetBytes(key).ToArray().Take(32).ToArray();
        this.Key = Convert.ToBase64String(sourceBytes);
    }

    public string Key { get; set; } = "";

    public string Encrypt(string plainText)
    {
        byte[] iv; // Initialization Vector 這是一個隨機的數據，用於加密和解密
        using (Aes aes = Aes.Create())
        {
            // the AES algorithm supports 3 different key sizes:
            // 128-bit key, 192-bit key, and 256-bit key
            aes.Key = Convert.FromBase64String(Key);
            aes.GenerateIV();
            iv = aes.IV; // Initialization Vector 這是一個隨機的數據，用於加密和解密

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream =
                    new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }
                }

                byte[] encryptedBytes = memoryStream.ToArray();
                byte[] result = new byte[iv.Length + encryptedBytes.Length];
                Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                Buffer.BlockCopy(encryptedBytes, 0, result, iv.Length, encryptedBytes.Length);

                return Convert.ToBase64String(result);
            }
        }
    }

    public string Decrypt(string cipherText)
    {
        byte[] fullCipher = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Convert.FromBase64String(Key);

            byte[] iv = new byte[aes.IV.Length];
            byte[] cipher = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            aes.IV = iv;

            using (MemoryStream memoryStream = new MemoryStream(cipher))
            {
                using (CryptoStream cryptoStream =
                    new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader(cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }
}