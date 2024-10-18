using Azure.AI.OpenAI;
using OpenAI.Chat;

namespace csGptNlpToJson;

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

        PromptToJson(chatClient);
    }


    private static void PromptToJson(ChatClient chatClient)
    {
        #region 將自然語言轉乘 JSON
        string json = File.ReadAllText("SearchCondition.json");
        List<ChatMessage> prompts;
        ChatCompletion completion;
        string userPrompt1 = "'''就醫時間為 2020-2022 年之間，具有糖尿病診斷且為門診就醫的搜尋條件'''";
        string userPrompt2 = "請將上述需求，使用底下 JSON 格式來表達，只需要生成 JSON，" +
            "不需要其他說明";
        string userPrompt3 = $"```json\n{json}\n```";
        prompts = new()
        {
            UserChatMessage.CreateUserMessage(userPrompt1),
            UserChatMessage.CreateUserMessage(userPrompt2),
            UserChatMessage.CreateUserMessage(userPrompt3),
        };
        foreach (var message in prompts)
        {
            string roleName = message is SystemChatMessage ? "System" :
                message is UserChatMessage ? "User" :
                "Assistant";
            Console.WriteLine($"{DateTime.Now}  [{roleName}]: {message.Content[0].Text}");
        }

        completion = chatClient.CompleteChat(prompts);
        Console.WriteLine($"{DateTime.Now}  [Assistant]");
        foreach (var message in completion.Content)
        {
            Console.WriteLine($"{DateTime.Now} {message.Text}");
        }

        Console.WriteLine($"");
        Console.WriteLine($"Role : {completion.Role}");
        Console.WriteLine($"InputTokenCount : {completion.Usage.InputTokenCount}");
        Console.WriteLine($"OutputTokenCount : {completion.Usage.OutputTokenCount}");
        Console.WriteLine($"ReasoningTokenCount : {completion.Usage.OutputTokenDetails?.ReasoningTokenCount}");
        Console.WriteLine($"TotalTokenCount : {completion.Usage.TotalTokenCount}");
        Console.WriteLine($"");
        Console.WriteLine($"");
        #endregion
    }
}
