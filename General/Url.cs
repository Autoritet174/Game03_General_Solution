using static General.Url;

namespace General;

/// <summary>
/// Класс предоставляющий ссылки на api сервера.
/// </summary>
public static class Url
{
#pragma warning disable

    /// <summary>
    /// URL API старт
    /// </summary>
    private const string URL_HEADER = "https://localhost:7227/api/";

    public static string Ping { get; private set; } = $"{URL_HEADER}{nameof(Ping)}";

    public static string Auth { get; private set; } = $"{URL_HEADER}{nameof(Auth)}";
    public static string Reg { get; private set; } = $"{URL_HEADER}{nameof(Reg)}";

    public static class Collection {
        private const string _Collection = $"{URL_HEADER}{nameof(Collection)}/";
        public static string All { get; private set; } = $"{_Collection}{nameof(All)}";
        //public static string Heroes { get; private set; } = $"{_Collection}{nameof(Heroes)}";
        //public static string Items { get; private set; } = $"{_Collection}{nameof(Items)}";
    }

    public static string Test { get; private set; } = $"{URL_HEADER}{nameof(Test)}";

    public static class General
    {
        private const string _General = $"{URL_HEADER}{nameof(General)}/";
    }

    /// <summary> URL. Все игровые данные нужные на клиенте игры. </summary>
    public static string GameData { get; private set; } = $"{URL_HEADER}{nameof(GameData)}";
}
