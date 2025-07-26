namespace Server.DB
{
    public static class UUIDv7
    {

        /// <summary>
        /// Генерирует UUIDv7
        /// </summary>
        /// <returns></returns>
        public static Guid Generate()
        {
            return UUIDNext.Uuid.NewDatabaseFriendly(UUIDNext.Database.PostgreSql);
        }

        /// <summary>
        /// Получаем[DateTimeOffset из UUIDv7
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static DateTimeOffset GetTimestampFromUuidV7(Guid uuid)
        {
            byte[] bytes = uuid.ToByteArray();

            // Проверяем версию UUID (должна быть 7)
            if (IsUuidV7(uuid))
            {
                throw new ArgumentException("The provided GUID is not a UUIDv7", nameof(uuid));
            }

            // Извлекаем 48-битную временную метку (первые 6 байт)
            byte[] timestampBytes = new byte[8];
            Array.Copy(bytes, 0, timestampBytes, 2, 6);

            // Конвертируем в big-endian (если система little-endian)
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes);
            }

            // Получаем миллисекунды (игнорируем первые 2 байта)
            long milliseconds = BitConverter.ToInt64(timestampBytes, 0) >> 16;

            // Используем встроенное свойство UnixEpoch
            return DateTimeOffset.UnixEpoch.AddMilliseconds(milliseconds);
        }

        public static bool IsUuidV7(Guid uuid)
        {
            byte[] bytes = uuid.ToByteArray();
            return (bytes[6] >> 4) == 0x7;
        }
    }
}
