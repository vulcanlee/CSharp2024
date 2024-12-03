using Azure.AI.OpenAI;
using OpenAI.Chat;
using System.Reflection.PortableExecutable;
using System.Text;

namespace csChatImage;

internal class Program
{
    static void Main(string[] args)
    {
        // 讀取環境變數 AOAILabKey 的 API Key
        string apiKey = System.Environment.GetEnvironmentVariable("AOAILabKey");
        AzureOpenAIClient azureClient = new(
            new Uri("https://gpt4tw.openai.azure.com/"),
            new System.ClientModel.ApiKeyCredential(apiKey));
        ChatClient chatClient = azureClient.GetChatClient("gpt-4o");

        ChatImage(chatClient);
    }
    private static void ChatImage(ChatClient chatClient)
    {
        #region 提供圖片並與其聊天
        List<ChatMessage> prompts;
        ChatCompletion completion;
        string userPrompt1 = $"從這張圖片中，請分析該名運動員的狀態與給出適當建議";

        var imageFilePath = "sample.png";
        using Stream imageStream = File.OpenRead(imageFilePath);
        var imageBytes = BinaryData.FromStream(imageStream);

        prompts = new List<ChatMessage>
            {
                new UserChatMessage(new List<ChatMessageContentPart>
                {
                    ChatMessageContentPart.CreateTextPart(userPrompt1),
                    ChatMessageContentPart.CreateImagePart(imageBytes, "image/png")
                })
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
