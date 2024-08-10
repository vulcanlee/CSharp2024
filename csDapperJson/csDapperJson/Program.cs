using Dapper;
using Microsoft.Data.SqlClient;

namespace csDapperJson;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=DapperDemo;Trusted_Connection=True;";

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // 多筆資料插入
            string insertQuery = @"
                    INSERT INTO Users (Name, Email, AdditionalData) 
                    VALUES (@Name, @Email, @AdditionalData)";

            var users = new List<dynamic>
                {
                    new { Name = "John Doe", Email = "john.doe@example.com", AdditionalData = "{\"Age\":30,\"Country\":\"USA\"}" },
                    new { Name = "Jane Smith", Email = "jane.smith@example.com", AdditionalData = "{\"Age\":25,\"City\":\"New York\"}" },
                    new { Name = "Sam Brown", Email = "sam.brown@example.com", AdditionalData = "{\"Occupation\":\"Engineer\",\"Country\":\"Canada\"}" }
                };

            connection.Execute(insertQuery, users);

            // 查詢資料，根據 Name 和 JSON 中的 Country 屬性
            string selectQuery = @"
                    SELECT * 
                    FROM Users 
                    WHERE Name = @Name AND JSON_VALUE(AdditionalData, '$.Country') = @Country";

            var result = connection.Query<User>(
                selectQuery, new { Name = "Sam Brown", Country = "Canada" });

            foreach (var u in result)
            {
                Console.WriteLine($"Id: {u.Id}, Name: {u.Name}, Email: {u.Email}, AdditionalData: {u.AdditionalData}");
            }
        }
    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string AdditionalData { get; set; }
}