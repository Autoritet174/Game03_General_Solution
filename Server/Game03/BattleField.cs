using MySql.Data.MySqlClient;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace General;
public class BattleField {
    private readonly string _connectionString = "Server=127.0.0.1;Database=Game03;User=root;Password=;SslMode=none";

    //public async Task<IEnumerable<User>> GetUsersAsync() {
    //    using (var connection = new MySqlConnection(_connectionString)) {
    //        await connection.OpenAsync();
    //        return await connection.QueryAsync<User>("SELECT * FROM Users");
    //    }
    //}
}
