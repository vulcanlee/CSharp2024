using Nest;

namespace csElasticsearchNestChangeSetting;

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
    static string IndexName = $"blog_setting";
    static async Task Main(string[] args)
    {
        var settings = new ConnectionSettings(new Uri("http://10.1.1.231:9200"))
            .DisableDirectStreaming()
            .BasicAuthentication("elastic", "elastic");

        var client = new ElasticClient(settings);

        await Console.Out.WriteLineAsync($"生成索引");
        await MakeRandomIndex(client);
        await Console.Out.WriteLineAsync($"取得索引設定");
        await GetIndexAllSetting(client);
        await Console.Out.WriteLineAsync($"變大 Result Windows Size");
        await EnlargeResultSize(client);
        await Console.Out.WriteLineAsync($"取得索引設定");
        await GetIndexAllSetting(client);
        await Console.Out.WriteLineAsync($"移除 Result Windows Size");
        await RemoveResultSize(client);
        await Console.Out.WriteLineAsync($"取得索引設定");
        await GetIndexAllSetting(client);
    }

    static async Task GetIndexAllSetting(IElasticClient client)
    {
        var response = client.Indices.GetSettings(IndexName);

        if (response.IsValid)
        {
            var indexSettings = response.Indices[IndexName].Settings;
            foreach (var setting in indexSettings)
            {
                Console.WriteLine($"{setting.Key}: {setting.Value}");
            }
        }
        else
        {
            Console.WriteLine($"Failed to retrieve index settings: {response.DebugInformation}");
        }
    }

    static async Task RemoveResultSize(IElasticClient client)
    {
        await Console.Out.WriteLineAsync();
        // 更新索引的配置
        var response = await client.Indices.UpdateSettingsAsync(IndexName, uis => uis
            .IndexSettings(s => s

                .Setting("index.max_result_window", 10_000)
            )
        );
        if (response.IsValid)
        {
            Console.WriteLine("Index settings updated successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to update index settings: {response.DebugInformation}");
        }
        await Console.Out.WriteLineAsync();
    }

    static async Task EnlargeResultSize(IElasticClient client)
    {
        await Console.Out.WriteLineAsync();
        // 更新索引的配置
        var response = await client.Indices.UpdateSettingsAsync(IndexName, uis => uis
            .IndexSettings(s => s

                .Setting("index.max_result_window", 200_000)
            )
        );
        if (response.IsValid)
        {
            Console.WriteLine("Index settings updated successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to update index settings: {response.DebugInformation}");
        }
        await Console.Out.WriteLineAsync();
    }

    static async Task MakeRandomIndex(IElasticClient client)
    {
        // 刪除指定的 Index
        await client.Indices.DeleteAsync(IndexName);

        Random random = new Random();

        var response = await client.IndexAsync<Blog>(new Blog()
        {
            BlogId = random.Next(),
            Title = $"Nice to meet your 999",
            Content = $"Hello Elasticsearch 999",
            CreateAt = DateTime.Now.AddDays(999),
            UpdateAt = DateTime.Now.AddDays(999),
        }, idx => idx.Index(IndexName));
        if (response.IsValid)
        {
            //Console.WriteLine($"Index document with ID {response.Id} succeeded.");
        }
        else
        {
            Console.WriteLine($"Error Message : {response.DebugInformation}");
        }
    }
}
