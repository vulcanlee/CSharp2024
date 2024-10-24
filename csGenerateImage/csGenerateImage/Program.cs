using Azure.AI.OpenAI;
using OpenAI.Chat;
using OpenAI.Images;
using System.ClientModel;
using static System.Net.Mime.MediaTypeNames;

namespace csGenerateImage;

internal class Program
{
    static void Main(string[] args)
    {
        // 讀取環境變數 AOAILabKey 的 API Key
        string apiKey = System.Environment.GetEnvironmentVariable("AOAIImageKey");
        AzureOpenAIClient azureClient = new(
            new Uri("https://vulca-m2mwld4n-australiaeast.openai.azure.com/"),
            new System.ClientModel.ApiKeyCredential(apiKey));
        ImageClient imageClient = azureClient.GetImageClient("dall-e-3");

        string userPrompt = "一隻在天上飛的蟑螂";
        Console.WriteLine($"{DateTime.Now}  [User]: {userPrompt}");
        ImageGenerationOptions options = new()
        {
            Quality = GeneratedImageQuality.High,
            Size = GeneratedImageSize.W1792xH1024,
            Style = GeneratedImageStyle.Vivid,
            ResponseFormat = GeneratedImageFormat.Bytes
        };
        GeneratedImage generatedImage = imageClient.GenerateImage(userPrompt, options);

        BinaryData bytes = generatedImage.ImageBytes;
        using FileStream stream = File.OpenWrite($"bug.png");
        bytes.ToStream().CopyTo(stream);
        Console.WriteLine($"{DateTime.Now}  [User]: {userPrompt} 圖片已經產生了");
    }
}
