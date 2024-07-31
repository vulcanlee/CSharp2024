using System.Security.Cryptography;
using System.Text;

namespace csAsymmetricEncryption;

internal class Program
{
    static void Main(string[] args)
    {
        RsaTool rsaTool = new();
        Console.WriteLine("Generating keys for Jim...");
        RSA jimRsa = rsaTool.GenerateKeys();
        string jimPublicKey = rsaTool.GetPublicKey(jimRsa);
        string jimPrivateKey = rsaTool.GetPrivateKey(jimRsa);
        Console.WriteLine($"Jim Public Key : {jimPublicKey}");
        Console.WriteLine($"");
        Console.WriteLine($"Jim Private Key : {jimPrivateKey}");
        Console.WriteLine($""); Console.WriteLine($"");

        Console.WriteLine("Generating keys for Bob...");
        RSA BobRsa = rsaTool.GenerateKeys();
        string BobPublicKey = rsaTool.GetPublicKey(BobRsa);
        string BobPrivateKey = rsaTool.GetPrivateKey(BobRsa);
        Console.WriteLine($"Bob Public Key : {BobPublicKey}");
        Console.WriteLine($"");
        Console.WriteLine($"Bob Private Key : {BobPrivateKey}");
        Console.WriteLine($""); Console.WriteLine($"");



        string plainText = "Hello, World!";
        Console.WriteLine($"Jim 準備要送出的未加密明碼文字 : {plainText}");
        string encryptedForBob = rsaTool.Encrypt(BobPublicKey, plainText);
        Console.WriteLine($"Jim 使用 Bob 公開金鑰 加密後的密文文字 : {encryptedForBob}");

        string decryptedByBob = rsaTool.Decrypt(BobPrivateKey, encryptedForBob);
        Console.WriteLine($"Bob 使用自己私鑰 進行解密後的明碼文字 : {decryptedByBob}");
        Console.WriteLine($""); Console.WriteLine($"");



        plainText = "Hello, World!";
        Console.WriteLine($"Bob 準備要送出的未加密明碼文字 : {plainText}");
        string encryptedForJim = rsaTool.Encrypt(jimPublicKey, plainText);
        Console.WriteLine($"Bob 使用 Jim 公開金鑰 加密後的密文文字 : {encryptedForJim}");

        string decryptedByJim = rsaTool.Decrypt(jimPrivateKey, encryptedForJim);
        Console.WriteLine($"Jim 使用自己私鑰 進行解密後的明碼文字 : {decryptedByJim}");
        Console.WriteLine($""); Console.WriteLine($"");



        plainText = "What happened to you?  你怎麼了? 123!";
        Console.WriteLine($"Bob 準備要送出的未加密明碼文字 : {plainText}");
        encryptedForJim = rsaTool.Encrypt(jimPublicKey, plainText);
        Console.WriteLine($"Bob 使用 Jim 公開金鑰 加密後的密文文字 : {encryptedForJim}");

        decryptedByJim = rsaTool.Decrypt(jimPrivateKey, encryptedForJim);
        Console.WriteLine($"Jim 使用自己私鑰 進行解密後的明碼文字 : {decryptedByJim}");
        Console.WriteLine($""); Console.WriteLine($"");
    }
}

public class RsaTool
{
    public RSA GenerateKeys()
    {
        RSA rsa = RSA.Create(2048); // 2048 位元的密鑰長度
        return rsa;
    }

    public string GetPublicKey(RSA rsa)
    {
        return Convert.ToBase64String(rsa.ExportRSAPublicKey());
    }

    public string GetPrivateKey(RSA rsa)
    {
        return Convert.ToBase64String(rsa.ExportRSAPrivateKey());
    }

    public string Encrypt(string publicKey, string plainText)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = rsa.Encrypt(plainBytes, RSAEncryptionPadding.OaepSHA256);
            return Convert.ToBase64String(encryptedBytes);
        }
    }

    public string Decrypt(string privateKey, string cipherText)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
            byte[] encryptedBytes = Convert.FromBase64String(cipherText);
            byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.OaepSHA256);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }

}
