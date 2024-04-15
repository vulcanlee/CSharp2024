using Nest;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace csElasticsearchNestFindIndex;

[ElasticsearchType(IdProperty = nameof(BlogId))]
public class Blog
{
    public int BlogId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreateAt { get; set; } = DateTime.Now;
    public DateTime UpdateAt { get; set; } = DateTime.Now;
}
internal class Program
{
    static string IndexPrefix = "random-index";
    static async Task Main(string[] args)
    {
        var settings = new ConnectionSettings(new Uri("http://10.1.1.231:9200/"))
            .DisableDirectStreaming()
            //.OnRequestCompleted(response =>
            //{
            //    Console.WriteLine($"Request: {response.RequestBodyInBytes?.Length} bytes");
            //    Console.WriteLine($"Response: {response.ResponseBodyInBytes?.Length} bytes");
            //})
            .BasicAuthentication("elastic", "elastic");

        var client = new ElasticClient(settings);

        await MakeRandomIndex(client);
        await FindMatchIndex(client);
    }

    static async Task FindMatchIndex(IElasticClient client)
    {
        var indexNamesResponse = await client.Indices.GetAliasAsync();
        var indexNames = indexNamesResponse.Indices.Keys.ToList();
        var findIndex = indexNames
            .Where(x=>x.Name.Contains(IndexPrefix+"-"));
        foreach (var index in findIndex)
        {
            Console.WriteLine($"Found index : {index}");
        }
        await Console.Out.WriteLineAsync($"按下任一按鍵，將會開始刪除 {IndexPrefix}- 開頭的索引名稱");
        Console.ReadKey();
        foreach (var index in findIndex)
        {
            await client.Indices.DeleteAsync(index);
            Console.WriteLine($"Found index : {index}");
        }
    }

    static async Task MakeRandomIndex(IElasticClient client)
    {
        int totalIndex = 10;
        Random random = new Random();
        for (var i = 0; i < totalIndex; i++)
        {
            var indexName = $"{IndexPrefix}-{i}";

            var response = await client.IndexAsync<Blog>(new Blog()
            {
                BlogId = random.Next(),
                Title = $"Nice to meet your 999",
                Content = $"Hello Elasticsearch 999",
                CreateAt = DateTime.Now.AddDays(999),
                UpdateAt = DateTime.Now.AddDays(999),
            }, idx => idx.Index(indexName));
            if (response.IsValid)
            {
                //Console.WriteLine($"Index document with ID {response.Id} succeeded.");
            }
            else
            {
                Console.WriteLine($"Error Message : {response.DebugInformation}");
            }
        }

        Console.WriteLine($"Total indices ({totalIndex}) has beed created");
    }
}
