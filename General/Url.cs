namespace General;

/// <summary>
/// Класс предоставляющий ссылки на API сервера.
/// </summary>
public static class Url
{
    public static string urlDomain { get; private set; } = string.Empty;
    public static string ping { get; private set; } = string.Empty;
    public static string authLogin { get; private set; } = string.Empty;
    public static string authRefreshTokens { get; private set; } = string.Empty;
    public static string authValidate { get; private set; } = string.Empty;
    public static string reg { get; private set; } = string.Empty;
    public static string test { get; private set; } = string.Empty;

    /// <summary> URL. Все игровые данные нужные на клиенте игры. </summary>
    public static string gameData { get; private set; } = string.Empty;

    public static class Collection
    {
        private static string collectionName = string.Empty;
        public static string all { get; private set; } = string.Empty;
        public static void Init(string urlHeader)
        {
            collectionName = $"{urlHeader}{nameof(Collection)}/";
            all = $"{collectionName}all";
        }
        //public static string Heroes { get; private set; } = $"{_Collection}{nameof(Heroes)}";
        //public static string Items { get; private set; } = $"{_Collection}{nameof(Items)}";
    }

    public static class General
    {
        private static string generalName = string.Empty;
        public static void Init(string urlHeader)
        {
            generalName = $"{urlHeader}{nameof(General)}/";
        }
    }

    public static void Init(string urlDomain)
    {
        Url.urlDomain = urlDomain;
        string urlHeader = $"{urlDomain}/api/";
        ping = $"{urlHeader}ping";
        authLogin = $"{urlHeader}auth/login";
        authRefreshTokens = $"{urlHeader}session/refresh";
        authValidate = $"{urlHeader}auth/validate";
        reg = $"{urlHeader}reg";
        test = $"{urlHeader}test";
        gameData = $"{urlHeader}gameData";
        Collection.Init(urlHeader);
        General.Init(urlHeader);
    }
}
