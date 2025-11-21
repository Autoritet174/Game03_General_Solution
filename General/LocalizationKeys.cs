namespace General;

/// <summary>
/// Статический класс, содержащий строковые ключи для доступа к текстовым ресурсам
/// из файлов локализации (например, JSON).
/// Ключи сгруппированы по контексту: UI, Ошибки, Информация.
/// </summary>
public static class LocalizationKeys
{
#pragma warning disable

    /// <summary>
    /// Группа ключей, относящихся к элементам пользовательского интерфейса (UI).
    /// </summary>
    public static class UI
    {
        private const string _UI = $"{nameof(UI)}.";

        /// <summary>
        /// Ключи для текста, отображаемого на кнопках.
        /// </summary>
        public static class Button
        {
            private const string _Button = $"{_UI}{nameof(Button)}.";

            public const string Ok = $"{_Button}{nameof(Ok)}";
            public const string Yes = $"{_Button}{nameof(Yes)}";
            public const string No = $"{_Button}{nameof(No)}";
            public const string Login = $"{_Button}{nameof(Login)}";
            public const string Reg = $"{_Button}{nameof(Reg)}";
            public const string ExitGame = $"{_Button}{nameof(ExitGame)}";
        }

        /// <summary>
        /// Ключи для текста, отображаемого на метках (Label) или заголовках.
        /// </summary>
        public static class Label
        {
            private const string _Label = $"{_UI}{nameof(Label)}.";

            public const string ExitGame = $"{_Label}{nameof(ExitGame)}";
            public const string Email = $"{_Label}{nameof(Email)}";
            public const string Password = $"{_Label}{nameof(Password)}";
        }
    }

    /// <summary>
    /// Группа ключей, относящихся к сообщениям об ошибках.
    /// </summary>
    public static class Error {
        private const string _Error = $"{nameof(Error)}.";

        public const string UnknownError = $"{_Error}{nameof(UnknownError)}";

        /// <summary>
        /// Ключи для ошибок, связанных с взаимодействием с сервером.
        /// </summary>
        public static class Server {
            private const string _Server = $"{_Error}{nameof(Server)}.";

            public const string Timeout = $"{_Server}{nameof(Timeout)}";
            public const string InvalidResponse = $"{_Server}{nameof(InvalidResponse)}";
            public const string InvalidCredentials = $"{_Server}{nameof(InvalidCredentials)}";
            public const string TooManyRequests = $"{_Server}{nameof(TooManyRequests)}";
            public const string AccountBannedUntil = $"{_Server}{nameof(AccountBannedUntil)}";
            public const string AccountBannedPermanently = $"{_Server}{nameof(AccountBannedPermanently)}";
            public const string Unavailable = $"{_Server}{nameof(Unavailable)}";
            public const string NoInternetConnection = $"{_Server}{nameof(NoInternetConnection)}";
            public const string OpeningWebSocketFailed = $"{_Server}{nameof(OpeningWebSocketFailed)}";
        }

        /// <summary>
        /// Ключи для ошибок, связанных с некорректным вводом данных пользователем.
        /// </summary>
        public static class User {
            private const string _User = $"{_Error}{nameof(User)}.";

            public const string NotEmail = $"{_User}{nameof(NotEmail)}";
            public const string EmailEmpty = $"{_User}{nameof(EmailEmpty)}";
            public const string PasswordEmpty = $"{_User}{nameof(PasswordEmpty)}";
        }
    }

    /// <summary>
    /// Группа ключей, относящихся к информационным сообщениям (например, о статусе).
    /// </summary>
    public static class Info
    {
        private const string _Info = $"{nameof(Info)}.";

        public const string Authentication = $"{_Info}{nameof(Authentication)}";
        public const string AuthenticationSuccess = $"{_Info}{nameof(AuthenticationSuccess)}";
        public const string OpeningWebSocket = $"{_Info}{nameof(OpeningWebSocket)}";
        public const string LoadingData = $"{_Info}{nameof(LoadingData)}";
    }


    /// <summary>
    /// Специальный ключ-маркер для автоматического определения строк,
    /// которые должны быть получены из системы локализации.
    /// </summary>
    public const string KEY_LOCALIZATION = nameof(KEY_LOCALIZATION);

    /// <summary>
    /// Ключ-заполнитель для указания даты и времени истечения срока действия.
    /// </summary>
    public const string DATE_TIME_EXPIRES_AT = nameof(DATE_TIME_EXPIRES_AT);

    /// <summary>
    /// Ключ-заполнитель для указания даты и времени истечения срока.
    /// </summary>
    public const string DATETIME_EXPIRATION = nameof(DATETIME_EXPIRATION);

    /// <summary>
    /// Ключ-заполнитель для указания оставшегося времени (в формате времени).
    /// </summary>
    public const string TIME_REMAINING = nameof(TIME_REMAINING);

    /// <summary>
    /// Ключ-заполнитель для указания оставшегося времени (в секундах).
    /// </summary>
    public const string SECONDS_REMAINING = nameof(SECONDS_REMAINING);
}
