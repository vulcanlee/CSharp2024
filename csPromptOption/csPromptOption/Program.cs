using Azure.AI.OpenAI;
using OpenAI.Chat;

namespace csPromptOption;

internal class Program
{
    static void Main(string[] args)
    {
        // 讀取環境變數 AOAILabKey 的 API Key
        string apiKey = System.Environment.GetEnvironmentVariable("AOAILabKey");
        AzureOpenAIClient azureClient = new(
            new Uri("https://gpt4tw.openai.azure.com/"),
            new System.ClientModel.ApiKeyCredential(apiKey));
        ChatClient chatClient = azureClient.GetChatClient("gpt-4");

        string userPrompt = "你如何形容海灘？";
        ChatCompletionOptions options = new();
        options.Temperature = 0.8f;

        Chart(chatClient, userPrompt, options);
        NewLine();

        userPrompt = "你如何形容海灘？";
        options = new();
        options.Temperature = 0.3f;

        Chart(chatClient, userPrompt, options);
        NewLine();

        userPrompt = "形容一棵樹";
        options = new();
        options.TopP = 1f;

        Chart(chatClient, userPrompt, options);
        NewLine();

        userPrompt = "形容一棵樹";
        options = new();
        options.TopP = 0.1f;

        Chart(chatClient, userPrompt, options);
        NewLine();

    }

    private static void NewLine()
    {
        Console.WriteLine(new string('-', 40));
        Console.WriteLine(new string('=', 40));
    }

    private static void Chart(ChatClient chatClient, string userPrompt, ChatCompletionOptions options)
    {
        List<ChatMessage> prompts = new()
        {
            UserChatMessage.CreateUserMessage(userPrompt)
        };

        foreach (var message in prompts)
        {
            string roleName = message is SystemChatMessage ? "System" :
                message is UserChatMessage ? "User" :
                "Assistant";
            Console.WriteLine($"{DateTime.Now}  [{roleName}]: {message.Content[0].Text}");
        }

        ChatCompletion completion = chatClient.CompleteChat(prompts, options);
        Console.WriteLine($"Role : {completion.Role}");

        foreach (var message in completion.Content)
        {
            Console.WriteLine($"{DateTime.Now} {message.Text}");
        }
        Console.WriteLine($"InputTokenCount : {completion.Usage.InputTokenCount}");
        Console.WriteLine($"OutputTokenCount : {completion.Usage.OutputTokenCount}");
        Console.WriteLine($"ReasoningTokenCount : {completion.Usage.OutputTokenDetails?.ReasoningTokenCount}");
        Console.WriteLine($"TotalTokenCount : {completion.Usage.TotalTokenCount}");
    }
}
