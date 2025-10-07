namespace EchoChatApi.Services;

using EchoChatApi.Models;
using System.Collections.Generic;

public class MessageService
{
    private readonly string _connectionString = "server=localhost;uid=root;pwd=bit_academy;database=echochat";

    public void SaveMessage(string username, string message)
    {
        using var connection = new MySql.Data.MySqlClient.MySqlConnection(_connectionString);
        connection.Open();

        string query = "INSERT INTO messages (user_id, message, timestamp) VALUES (@user_id, @message, @timestamp)";
        using var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@user_id", username);
        cmd.Parameters.AddWithValue("@message", message);
        cmd.Parameters.AddWithValue("@timestamp", DateTime.UtcNow);

        cmd.ExecuteNonQuery();
    }

    public List<MessageResponse> GetMessages()
    {
        var messages = new List<MessageResponse>();
        using var connection = new MySql.Data.MySqlClient.MySqlConnection(_connectionString);
        connection.Open();
        string query = "SELECT user_id, message, timestamp FROM messages ORDER BY timestamp DESC";
        using var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            messages.Add(new MessageResponse
            {
                UserId = reader.GetInt32("user_id"),
                Message = reader.GetString("message"),
            });
        }
        return messages;
    }
}