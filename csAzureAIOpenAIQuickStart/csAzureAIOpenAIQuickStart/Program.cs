using Azure.AI.OpenAI;
using Azure;
using OpenAI.Chat;
using System.Net;

namespace csAzureAIOpenAIQuickStart;

internal class Program
{
    static void Main(string[] args)
    {
        // 讀取環境變數 AOAILabKey 的 API Key
        string apiKey = System.Environment.GetEnvironmentVariable("AOAILabKey");
        AzureOpenAIClient azureClient = new(
            new Uri("https://gpt4tw.openai.azure.com/"),
            new AzureKeyCredential(apiKey));
        ChatClient chatClient = azureClient.GetChatClient("gpt-4");

        string userPrompt = "Say 'this is a test.'";
        Console.WriteLine($"{DateTime.Now}  [User]: {userPrompt}");
        ChatCompletion completion = chatClient.CompleteChat("Say 'this is a test.'");

        Console.WriteLine($"{DateTime.Now}  [ASSISTANT]: {completion}");
    }
}
