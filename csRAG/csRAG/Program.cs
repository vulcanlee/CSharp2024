using Azure.AI.OpenAI;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using OpenAI.Chat;
using OpenAI.Embeddings;
using System.Text;

namespace csRAG
{
    public class ChunkItem
    {
        public string Text { get; set; }
        public string Filename { get; set; }
        public List<float> Embedding { get; set; }
        public float Similarity { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // 讀取環境變數 AOAILabKey 的 API Key
            string apiKey = System.Environment.GetEnvironmentVariable("AOAILabKey");
            AzureOpenAIClient azureClient = new(
                new Uri("https://gpt4tw.openai.azure.com/"),
                new System.ClientModel.ApiKeyCredential(apiKey));
            //EmbeddingClient embeddingClient = azureClient.GetEmbeddingClient("text-embedding-3-large");
            EmbeddingClient embeddingClient = azureClient.GetEmbeddingClient("text-embedding-3-small");
            ChatClient chatClient = azureClient.GetChatClient("gpt-4o");

            // 建立 Embedding
            List<ChunkItem> embeddings = BuildEmbedding(embeddingClient);

            string prompt = "對於一本好的 ASP.NET Core Web API 開發書籍，最重要的五個章節是甚麼?";
            //string prompt = "若要設計一個軟體醫材並且要能更進行推廣與獲得更多的成效，該如何做";
            ChunkItem promptEmbedding = PromptToEmbedding(embeddingClient, prompt);

            ChunkItem result = FindMostSimilarDocument(embeddings, promptEmbedding);

            System.Console.WriteLine($"最相關的文件是 ({result.Similarity}) {result.Filename}");
            prompt = $"{prompt}\n\n使用底下內容來生成 zh-tw 結果\n\n'''{result.Text}'''";

            ChatWithGPT(chatClient, prompt);
        }

        #region 根據最相近的檔案進行 GPT 對話
        static void ChatWithGPT(ChatClient chatClient, string prompt)
        {
            List<ChatMessage> prompts = new()
            {
                UserChatMessage.CreateUserMessage(prompt),
            };

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
                    Console.WriteLine($"{DateTime.Now} {message.Text}");
            }
        }
        #endregion

        #region 找出最相關的文件
        static ChunkItem FindMostSimilarDocument(List<ChunkItem> embeddings,
            ChunkItem promptEmbedding)
        {
            ChunkItem result = new ChunkItem();
            double maxSimilarity = 0;
            foreach (var item in embeddings)
            {
                double similarity = CosineSimilarity(item.Embedding.ToArray(),
                    promptEmbedding.Embedding.ToArray());
                item.Similarity = (float)similarity;
                if (similarity > maxSimilarity)
                {
                    maxSimilarity = similarity;
                    result = item;
                }
            }
            return result;
        }
        static float CosineSimilarity(float[] vectorA, float[] vectorB)
        {
            if (vectorA.Length != vectorB.Length)
                throw new ArgumentException("Vectors must be of the same length.");

            float dotProduct = vectorA.Zip(vectorB, (a, b) => a * b).Sum();
            float magnitudeA = (float)Math.Sqrt(vectorA.Sum(a => a * a));
            float magnitudeB = (float)Math.Sqrt(vectorB.Sum(b => b * b));

            return dotProduct / (magnitudeA * magnitudeB);
        }
        #endregion

        #region prompt 轉 Embedding
        static ChunkItem PromptToEmbedding(EmbeddingClient embeddingClient,
            string prompt)
        {
            ChunkItem result = new ChunkItem();
            string content = prompt;
            var embedding = embeddingClient.GenerateEmbedding(content);
            var value = embedding.Value.ToFloats().ToArray().ToList();
            result = new ChunkItem
            {
                Text = content,
                Filename = "Prompt",
                Embedding = value
            };
            return result;
        }
        #endregion

        #region 讀取檔案文字並且取得文件內容的 Embedding
        static List<ChunkItem> BuildEmbedding(EmbeddingClient embeddingClient)
        {
            List<ChunkItem> embeddings = new List<ChunkItem>();
            for (int i = 1; i <= 4; i++)
            {
                string filename = $"file{i}.pdf";
                string content = GetPdf(filename);
                var embedding = embeddingClient.GenerateEmbedding(content);
                var value = embedding.Value.ToFloats().ToArray().ToList();
                ChunkItem item = new ChunkItem
                {
                    Text = content,
                    Filename = filename,
                    Embedding = value
                };
                embeddings.Add(item);
            }
            return embeddings;
        }
        #endregion

        #region 讀取 PDF
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
        #endregion
    }
}
