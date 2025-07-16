namespace Server.DB.Users;
public static class UtilitiesFunctions
{
    public static string GetConnectionString()
    {
        return "Host=127.127.126.5;Port=5432;Database=Game03_Users;Username=postgres;Password=";
    }
    public static bool TestConnectionWithDataBase()
    {
        try
        {
            Db db = new();
            _ = db.Users.FirstOrDefault();
            return true;
        }
        catch { 
            return false;
        }
    }
}
