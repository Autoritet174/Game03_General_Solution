namespace General;

/// <summary>
/// Коды для получения текстовых ошибок из json файлов локализации.
/// </summary>
public static class LocalizationKeys
{

    public static class UI
    {
        private const string _UI = $"{nameof(UI)}.";

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
        public static class Label
        {
            private const string _Label = $"{_UI}{nameof(Label)}.";

            public const string ExitGame = $"{_Label}{nameof(ExitGame)}";
            public const string Email = $"{_Label}{nameof(Email)}";
            public const string Password = $"{_Label}{nameof(Password)}";
        }
    }

    public static class Error {
        private const string _Error = $"{nameof(Error)}.";

        public const string UnknownError = $"{_Error}{nameof(UnknownError)}";
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
        public static class User {
            private const string _User = $"{_Error}{nameof(User)}.";

            public const string NotEmail = $"{_User}{nameof(NotEmail)}";
            public const string EmailEmpty = $"{_User}{nameof(EmailEmpty)}";
            public const string PasswordEmpty = $"{_User}{nameof(PasswordEmpty)}";
        }
    }

    public static class Info
    {
        private const string _Info = $"{nameof(Info)}.";

        public const string Authentication = $"{_Info}{nameof(Authentication)}";
        public const string AuthenticationSuccess = $"{_Info}{nameof(AuthenticationSuccess)}";
        public const string OpeningWebSocket = $"{_Info}{nameof(OpeningWebSocket)}";
        public const string LoadingData = $"{_Info}{nameof(LoadingData)}";
    }


    /// <summary>
    /// Константа для отметки строк как те что можно считать из локализции.
    /// </summary>
    public const string KEY_LOCALIZATION = nameof(KEY_LOCALIZATION);
    public const string DATE_TIME_EXPIRES_AT = nameof(DATE_TIME_EXPIRES_AT);
    public const string DATETIME_EXPIRATION = nameof(DATETIME_EXPIRATION);
    public const string TIME_REMAINING = nameof(TIME_REMAINING);
    public const string SECONDS_REMAINING = nameof(SECONDS_REMAINING);
}
