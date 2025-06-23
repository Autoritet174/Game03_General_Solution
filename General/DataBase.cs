namespace General;
public static class DataBase
{
    private static readonly string connPart1 = "server=127.127.126.12;port=3306;";
    private static readonly string connPart2 = "user=root;password=;SslMode=none;charset=utf8mb4";
    public static string ConnectionString_GameData { get; set; } = $"{connPart1}database=Game03_GameData;{connPart2}";
    public static string ConnectionString_UsersData { get; set; } = $"{connPart1}database=Game03_UsersData;{connPart2}";
}
