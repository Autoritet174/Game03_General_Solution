using Dapper;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using Server.DB.Users.Repositories;

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

}
