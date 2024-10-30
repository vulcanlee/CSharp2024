using Azure.AI.OpenAI;
using OpenAI.Chat;

namespace csAzureAIOpenAIHistory;

internal class Program
{
    static void Main(string[] args)
    {
        List<ChatMessage> history = new();
        string userPrompt = "請列出何謂 GPT 與其四個主要的特色";
        history.Add(UserChatMessage.CreateUserMessage(userPrompt));
        GptChart(history);

        userPrompt = "請列出何謂 LLM ";
        history.Add(UserChatMessage.CreateUserMessage(userPrompt));
        GptChart(history);

        userPrompt = "請列出何謂 Transformer ";
        history.Add(UserChatMessage.CreateUserMessage(userPrompt));
        GptChart(history);
    }

    private static void GptChart(List<ChatMessage> prompts)
    {
        // 讀取環境變數 AOAILabKey 的 API Key
        string apiKey = System.Environment.GetEnvironmentVariable("AOAILabKey");
        AzureOpenAIClient azureClient = new(
            new Uri("https://gpt4tw.openai.azure.com/"),
            new System.ClientModel.ApiKeyCredential(apiKey));
        ChatClient chatClient = azureClient.GetChatClient("gpt-4");

        foreach (var message in prompts)
        {
            string roleName = message is SystemChatMessage ? "System" :
                message is UserChatMessage ? "User" :
                "Assistant";
            Console.WriteLine($"{DateTime.Now}  [{roleName}]: {message.Content[0].Text}");
        }

        ChatCompletion completion = chatClient.CompleteChat(prompts);

        Console.WriteLine($"{DateTime.Now}  [Assistant]");
        foreach (var message in completion.Content)
        {
            if (message.Text != null && message.Text.Trim() != "")
            {
                Console.WriteLine($"{DateTime.Now} {message.Text}");
                prompts.Add(AssistantChatMessage.CreateAssistantMessage(message.Text));
            }
        }

        Console.WriteLine($"");
        Console.WriteLine($"Role : {completion.Role}");
        Console.WriteLine($"InputTokenCount : {completion.Usage.InputTokenCount}");
        Console.WriteLine($"OutputTokenCount : {completion.Usage.OutputTokenCount}");
        Console.WriteLine($"ReasoningTokenCount : {completion.Usage.OutputTokenDetails?.ReasoningTokenCount}");
        Console.WriteLine($"TotalTokenCount : {completion.Usage.TotalTokenCount}");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(new string('-', 40));
        Console.WriteLine(new string('-', 40));
        Console.WriteLine();
        Console.WriteLine();
    }
}
