using System;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;

namespace EchoChatApi.Services;

public class AuthService
{
    private readonly string _connectionString = "server=localhost;uid=root;pwd=bit_academy;database=echochat";

    public bool Login(string email, string password)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        string query = "SELECT password FROM users WHERE email = @email";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@email", email);

        var storedHash = cmd.ExecuteScalar() as string;
        if (string.IsNullOrEmpty(storedHash))
            return false;

        return VerifyPassword(password, storedHash);
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

        // Hash the password (no salt, per request)
        string hashedPassword = HashPassword(password);

        // Insert new user
        string insertQuery = "INSERT INTO users (email, username, password) VALUES (@email, @username, @password)";
        using var insertCmd = new MySqlCommand(insertQuery, connection);
        insertCmd.Parameters.AddWithValue("@email", email);
        insertCmd.Parameters.AddWithValue("@username", username);
        insertCmd.Parameters.AddWithValue("@password", hashedPassword);
        insertCmd.ExecuteNonQuery();

        return true;
    }

    protected string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        var hashedInput = HashPassword(password);
        return string.Equals(hashedInput, storedHash, StringComparison.Ordinal);
    }
}
