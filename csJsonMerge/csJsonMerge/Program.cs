using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
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

        #region 使用 Json.NET
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
        #endregion

        #region 使用 System.Text.Json
        JsonNode node1 = JsonNode.Parse(oldJson);
        JsonNode node2 = JsonNode.Parse(newJson);

        JsonNode MergeJsonNodes(JsonNode target, JsonNode source)
        {
            if (source is JsonObject sourceObject && target is JsonObject targetObject)
            {
                foreach (var property in sourceObject)
                {
                    if (targetObject.ContainsKey(property.Key) &&
                        targetObject[property.Key] is JsonObject &&
                        property.Value is JsonObject)
                    {
                        targetObject[property.Key] = MergeJsonNodes(targetObject[property.Key], property.Value);
                    }
                    else
                    {
                        targetObject[property.Key] = property.Value.DeepClone();
                    }
                }
                return targetObject;
            }
            return source.DeepClone();
        }

        JsonNode mergedNode = MergeJsonNodes(node1.DeepClone(), node2);

        string mergedJson2 = mergedNode.ToJsonString(new JsonSerializerOptions
        {
            WriteIndented = true
        });

        Console.WriteLine(mergedJson2);
        #endregion
    }
}
