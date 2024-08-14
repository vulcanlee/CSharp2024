using Newtonsoft.Json.Linq;
using System.Xml;

namespace csJsonMerge;

internal class Program
{
    static void Main(string[] args)
    {
        // 舊 JSON
        string oldJson = """ 
        {
            "F1": 123,
            "F2": "abc"
        }
        """;

        // 新 JSON
        string newJson = """
        {
            "F1": "890",
            "F3": true
        }
        """;

        // 將 JSON 轉換為 JObject
        JObject oldJsonObject = JObject.Parse(oldJson);
        JObject newJsonObject = JObject.Parse(newJson);

        // 合併兩個 JObject
        oldJsonObject.Merge(newJsonObject, new JsonMergeSettings
        {
            // 覆蓋相同的屬性
            MergeArrayHandling = MergeArrayHandling.Merge
        });

        // 轉換回 JSON 字串
        string mergedJson = oldJsonObject.ToString((Newtonsoft.Json.Formatting)Formatting.Indented);

        // 顯示結果
        Console.WriteLine(mergedJson);
    }
}
