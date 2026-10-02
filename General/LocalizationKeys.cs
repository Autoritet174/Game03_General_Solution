namespace General;

/// <summary>
/// Статический класс, содержащий строковые ключи для доступа к текстовым ресурсам
/// из файлов локализации (например, JSON).
/// Ключи сгруппированы по контексту.
/// </summary>
public static class LocalizationKeys
{
    /// <summary>Группа ключей, относящихся к элементам пользовательского интерфейса (UI).</summary>
    public static class UI
    {
        private static readonly string uiPrefix = $"{nameof(UI)}.";

        /// <summary>Ключи для текста, отображаемого на кнопках.</summary>
        public static class Button
        {
            private static readonly string buttonPrefix = $"{uiPrefix}{nameof(Button)}.";

            public static readonly string ok = $"{buttonPrefix}{nameof(ok)}";
            public static readonly string yes = $"{buttonPrefix}{nameof(yes)}";
            public static readonly string no = $"{buttonPrefix}{nameof(no)}";
            public static readonly string login = $"{buttonPrefix}{nameof(login)}";
            public static readonly string reg = $"{buttonPrefix}{nameof(reg)}";
            public static readonly string exitGame = $"{buttonPrefix}{nameof(exitGame)}";
            public static readonly string heroes = $"{buttonPrefix}{nameof(heroes)}";
            public static readonly string equipment = $"{buttonPrefix}{nameof(equipment)}";
            public static readonly string changingEquipment = $"{buttonPrefix}{nameof(changingEquipment)}";
            public static readonly string item = $"{buttonPrefix}{nameof(item)}";
            public static readonly string takeOn = $"{buttonPrefix}{nameof(takeOn)}";
            public static readonly string takeOff = $"{buttonPrefix}{nameof(takeOff)}";
            public static readonly string takeOnAlt = $"{buttonPrefix}{nameof(takeOnAlt)}";
            public static readonly string sell = $"{buttonPrefix}{nameof(sell)}";
            public static readonly string showHero = $"{buttonPrefix}{nameof(showHero)}";
            public static readonly string cancel = $"{buttonPrefix}{nameof(cancel)}";
            public static readonly string startBattle = $"{buttonPrefix}{nameof(startBattle)}";

            public static class Ability
            {
                private static readonly string abilityPrefix = $"{buttonPrefix}{nameof(Ability)}.";

                public static readonly string attack = $"{abilityPrefix}{nameof(attack)}";
            }
        }

        /// <summary>Ключи для текста, отображаемого на метках (Label) или заголовках.</summary>
        public static class Label
        {
            private static readonly string labelPrefix = $"{uiPrefix}{nameof(Label)}.";

            public static readonly string exitGame = $"{labelPrefix}{nameof(exitGame)}";
            public static readonly string email = $"{labelPrefix}{nameof(email)}";
            public static readonly string password = $"{labelPrefix}{nameof(password)}";

            public static readonly string noGroup = $"{labelPrefix}{nameof(noGroup)}";
            public static readonly string endBattle = $"{labelPrefix}{nameof(endBattle)}";

            public static readonly string battlefield = $"{labelPrefix}{nameof(battlefield)}";
            public static readonly string connectionLost = $"{labelPrefix}{nameof(connectionLost)}";
            public static readonly string reconnecting = $"{labelPrefix}{nameof(reconnecting)}";
            public static readonly string @try = $"{labelPrefix}{nameof(@try)}";
            public static readonly string after = $"{labelPrefix}{nameof(after)}";
            public static readonly string turn = $"{labelPrefix}{nameof(turn)}";
            public static readonly string dead = $"{labelPrefix}{nameof(dead)}";
            public static readonly string damage = $"{labelPrefix}{nameof(damage)}";
            public static readonly string mainStat = $"{labelPrefix}{nameof(mainStat)}";
            public static readonly string rarity = $"{labelPrefix}{nameof(rarity)}";

            public static readonly string characteristic = $"{labelPrefix}{nameof(characteristic)}";
            public static readonly string expectedValue = $"{labelPrefix}{nameof(expectedValue)}";
            public static readonly string dice = $"{labelPrefix}{nameof(dice)}";
            public static readonly string range = $"{labelPrefix}{nameof(range)}";

            public static class Slot
            {
                private static readonly string slotsPrefix = $"{labelPrefix}{nameof(Slot)}.";
                public static string GetKey(string slot)
                {
                    return $"{slotsPrefix}{slot}";
                }

                public static readonly string head = $"{slotsPrefix}{nameof(head)}";
                public static readonly string armor = $"{slotsPrefix}{nameof(armor)}";
                public static readonly string hands = $"{slotsPrefix}{nameof(hands)}";
                public static readonly string feet = $"{slotsPrefix}{nameof(feet)}";
                public static readonly string waist = $"{slotsPrefix}{nameof(waist)}";
                public static readonly string weapon = $"{slotsPrefix}{nameof(weapon)}";
                public static readonly string weaponShield = $"{slotsPrefix}{nameof(weaponShield)}";
                public static readonly string neck = $"{slotsPrefix}{nameof(neck)}";
                public static readonly string ring = $"{slotsPrefix}{nameof(ring)}";
                public static readonly string trinket = $"{slotsPrefix}{nameof(trinket)}";
            }

            public static class Stat
            {
                private static readonly string statPrefix = $"{labelPrefix}{nameof(Stat)}.";
                public static string GetKey(string stat)
                {
                    return $"{statPrefix}{stat}";
                }

