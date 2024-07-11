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

        Console.WriteLine("Generating keys for Tom...");
        RSA tomRsa = rsaTool.GenerateKeys();
        string tomPublicKey = rsaTool.GetPublicKey(tomRsa);
        string tomPrivateKey = rsaTool.GetPrivateKey(tomRsa);

        while (true)
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Jim sends an encrypted message to Tom");
            Console.WriteLine("2. Tom sends an encrypted message to Jim");
            Console.WriteLine("3. Exit");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Jim, enter your message: ");
                    string jimMessage = Console.ReadLine();
                    string encryptedForTom = rsaTool.Encrypt(tomPublicKey, jimMessage);
                    Console.WriteLine("Encrypted message: " + encryptedForTom);
                    Console.WriteLine("Tom decrypts the message:");
                    string decryptedByTom = rsaTool.Decrypt(tomPrivateKey, encryptedForTom);
                    Console.WriteLine("Decrypted message: " + decryptedByTom);
                    break;
                case "2":
                    Console.Write("Tom, enter your message: ");
                    string tomMessage = Console.ReadLine();
                    string encryptedForJim = rsaTool.Encrypt(jimPublicKey, tomMessage);
                    Console.WriteLine("Encrypted message: " + encryptedForJim);
                    Console.WriteLine("Jim decrypts the message:");
                    string decryptedByJim = rsaTool.Decrypt(jimPrivateKey, encryptedForJim);
                    Console.WriteLine("Decrypted message: " + decryptedByJim);
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
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
