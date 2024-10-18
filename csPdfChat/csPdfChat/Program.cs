using Azure.AI.OpenAI;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using OpenAI.Chat;
using System.Text;

namespace csPdfChat;

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

        string pdfContent = GetPdf("jmir-2024-1-e52399.pdf");
        PromptToJson(chatClient, pdfContent);
    }

    static string GetPdf(string filename)
    {
        StringBuilder result = new StringBuilder();

        using (PdfReader pdfReader = new PdfReader(filename))
        {
            using (PdfDocument pdfDoc = new PdfDocument(pdfReader))
            {
                int numberOfPages = pdfDoc.GetNumberOfPages();

                for (int i = 1; i <= numberOfPages; i++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string pageContent = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i), strategy);
                    result.AppendLine(pageContent);
                }
            }
        }

        string content = result.ToString();
        return content;
    }

    private static void PromptToJson(ChatClient chatClient, string pdfContent)
    {
        #region 將自然語言轉乘 JSON
        List<ChatMessage> prompts;
        ChatCompletion completion;
        string userPrompt1 = $"'''{pdfContent}'''";
        //string userPrompt2 = "大語言模型，經過研究，在醫療照顧上是否具有可行性，有甚麼要注意的地方";
        //string userPrompt2 = "摘要這篇論文的重點與結論";
        string userPrompt2 = "醫療照顧採用 LLM 是否可行，多久可以實現這樣的技術";
        prompts = new()
        {
            UserChatMessage.CreateUserMessage(userPrompt1),
            UserChatMessage.CreateUserMessage(userPrompt2),
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
