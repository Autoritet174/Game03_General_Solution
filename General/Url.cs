namespace General;

/// <summary>
/// Класс предоставляющий ссылки на API сервера.
/// </summary>
public static class Url
{
    /// <summary>
    /// URL API старт
    /// </summary>
    private const string URL_HEADER = "https://localhost:7227/api/";

    public const string PING = $"{URL_HEADER}ping";

    public const string AUTH_LOGIN = $"{URL_HEADER}auth/login";
    public const string AUTH_REFRESH_TOKENS = $"{URL_HEADER}session/refresh";
    public const string AUTH_VALIDATE = $"{URL_HEADER}auth/validate";
    public const string REG = $"{URL_HEADER}reg";

    public static class Collection
    {
        private const string COLLECTION_NAME = $"{URL_HEADER}{nameof(Collection)}/";
        public const string ALL = $"{COLLECTION_NAME}all";
        //public static string Heroes { get; private set; } = $"{_Collection}{nameof(Heroes)}";
        //public static string Items { get; private set; } = $"{_Collection}{nameof(Items)}";
    }

    public const string TEST = $"{URL_HEADER}test";

    public static class General
    {
        private const string GENERAL_NAME = $"{URL_HEADER}{nameof(General)}/";
    }

    /// <summary> URL. Все игровые данные нужные на клиенте игры. </summary>
    public const string GAME_DATA = $"{URL_HEADER}gameData";
}
