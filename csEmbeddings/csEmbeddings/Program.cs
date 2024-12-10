using Azure.AI.OpenAI;
using OpenAI.Chat;
using OpenAI.Embeddings;
using System.ClientModel;

namespace csEmbeddings;

internal class Program
{
    static void Main(string[] args)
    {
        // 讀取環境變數 AOAILabKey 的 API Key
        string apiKey = System.Environment.GetEnvironmentVariable("AOAILabKey");
        AzureOpenAIClient azureClient = new(
            new Uri("https://gpt4tw.openai.azure.com/"),
            new System.ClientModel.ApiKeyCredential(apiKey));
        EmbeddingClient embeddingClient = azureClient.GetEmbeddingClient("text-embedding-3-large");
        //EmbeddingClient embeddingClient = azureClient.GetEmbeddingClient("text-embedding-3-small");

        string userPrompt = "[客戶服務] 如何聯絡蝦皮客服？\n" +
        "您可以在以下位置找到蝦皮客服團隊(請使用App並登入)：蝦皮購物App右下角『我的』➜ 滑至下方，即可看到客服選項。\n\n" +
        "🔹網路電話客服（語音）：請點選【透過免費網路電話聯繫客服】（下圖一）\n" +
        " 真人客服服務時間：週一至週五 09:00 - 18:00\n\n" +
        "🔹即時交談客服（文字）：請點選【與客服即時交談】（下圖二）\n\n" +
        "真人客服服務時間：週一至週五 09:00 - 21:00\n" +
        "* 特殊服務時間變動請參考置頂公告";

        Console.WriteLine($"{DateTime.Now}  [User]: {userPrompt}");

        ClientResult<OpenAIEmbedding> embedding = 
            embeddingClient.GenerateEmbedding(userPrompt);

        ReadOnlyMemory<float> vector = embedding.Value.ToFloats();

        foreach (var value in vector.Span)
        {
            Console.Write($"{value},");
        }

        Console.WriteLine();
    }
}
