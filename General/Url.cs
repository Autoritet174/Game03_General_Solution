namespace General;

/// <summary>
/// Класс предоставляющий ссылки на API сервера.
/// </summary>
public static class Url
{
    public static string UrlDomain { get; private set; } = string.Empty;
    public static string PING { get; private set; } = string.Empty;
    public static string AUTH_LOGIN { get; private set; } = string.Empty;
    public static string AUTH_REFRESH_TOKENS { get; private set; } = string.Empty;
    public static string AUTH_VALIDATE { get; private set; } = string.Empty;
    public static string REG { get; private set; } = string.Empty;
    public static string TEST { get; private set; } = string.Empty;

    /// <summary> URL. Все игровые данные нужные на клиенте игры. </summary>
    public static string GAME_DATA { get; private set; } = string.Empty;

    public static class Collection
    {
        private static string COLLECTION_NAME = string.Empty;
        public static string ALL { get; private set; } = string.Empty;
        public static void Init(string urlHeader)
        {
            COLLECTION_NAME = $"{urlHeader}{nameof(Collection)}/";
            ALL = $"{COLLECTION_NAME}all";
        }
        //public static string Heroes { get; private set; } = $"{_Collection}{nameof(Heroes)}";
        //public static string Items { get; private set; } = $"{_Collection}{nameof(Items)}";
    }

    public static class General
    {
        private static string GENERAL_NAME = string.Empty;
        public static void Init(string urlHeader)
        {
            GENERAL_NAME = $"{urlHeader}{nameof(General)}/";
        }
    }

    public static void Init(string urlDomain)
    {
        UrlDomain = urlDomain;
        string urlHeader = $"{urlDomain}/api/";
        PING = $"{urlHeader}ping";
        AUTH_LOGIN = $"{urlHeader}auth/login";
        AUTH_REFRESH_TOKENS = $"{urlHeader}session/refresh";
        AUTH_VALIDATE = $"{urlHeader}auth/validate";
        REG = $"{urlHeader}reg";
        TEST = $"{urlHeader}test";
        GAME_DATA = $"{urlHeader}gameData";
        Collection.Init(urlHeader);
        General.Init(urlHeader);
    }
}