                public static readonly string level = $"{statPrefix}{nameof(level)}";
                public static readonly string health = $"{statPrefix}{nameof(health)}";
                public static readonly string strength = $"{statPrefix}{nameof(strength)}";
                public static readonly string agility = $"{statPrefix}{nameof(agility)}";
                public static readonly string intelligence = $"{statPrefix}{nameof(intelligence)}";
                public static readonly string critChance = $"{statPrefix}{nameof(critChance)}";
                public static readonly string critMultiplier = $"{statPrefix}{nameof(critMultiplier)}";
                public static readonly string versality = $"{statPrefix}{nameof(versality)}";
                public static readonly string initiative = $"{statPrefix}{nameof(initiative)}";
                public static readonly string haste = $"{statPrefix}{nameof(haste)}";
                public static readonly string damage = $"{statPrefix}{nameof(damage)}";
                public static readonly string universal = $"{statPrefix}{nameof(universal)}";
                public static readonly string endurancePhysical = $"{statPrefix}{nameof(endurancePhysical)}";
                public static readonly string enduranceMagical = $"{statPrefix}{nameof(enduranceMagical)}";
            }
            public static class Rarity
            {
                private static readonly string rarityPrefix = $"{labelPrefix}{nameof(Rarity)}.";
                public static string GetKey(string rarity)
                {
                    return $"{rarityPrefix}{rarity}";
                }

                public static readonly string r1 = $"{rarityPrefix}{nameof(r1)}";
                public static readonly string r2 = $"{rarityPrefix}{nameof(r2)}";
                public static readonly string r3 = $"{rarityPrefix}{nameof(r3)}";
                public static readonly string r4 = $"{rarityPrefix}{nameof(r4)}";
                public static readonly string r5 = $"{rarityPrefix}{nameof(r5)}";
            }
        }
    }

    /// <summary>Группа ключей, относящихся к сообщениям об ошибках.</summary>
    public static class Error
    {
        private static readonly string errorPrefix = $"{nameof(Error)}.";

        public static readonly string unknownError = $"{errorPrefix}{nameof(unknownError)}";

        /// <summary>Ключи для ошибок, связанных с взаимодействием с сервером.</summary>
        public static class Server
        {
            private static readonly string serverPrefix = $"{errorPrefix}{nameof(Server)}.";

            public static readonly string timeout = $"{serverPrefix}{nameof(timeout)}";
            public static readonly string invalidRequest = $"{serverPrefix}{nameof(invalidRequest)}";
            public static readonly string invalidResponse = $"{serverPrefix}{nameof(invalidResponse)}";
            public static readonly string invalidCredentials = $"{serverPrefix}{nameof(invalidCredentials)}";
            public static readonly string tooManyRequests = $"{serverPrefix}{nameof(tooManyRequests)}";
            public static readonly string accountBannedUntil = $"{serverPrefix}{nameof(accountBannedUntil)}";
            public static readonly string accountBannedPermanently = $"{serverPrefix}{nameof(accountBannedPermanently)}";
            public static readonly string unavailable = $"{serverPrefix}{nameof(unavailable)}";
            public static readonly string noInternetConnection = $"{serverPrefix}{nameof(noInternetConnection)}";
            public static readonly string openingWebSocketFailed = $"{serverPrefix}{nameof(openingWebSocketFailed)}";
            public static readonly string loadingCollectionFailed = $"{serverPrefix}{nameof(loadingCollectionFailed)}";
            public static readonly string userAlreadyExists = $"{serverPrefix}{nameof(userAlreadyExists)}";
            public static readonly string required2FA = $"{serverPrefix}{nameof(required2FA)}";
            public static readonly string refreshTokenErrorCreating = $"{serverPrefix}{nameof(refreshTokenErrorCreating)}";
            public static readonly string combatBreak = $"{serverPrefix}{nameof(combatBreak)}";
        }

        /// <summary>Ключи для ошибок, связанных с некорректным вводом данных пользователем.</summary>
        public static class User
        {
            private static readonly string userPrefix = $"{errorPrefix}{nameof(User)}.";

            public static readonly string notEmail = $"{userPrefix}{nameof(notEmail)}";
            public static readonly string emailEmpty = $"{userPrefix}{nameof(emailEmpty)}";
            public static readonly string passwordEmpty = $"{userPrefix}{nameof(passwordEmpty)}";
        }
    }

    /// <summary>Группа ключей, относящихся к информационным сообщениям (например, о статусе).</summary>
    public static class Info
    {
        private static readonly string infoPrefix = $"{nameof(Info)}.";

        public static readonly string authentication = $"{infoPrefix}{nameof(authentication)}";
        public static readonly string authenticationSuccess = $"{infoPrefix}{nameof(authenticationSuccess)}";
        public static readonly string openingWebSocket = $"{infoPrefix}{nameof(openingWebSocket)}";
        public static readonly string loadingData = $"{infoPrefix}{nameof(loadingData)}";
        public static readonly string loadingCollection = $"{infoPrefix}{nameof(loadingCollection)}";
        public static readonly string checkingServerAvailability = $"{infoPrefix}{nameof(checkingServerAvailability)}";
        public static readonly string selectHero = $"{infoPrefix}{nameof(selectHero)}";
    }


    /// <summary>
    /// Специальный ключ-маркер для автоматического определения строк,
    /// которые должны быть получены из системы локализации.
    /// </summary>
    public static readonly string keyLocalization = nameof(keyLocalization);

    /// <summary>Ключ-заполнитель для указания даты и времени истечения срока.</summary>
    public static readonly string datetimeExpiration = nameof(datetimeExpiration);

    /// <summary>Ключ-заполнитель для указания оставшегося времени (в формате времени).</summary>
    public static readonly string timeRemaining = nameof(timeRemaining);

    /// <summary>Ключ-заполнитель для указания оставшегося времени (в секундах).</summary>
    public static readonly string secondsRemaining = nameof(secondsRemaining);
}
