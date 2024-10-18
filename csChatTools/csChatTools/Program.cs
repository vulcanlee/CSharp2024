using Azure.AI.OpenAI;
using OpenAI.Chat;
using System.Text.Json;

namespace csChatTools;

// https://zenn.dev/microsoft/articles/dotnet-openai-sdk-v2
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

        PromptCallMethod(chatClient);
    }


    private static void PromptCallMethod(ChatClient chatClient)
    {
        ChatTool getPatientRecordTool = ChatTool.CreateFunctionTool(
            functionName: nameof(GetPatientRecord),
            functionDescription: "使用病歷號與就診日期，查詢該病歷號的當日就醫紀錄",
            functionParameters: BinaryData.FromString("""
            {
                "type": "object",
                "properties": {
                    "chartNo": {
                        "type": "string",
                        "description": "病患的病歷號碼"
                    },
                    "RecordDate": {
                        "type": "string",
                        "description": "為一個日期，用來指明該病患的就醫時間"
                    }
                },
                "required": [ "chartNo","RecordDate" ]
            }
            """)
        );
        #region 自然語言呼叫C#方法
        ChatCompletion completion;
        //string userPrompt1 = "今天有個病人 Vulcan X000456 來詢問，他在2024-1-3來看病，我想知道他當時發生了甚麼問題";
        string userPrompt1 = "在 2023/08/8 的代號為 X000123 就醫相關診斷紀錄";
        //string userPrompt1 = "我要查看病歷號為 X000000 在 2020/12/23 的就醫相關診斷紀錄";
        List<ChatMessage> prompts = new()
        {
            UserChatMessage.CreateUserMessage(userPrompt1),
        };

        ChatCompletionOptions options = new()
        {
            Tools = { getPatientRecordTool },
        };



        string GetToolCallContent(ChatToolCall toolCall)
        {
            if (toolCall.FunctionName == getPatientRecordTool.FunctionName)
            {
                // Validate arguments before using them; it's not always guaranteed to be valid JSON!
                try
                {
                    using JsonDocument argumentsDocument = JsonDocument.Parse(toolCall.FunctionArguments);
                    if (!argumentsDocument.RootElement.TryGetProperty("chartNo", out JsonElement locationElement))
                    {
                        return "Missing required argument 'chartNo'";
                    }
                    else
                    {
                        string chartNo = locationElement.GetString();
                        if (argumentsDocument.RootElement.TryGetProperty("RecordDate", out JsonElement unitElement))
                        {
                            string RecordDate = unitElement.GetString();
                            return GetPatientRecord(chartNo, RecordDate);
                        }
                        else
                        {
                            return "Missing required argument 'RecordDate'";
                        }
                    }
                }
                catch (JsonException)
                {
                    return "Invalid JSON";
                }
            }
            // Handle unexpected tool calls
            throw new NotImplementedException();
        }




        while (true)
        {
            ShowPrompt(prompts);
            completion = chatClient.CompleteChat(prompts, options);
            ShowCompletionResult(completion);
            if (completion.FinishReason == ChatFinishReason.Stop)
            {
                Console.WriteLine($"{DateTime.Now}  [Assistant]");
                foreach (var message in completion.Content)
                {
                    Console.WriteLine($"{DateTime.Now} {message.Text}");
                }
                break;
            }

            if (completion.FinishReason == ChatFinishReason.ToolCalls)
            {
                // Add a new assistant message to the conversation history that includes the tool calls
                prompts.Add(new AssistantChatMessage(completion));

                foreach (ChatToolCall toolCall in completion.ToolCalls)
                {
                    prompts.Add(new ToolChatMessage(toolCall.Id, GetToolCallContent(toolCall)));
                }

                //completion = chatClient.CompleteChat(prompts, options);

            }
            else
            {
                throw new InvalidOperationException("今回のサンプルでは考慮してない結果。");
            }
        }
        #endregion
    }

    private static void ShowPrompt(List<ChatMessage> prompts)
    {
        foreach (var message in prompts)
        {
            string roleName = message is SystemChatMessage ? "System" :
                message is UserChatMessage ? "User" :
                message is AssistantChatMessage ? "Assistant" : 
                message is ToolChatMessage ? "Tool": "N/A";
            Console.WriteLine($"{DateTime.Now}  [{roleName}]: {message.Content.FirstOrDefault()?.Text}");
        }
    }

    private static void ShowCompletionResult(ChatCompletion completion)
    {
        Console.WriteLine($"");
        Console.WriteLine($"Role : {completion.Role}");
        Console.WriteLine($"InputTokenCount : {completion.Usage.InputTokenCount}");
        Console.WriteLine($"OutputTokenCount : {completion.Usage.OutputTokenCount}");
        Console.WriteLine($"ReasoningTokenCount : {completion.Usage.OutputTokenDetails?.ReasoningTokenCount}");
        Console.WriteLine($"TotalTokenCount : {completion.Usage.TotalTokenCount}");
        Console.WriteLine($"");
        Console.WriteLine($"");
    }

    static string GetPatientRecord(string chartNo, string RecordDate)
    {
        if (chartNo == "X000123")
            return $"病人:{chartNo} 於 {RecordDate} 來看肚子痛";
        else if (chartNo == "X000456")
            return $"病人:{chartNo} 於 {RecordDate} 來看痛風";
        else
            return $"病人:{chartNo} 於 {RecordDate} 來看感冒";
    }
}
