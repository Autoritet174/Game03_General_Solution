using Dapper;
using MySql.Data.MySqlClient;

namespace Server;

public static class GF_DataBase
{
    public static async Task<bool> IsExistsEmail(string? email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return false;
        }
        using MySqlConnection connection = new(General.DataBase.ConnectionString_UsersData);
        await connection.OpenAsync();

        const string query = "SELECT COUNT(1) FROM users WHERE email = @email LIMIT 1";
        int result = await connection.ExecuteScalarAsync<int>(query, new { email });

        return result > 0;
    }

    public static async Task<bool> IsCorrectEmailPassword(string email, string password)
    {
        await using MySqlConnection connection = new(General.DataBase.ConnectionString_UsersData);
        const string sql = """
            SELECT password_hash
            FROM users
            WHERE deleted_at IS NULL AND email = @email
            LIMIT 1
            """;
        dynamic? result = await connection.QueryFirstOrDefaultAsync(sql, new { email });
        if (result != null)
        {
            string? password_hash = result.password_hash;
            return UserRegAuth_NS.Password.Verify(email, password_hash ?? "", password);
        }

        return false;
    }
}
