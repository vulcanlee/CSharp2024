using Azure.AI.OpenAI;
using Azure;
using OpenAI.Chat;

namespace csAzureAIOpenAIQuickStart;

public class MagicObject
{
    public const string AZURE_OPENAI_API_ENDPOINT = "https://gpt4tw.openai.azure.com/";
    public const string GPT4_Model_NAME = "gpt-4";
    public const string GPT4_32K_Model_NAME = "gpt-4-32k";
    public const string GPT35_TURBO_16K_Model_NAME = "gpt-35-turbo-16k";
    public const string GPT35_TURBO_Model_NAME = "GPT-35-TURBO";
}

public class AzureOpenAIClientFactory
{
    public AzureOpenAIClient CreateAzureOpenAIClient(string apiKey, string endPoint)
    {

        // 使用 API Key 建立 AzureOpenAIClient 物件
        AzureOpenAIClient azureClient = new(
            new Uri(endPoint),
            new System.ClientModel.ApiKeyCredential(apiKey));
        return azureClient;
    }

    public ChatClient CreateChatClient(AzureOpenAIClient azureClient, string modelName)
    {
        ChatClient chatClient = azureClient.GetChatClient(modelName);
        return chatClient;
    }
}
internal class Program
{
    static void Main(string[] args)
    {
        // 讀取環境變數 AOAILabKey 的 API Key
        string apiKey = System.Environment.GetEnvironmentVariable("AOAILabKey");
        var openAiFactory = new AzureOpenAIClientFactory();
        AzureOpenAIClient azureClient = openAiFactory
            .CreateAzureOpenAIClient(apiKey, MagicObject.AZURE_OPENAI_API_ENDPOINT);
        ChatClient chatClient = openAiFactory.CreateChatClient(azureClient, MagicObject.GPT4_Model_NAME);

        //string userPrompt = "Hello, I am a chatbot. How can I help you today?";
        string userPrompt = "Say 'this is a test.'";
        Console.WriteLine($"{DateTime.Now}  [User]: {userPrompt}");
        ChatCompletion completion = chatClient.CompleteChat("Say 'this is a test.'");

        foreach (var content in completion.Content)
        {
            Console.WriteLine($"{DateTime.Now}  [ASSISTANT]: {content.Text}");
        }

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine(Environment.NewLine);
        Console.WriteLine(new string('-', 40));
        Console.WriteLine(Environment.NewLine);

        userPrompt = "列出所有中風的 ICD-10 前三碼, zh-tw";
        Console.WriteLine($"{DateTime.Now}  [User]: {userPrompt}");

        completion = chatClient.CompleteChat(userPrompt);

        foreach (var content in completion.Content)
        {
            Console.WriteLine($"{DateTime.Now}  [ASSISTANT]: {content.Text}");
        }
    }
}
