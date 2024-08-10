using Dapper;
using Microsoft.Data.SqlClient;

namespace csDapperLocalDB;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=DapperDemo;Trusted_Connection=True;";

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // 新增資料
            string insertQuery = "INSERT INTO Users (Name, Email) VALUES (@Name, @Email)";
            var user = new { Name = "John Doe", Email = "john.doe@example.com" };
            connection.Execute(insertQuery, user);

            // 查詢資料
            string selectQuery = "SELECT * FROM Users";
            var users = connection.Query<User>(selectQuery);

            foreach (var u in users)
            {
                Console.WriteLine($"Id: {u.Id}, Name: {u.Name}, Email: {u.Email}");
            }
        }
    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}