using General.DTO.Interfaces;

namespace Server_DB_Postgres.Entities.Users;

/// <summary> Представляет пользователя системы. </summary>
public class UserDevice : ICreatedAt
{
    public Guid id { get; init; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset createdAt { get; set; }

    /// <summary> Получает или задаёт имя пользователя операционной системы. </summary>
    /// <value>Имя пользователя, под которым запущено приложение (например, 'JohnDoe').</value>
    public string? systemEnvironmentUserName { get; set; }

    /// <summary> Получает или задаёт смещение локального часового пояса от UTC в минутах. </summary>
    /// <value>Целое число, представляющее разницу во времени (например, +180 для UTC+3).</value>
    public int? timeZoneMinutes { get; set; }

    /// <summary> Получает или задаёт уникальный идентификатор устройства. </summary>
    /// <value>Уникальная строка, идентифицирующая устройство (например, IMEI, UUID).</value>
    public string? deviceUniqueIdentifier { get; set; }

    /// <summary> Получает или задаёт модель устройства. </summary>
    /// <value>Описание аппаратной модели устройства.</value>
    public string? deviceModel { get; set; }

    /// <summary> Получает или задаёт тип устройства. </summary>
    /// <value>Категория устройства: мобильное, настольное, игровая консоль и т.д.</value>
    public string? deviceType { get; set; }

    /// <summary> Получает или задаёт операционную систему устройства. </summary>
    /// <value>Название и версия ОС (например, 'Windows 10', 'iOS 17').</value>
    public string? operatingSystem { get; set; }

    /// <summary> Получает или задаёт тип процессора устройства. </summary>
    /// <value>Архитектура и модель процессора (например, 'Intel Core i7', 'Apple M1').</value>
    public string? processorType { get; set; }

    /// <summary> Получает или задаёт количество логических ядер процессора. </summary>
    /// <value>Число, равное количеству потоков процессора.</value>
    public int? processorCount { get; set; }

    /// <summary> Получает или задаёт объём оперативной памяти устройства в мегабайтах. </summary>
    /// <value>Размер ОЗУ в мегабайтах (например, 16384 для 16 ГБ).</value>
    public int? systemMemorySize { get; set; }

    /// <summary> Получает или задаёт название графического устройства. </summary>
    /// <value>Модель видеокарты или встроенного GPU.</value>
    public string? graphicsDeviceName { get; set; }

    /// <summary> Получает или задаёт объём видеопамяти в мегабайтах. </summary>
    /// <value>Размер видеопамяти (VRAM) в МБ.</value>
    public int? graphicsMemorySize { get; set; }

    /// <summary> Получает или задаёт признак поддержки графической системой инстансинга. </summary>
    /// <value><c>true</c>, если поддерживается; иначе <c>false</c>.</value>
    public bool? systemInfoSupportsInstancing { get; set; }

    /// <summary> Получает или задаёт информацию о поддержке текстур с размерами, не являющимися степенью двойки (NPOT). </summary>
    /// <value>Строка, описывающая уровень поддержки NPOT (например, 'Full', 'Restricted', 'None').</value>
    public string? systemInfoNpotSupport { get; set; }
}
