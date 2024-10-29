using Azure.AI.Inference;
using Azure;
using System.ClientModel.Primitives;

namespace csInference
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Chart("",
                "");
            Chart("",
                "");
        }

        private static void Chart(string endpoint, string key)
        {
            ChatCompletionsClient client = new ChatCompletionsClient(
                new Uri(endpoint),
                new AzureKeyCredential(key)
            );

            var requestOptions = new ChatCompletionsOptions()
            {
                Messages = {
        //new ChatRequestSystemMessage("You are a helpful assistant."),
        //new ChatRequestUserMessage("How many languages are in the world?")
        new ChatRequestUserMessage("'''就醫時間為 2020-2022 年之間，具有糖尿病診斷且為門診就醫的搜尋條件'''") ,
        new ChatRequestUserMessage("請將上述需求，使用底下 JSON 格式來表達，只需要生成 JSON，不需要其他說明") ,
        new ChatRequestUserMessage("{\r\n  // 醫療紀錄搜尋條件\r\n  \"BaseCondition\": {\r\n    \"BeginDate\": \"\", // 就醫紀錄搜尋開始時間\r\n    \"EndDate\": \"\",  // 就醫紀錄搜尋結束時間\r\n    \"Department\": \"\",      // 就醫紀錄搜尋科別 內科,心臟科\r\n    \"RecordType\": \"\"       // 就醫紀錄搜尋類型 門診,急診,住院\r\n  }\r\n  \"ICD10\": [] // ICD10疾病碼 搜尋條件\r\n  \"Medicine\": [] // 藥品名稱 搜尋條件\r\n}") },
                AdditionalProperties = { { "logprobs", BinaryData.FromString("false") } },
            };

            var response = client.Complete(requestOptions);
            Console.WriteLine($"Response: {response.Value.Content}");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(new string('-',40));
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
