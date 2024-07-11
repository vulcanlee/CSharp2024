using System.Security.Cryptography;
using System.Text;

namespace csDigitalSignature;

internal class Program
{
    static void Main(string[] args)
    {
        DigitalSignature digitalSignature = new DigitalSignature();
        Console.WriteLine("Generating keys for Jim...");
        RSA jimRsa = digitalSignature.GenerateKeys();
        string jimPublicKey = digitalSignature.GetPublicKey(jimRsa);
        string jimPrivateKey = digitalSignature.GetPrivateKey(jimRsa);

        Console.WriteLine("Generating keys for Tom...");
        RSA tomRsa = digitalSignature.GenerateKeys();
        string tomPublicKey = digitalSignature.GetPublicKey(tomRsa);
        string tomPrivateKey = digitalSignature.GetPrivateKey(tomRsa);

        while (true)
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Jim signs a message for Tom");
            Console.WriteLine("2. Tom signs a message for Jim");
            Console.WriteLine("3. Exit");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Jim, enter your message: ");
                    string jimMessage = Console.ReadLine();
                    string jimSignature = digitalSignature.Sign(jimPrivateKey, jimMessage);
                    Console.WriteLine("Jim's signature: " + jimSignature);
                    Console.WriteLine("Tom verifies the message:");
                    bool verifiedByTom = digitalSignature.Verify(jimPublicKey, jimMessage, jimSignature);
                    Console.WriteLine("Verification result: " + (verifiedByTom ? "Valid" : "Invalid"));

                    // 模擬訊息被篡改
                    Console.WriteLine("\nSimulating message tampering:");
                    string tamperedMessage = jimMessage + " (tampered)";
                    bool verifiedTamperedByTom = digitalSignature.Verify(jimPublicKey, tamperedMessage, jimSignature);
                    Console.WriteLine("Verification result for tampered message: " + (verifiedTamperedByTom ? "Valid" : "Invalid"));
                    break;
                case "2":
                    Console.Write("Tom, enter your message: ");
                    string tomMessage = Console.ReadLine();
                    string tomSignature = digitalSignature.Sign(tomPrivateKey, tomMessage);
                    Console.WriteLine("Tom's signature: " + tomSignature);
                    Console.WriteLine("Jim verifies the message:");
                    bool verifiedByJim = digitalSignature.Verify(tomPublicKey, tomMessage, tomSignature);
                    Console.WriteLine("Verification result: " + (verifiedByJim ? "Valid" : "Invalid"));

                    // 模擬訊息被篡改
                    Console.WriteLine("\nSimulating message tampering:");
                    string tamperedTomMessage = tomMessage + " (tampered)";
                    bool verifiedTamperedByJim = digitalSignature.Verify(tomPublicKey, tamperedTomMessage, tomSignature);
                    Console.WriteLine("Verification result for tampered message: " + (verifiedTamperedByJim ? "Valid" : "Invalid"));
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

public class DigitalSignature
{
    public RSA GenerateKeys()
    {
        return RSA.Create(2048); // 2048 位元的密鑰長度
    }

    public string GetPublicKey(RSA rsa)
    {
        return Convert.ToBase64String(rsa.ExportRSAPublicKey());
    }

    public string GetPrivateKey(RSA rsa)
    {
        return Convert.ToBase64String(rsa.ExportRSAPrivateKey());
    }

    public string Sign(string privateKey, string message)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] signatureBytes = rsa.SignData(messageBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return Convert.ToBase64String(signatureBytes);
        }
    }

    public bool Verify(string publicKey, string message, string signature)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] signatureBytes = Convert.FromBase64String(signature);
            return rsa.VerifyData(messageBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}
