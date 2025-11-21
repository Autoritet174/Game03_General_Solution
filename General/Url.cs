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


    public static string Authentication { get; private set; } = $"{URL_HEADER}{nameof(Authentication)}";
    public static string Registration { get; private set; } = $"{URL_HEADER}{nameof(Registration)}";

    public static class Collection {
        private const string _Collection = $"{URL_HEADER}{nameof(Collection)}/";
        public static string Heroes { get; private set; } = $"{_Collection}{nameof(Heroes)}";
        public static string Items { get; private set; } = $"{_Collection}{nameof(Items)}";
    }

    public static string Test { get; private set; } = $"{URL_HEADER}{nameof(Test)}";

    public static class General
    {
        private const string _General = $"{URL_HEADER}{nameof(General)}/";
        public static string ListAllHeroes { get; private set; } = $"{_General}{nameof(ListAllHeroes)}";
    }
}
