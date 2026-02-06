using Newtonsoft.Json;
using static General.LocalizationKeys.UI.Label;

namespace General;

/// <summary>
/// Статический класс, содержащий строковые ключи для доступа к текстовым ресурсам
/// из файлов локализации (например, JSON).
/// Ключи сгруппированы по контексту.
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
            public const string Heroes = $"{_Button}{nameof(Heroes)}";
            public const string Equipment = $"{_Button}{nameof(Equipment)}";
            public const string ChangingEquipment = $"{_Button}{nameof(ChangingEquipment)}";
            public const string Item = $"{_Button}{nameof(Item)}";
            public const string TakeOn = $"{_Button}{nameof(TakeOn)}";
            public const string TakeOff = $"{_Button}{nameof(TakeOff)}";
            public const string Sell = $"{_Button}{nameof(Sell)}";
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

            public static class Slot
            {
                private const string _Slots = $"{_Label}{nameof(Slot)}.";
                public static string GetKey(string slot) => $"{_Slots}{slot}";

                public const string Head = $"{_Slots}{nameof(Head)}";
                public const string Armor = $"{_Slots}{nameof(Armor)}";
                public const string Hands = $"{_Slots}{nameof(Hands)}";
                public const string Feet = $"{_Slots}{nameof(Feet)}";
                public const string Waist = $"{_Slots}{nameof(Waist)}";
                public const string Weapon = $"{_Slots}{nameof(Weapon)}";
                public const string WeaponShield = $"{_Slots}{nameof(WeaponShield)}";
                public const string Neck = $"{_Slots}{nameof(Neck)}";
                public const string Ring = $"{_Slots}{nameof(Ring)}";
                public const string Trinket = $"{_Slots}{nameof(Trinket)}";
            }
            public const string NoGroup = $"{_Label}{nameof(NoGroup)}";
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
            public const string InvalidRequest = $"{_Server}{nameof(InvalidRequest)}";
            public const string InvalidResponse = $"{_Server}{nameof(InvalidResponse)}";
            public const string InvalidCredentials = $"{_Server}{nameof(InvalidCredentials)}";
            public const string TooManyRequests = $"{_Server}{nameof(TooManyRequests)}";
            public const string AccountBannedUntil = $"{_Server}{nameof(AccountBannedUntil)}";
            public const string AccountBannedPermanently = $"{_Server}{nameof(AccountBannedPermanently)}";
            public const string Unavailable = $"{_Server}{nameof(Unavailable)}";
            public const string NoInternetConnection = $"{_Server}{nameof(NoInternetConnection)}";
            public const string OpeningWebSocketFailed = $"{_Server}{nameof(OpeningWebSocketFailed)}";
            public const string LoadingCollectionFailed = $"{_Server}{nameof(LoadingCollectionFailed)}";
            public const string UserAlreadyExists = $"{_Server}{nameof(UserAlreadyExists)}";
            public const string Required2FA = $"{_Server}{nameof(Required2FA)}";
            public const string RefreshTokenErrorCreating = $"{_Server}{nameof(RefreshTokenErrorCreating)}";
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
        public const string LoadingCollection = $"{_Info}{nameof(LoadingCollection)}";
        public const string CheckingServerAvailability = $"{_Info}{nameof(CheckingServerAvailability)}";
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
