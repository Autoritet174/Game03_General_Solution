using Dapper;
using General.DataBaseModels;
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

    public static async Task<bool> IsCorrectEmailPassword(string? email, string? password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return false;
        }
        await using MySqlConnection connection = new(General.DataBase.ConnectionString_UsersData);
        const string sql = """
            SELECT email, password_hash
            FROM users WHERE email = @email LIMIT 1
            """;
        User? qwe = await connection.QueryFirstOrDefaultAsync<User>(sql, new { email});
        



        await using MySqlConnection connection = new(General.DataBase.ConnectionString_UsersData);
       

        return result == 1;
    }
}
