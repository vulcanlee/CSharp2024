using SharpToken;

namespace csComputeToken;

internal class Program
{
    static void Main(string[] args)
    {
        // https://platform.openai.com/tokenizer
        string content = "Hello, World!";
        Console.WriteLine(content);
        var encoding = GptEncoding.GetEncodingForModel("gpt-4");
        var encoded = encoding.Encode(content); // Output: [9906, 11, 1917, 0]
        var decoded = encoding.Decode(encoded); // Output: "Hello, world!"
        var count = encoding.CountTokens("Hello, world!"); // Output: 4

        ShowInformation(encoded, decoded, count);

        content = "你好，世界!";
        Console.WriteLine(new string('-',40));
        Console.WriteLine(content);
         encoded = encoding.Encode(content); 
         decoded = encoding.Decode(encoded);
         count = encoding.CountTokens("Hello, world!"); 

        ShowInformation(encoded, decoded, count);

    }

    private static void ShowInformation(List<int> encoded, string decoded, int count)
    {
        string indexText = encoded.Select(x => x.ToString()).Aggregate((x, y) => $"{x}, {y}");
        Console.WriteLine($"Encode: {indexText}");
        Console.WriteLine($"Decode: {decoded}");
        Console.WriteLine($"CountTokens: {count}");
    }
}
