using Azure.AI.OpenAI;
using OpenAI.Chat;

namespace csZeroOneFewShot;

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

        ZeroShot(chatClient, "請判斷以下產品評論的情感是正面還是負面：'這款產品的質量非常糟糕，我絕對不會再買。'");
        OneShot(chatClient, "這款產品的質量非常糟糕，我絕對不會再買。");
        FewShot(chatClient, "這款產品的質量非常糟糕，我絕對不會再買。");
        FewShot(chatClient, "中午吃蛋包飯");
    }


    private static void ZeroShot(ChatClient chatClient, string promptText)
    {
        #region zero-shot
        string userPrompt;
        List<ChatMessage> prompts;
        ChatCompletion completion;
        string assistantPrompt;
        Console.WriteLine("Zero-Shot");
        Console.WriteLine(new string('-', 40));
        string userPrompt1 = "請判斷以下產品評論的情感是正面還是負面：'這款產品的質量非常糟糕，我絕對不會再買。'";
        userPrompt = $"{promptText}";
        prompts = new()
        {
            UserChatMessage.CreateUserMessage(userPrompt),
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

    private static void OneShot(ChatClient chatClient, string promptText)
    {
        #region one-shot
        string userPrompt;
        ChatCompletion completion;
        string assistantPrompt;
        userPrompt = "請判斷以下產品評論的情感是正面還是負面：'這款產品真的很棒，我非常滿意！'";
        assistantPrompt = "情感：正面";
        string userPrompt1 = $"{promptText}";

        Console.WriteLine("One-Shot");
        Console.WriteLine(new string('-', 40));
        List<ChatMessage> promptsOneShot = new()
        {
            UserChatMessage.CreateUserMessage(userPrompt),
            UserChatMessage.CreateAssistantMessage(assistantPrompt),
            UserChatMessage.CreateUserMessage(userPrompt1),
        };

        foreach (var message in promptsOneShot)
        {
            string roleName = message is SystemChatMessage ? "System" :
                message is UserChatMessage ? "User" :
                "Assistant";
            Console.WriteLine($"{DateTime.Now}  [{roleName}]: {message.Content[0].Text}");
        }

        ChatCompletion completionOneShot = chatClient.CompleteChat(promptsOneShot);

        Console.WriteLine($"{DateTime.Now}  [Assistant]");
        foreach (var message in completionOneShot.Content)
        {
            Console.WriteLine($"{DateTime.Now} {message.Text}");
        }

        Console.WriteLine($"");
        Console.WriteLine($"Role : {completionOneShot.Role}");
        Console.WriteLine($"InputTokenCount : {completionOneShot.Usage.InputTokenCount}");
        Console.WriteLine($"OutputTokenCount : {completionOneShot.Usage.OutputTokenCount}");
        Console.WriteLine($"ReasoningTokenCount : {completionOneShot.Usage.OutputTokenDetails?.ReasoningTokenCount}");
        Console.WriteLine($"TotalTokenCount : {completionOneShot.Usage.TotalTokenCount}");
        Console.WriteLine($"");
        Console.WriteLine($"");
        #endregion
    }

    private static void FewShot(ChatClient chatClient, string promptText)
    {
        #region few-shot
        string userPrompt;
        List<ChatMessage> prompts;
        ChatCompletion completion;
        string assistantPrompt;
        string systemPrompt = "需要分析傳入文字的情感，並且只輸出:正面、負面、不知道，不需要其他多餘內容";
        userPrompt = "請判斷以下產品評論的情感是正面還是負面：'這款產品真的很棒，我非常滿意！'";
        assistantPrompt = "情感：正面";
        string userPrompt2 = "包裝很爛，產品也有問題。";
        string assistantPrompt2 = "情感：正面";
        //string userPrompt3 = "只需要回答:［正面］、［負面］、［不知道］，" +
        //    "這三個文字的其中一個，不需要其他多餘內容或者做說明，" +
        //    $"問題:'''{promptText}'''" +
        //    "";
        string userPrompt3 = $"{promptText}";

        Console.WriteLine("Few-Shot");
        Console.WriteLine(new string('-', 40));
        List<ChatMessage> promptsFewShot = new()
        {
            //UserChatMessage.CreateSystemMessage(systemPrompt),
            UserChatMessage.CreateUserMessage(userPrompt),
            UserChatMessage.CreateAssistantMessage(assistantPrompt),
            UserChatMessage.CreateUserMessage(userPrompt2),
            UserChatMessage.CreateAssistantMessage(assistantPrompt2),
            UserChatMessage.CreateUserMessage(userPrompt3),
        };

        foreach (var message in promptsFewShot)
        {
            string roleName = message is SystemChatMessage ? "System" :
                message is UserChatMessage ? "User" :
                "Assistant";
            Console.WriteLine($"{DateTime.Now}  [{roleName}]: {message.Content[0].Text}");
        }

        ChatCompletion completionFewShot = chatClient.CompleteChat(promptsFewShot);

        Console.WriteLine($"{DateTime.Now}  [Assistant]");
        foreach (var message in completionFewShot.Content)
        {
            Console.WriteLine($"{DateTime.Now} {message.Text}");
        }

        Console.WriteLine($"");
        Console.WriteLine($"Role : {completionFewShot.Role}");
        Console.WriteLine($"InputTokenCount : {completionFewShot.Usage.InputTokenCount}");
        Console.WriteLine($"OutputTokenCount : {completionFewShot.Usage.OutputTokenCount}");
        Console.WriteLine($"ReasoningTokenCount : {completionFewShot.Usage.OutputTokenDetails?.ReasoningTokenCount}");
        Console.WriteLine($"TotalTokenCount : {completionFewShot.Usage.TotalTokenCount}");
        Console.WriteLine($"");
        Console.WriteLine($"");
        #endregion
    }
}
