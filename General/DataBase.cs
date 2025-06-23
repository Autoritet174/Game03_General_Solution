namespace General;
public static class DataBase
{
    public static string ConnectionString_GameData { get; set; } = "server=127.127.126.12;port=3306;database=Game03_GameData;user=root;password=;SslMode=none;charset=utf8mb4";
    public static string ConnectionString_UsersData { get; set; } = "server=127.127.126.12;port=3306;database=Game03_UsersData;user=root;password=;SslMode=none;charset=utf8mb4";
}
