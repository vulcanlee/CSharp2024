using Azure.AI.OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace csChatStream;

internal class Program
{
    static async Task Main(string[] args)
    {
        // 讀取環境變數 AOAILabKey 的 API Key
        string apiKey = System.Environment.GetEnvironmentVariable("AOAILabKey");
        AzureOpenAIClient azureClient = new(
            new Uri("https://gpt4tw.openai.azure.com/"),
            new System.ClientModel.ApiKeyCredential(apiKey));
        ChatClient chatClient = azureClient.GetChatClient("gpt-4");

        await ChatStream(chatClient, "請以正大光明為主題，做出一首詩與一篇散文");
    }

    private static async Task ChatStream(ChatClient chatClient, string promptText)
    {
        #region 串流媒體聊天
        List<ChatMessage> prompts;
        AsyncCollectionResult<StreamingChatCompletionUpdate> completionUpdates;
        Console.WriteLine("串流媒體聊天");
        Console.WriteLine(new string('-', 40));
        prompts = new()
        {
            UserChatMessage.CreateUserMessage(promptText),
        };
        foreach (var message in prompts)
        {
            string roleName = message is SystemChatMessage ? "System" :
                message is UserChatMessage ? "User" :
                "Assistant";
            Console.WriteLine($"{DateTime.Now}  [{roleName}]: {message.Content[0].Text}");
        }

        //completion = chatClient.CompleteChat(prompts);
        completionUpdates =
           chatClient.CompleteChatStreamingAsync(prompts);
        Console.WriteLine($"{DateTime.Now}  [Assistant]");
        Console.Write($"{DateTime.Now} ");
        await foreach (StreamingChatCompletionUpdate completionUpdate in completionUpdates)
        {
            if (completionUpdate.ContentUpdate.Count > 0)
            {
                foreach (var message in completionUpdate.ContentUpdate)
                {
                    Console.Write($"{message.Text}");
                }
            }
        }
        Console.WriteLine();

        //Console.WriteLine($"");
        //Console.WriteLine($"Role : {completionUpdate.ContentUpdate}");
        //Console.WriteLine($"InputTokenCount : {completionUpdate.Usage.InputTokenCount}");
        //Console.WriteLine($"OutputTokenCount : {completionUpdate.Usage.OutputTokenCount}");
        //Console.WriteLine($"ReasoningTokenCount : {completionUpdate.Usage.OutputTokenDetails?.ReasoningTokenCount}");
        //Console.WriteLine($"TotalTokenCount : {completionUpdate.Usage.TotalTokenCount}");
        //Console.WriteLine($"");
        //Console.WriteLine($"");
        #endregion
    }
}
