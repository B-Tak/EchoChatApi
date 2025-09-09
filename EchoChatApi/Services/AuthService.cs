using MySql.Data.MySqlClient;

namespace EchoChatApi.Services;

public class AuthService
{
    private readonly string _connectionString = "server=localhost;uid=root;pwd=bit_academy;database=echochat";

    public bool Login(string email, string password)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        string query = "SELECT COUNT(*) FROM users WHERE email = @email AND password = @password";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@password", password);

        var result = Convert.ToInt32(cmd.ExecuteScalar());
        return result > 0;
    }

    public bool Register(string email, string username, string password)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        // Check if username exists
        string checkQuery = "SELECT COUNT(*) FROM users WHERE username = @username";
        using var checkCmd = new MySqlCommand(checkQuery, connection);
        checkCmd.Parameters.AddWithValue("@username", username);
        if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
            return false;

        // Insert new user
        string insertQuery = "INSERT INTO users (email, username, password) VALUES (@email, @username, @password)";
        using var insertCmd = new MySqlCommand(insertQuery, connection);
        insertCmd.Parameters.AddWithValue("@email", email);
        insertCmd.Parameters.AddWithValue("@username", username);
        insertCmd.Parameters.AddWithValue("@password", password);
        insertCmd.ExecuteNonQuery();

        return true;
    }

    protected string HashPassword(string password)
    {
        // Implement password hashing here
        return password; // Placeholder, do not use in production
    }
}