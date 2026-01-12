namespace General.DTO.RestRequest;

/// <summary>
/// Класс, содержащий данные для авторизации пользователя и системную информацию об устройстве.
/// </summary>
/// <remarks>
/// Этот класс использует конструктор с параметрами для инициализации всех свойств.
/// Все поля обязательны для заполнения при создании экземпляра.
/// </remarks>
/// <remarks>
/// Инициализирует новый экземпляр класса <see cref="DtoRequestAuthReg"/>.
/// </remarks>
/// <param name="email">Адрес электронной почты пользователя, используемый для входа в систему.</param>
/// <param name="password">Пароль пользователя в открытом виде (должен передаваться по защищённому каналу).</param>
/// <param name="timeZoneInfo_Local_BaseUtcOffset_Minutes">Смещение локального часового пояса пользователя от UTC в минутах.</param>
/// <param name="system_Environment_UserName">Имя пользователя операционной системы, под которым запущено приложение.</param>
/// <param name="deviceModel">Модель устройства (например, 'iPhone 14', 'Samsung Galaxy S23').</param>
/// <param name="deviceType">Тип устройства (например, 'Mobile', 'Desktop', 'Console').</param>
/// <param name="operatingSystem">Операционная система устройства (например, 'Windows 11', 'macOS 14', 'Android 14').</param>
/// <param name="processorType">Архитектура и модель процессора (например, 'x64', 'ARM64').</param>
/// <param name="processorCount">Количество логических процессоров на устройстве.</param>
/// <param name="systemMemorySize">Объём оперативной памяти устройства в мегабайтах (МБ).</param>
/// <param name="graphicsDeviceName">Название графического адаптера (например, 'NVIDIA GeForce RTX 4070').</param>
/// <param name="graphicsMemorySize">Объём видеопамяти в мегабайтах (МБ).</param>
/// <param name="deviceUniqueIdentifier">Уникальный идентификатор устройства, используемый для привязки сессии.</param>
/// <param name="systemInfo_supportsInstancing">Указывает, поддерживает ли графическая система инстансинг (отрисовку множественных копий объектов за один вызов).</param>
/// <param name="systemInfo_npotSupport">Указывает, поддерживает ли графическая карта текстуры с размерами, не являющимися степенью двойки (NPOT).</param>
public class DtoRequestAuthReg(
    string email,
    string password,
    int timeZoneInfo_Local_BaseUtcOffset_Minutes,
    string system_Environment_UserName,
    string deviceUniqueIdentifier,
    string deviceModel,
    string deviceType,
    string operatingSystem,
    string processorType,
    int processorCount,
    int systemMemorySize,
    string graphicsDeviceName,
    int graphicsMemorySize,
    bool systemInfo_supportsInstancing,
    string systemInfo_npotSupport)
{

    /// <summary>
    /// Получает или задаёт адрес электронной почты пользователя.
    /// </summary>
    /// <value>Строка, содержащая email пользователя.</value>
    public string Email { get; } = email;

    /// <summary>
    /// Получает или задаёт пароль пользователя.
    /// </summary>
    /// <value>Строка с паролем. Передача должна осуществляться по защищённому соединению.</value>
    public string Password { get; } = password;

    /// <summary>
    /// Получает или задаёт смещение локального часового пояса от UTC в минутах.
    /// </summary>
    /// <value>Целое число, представляющее разницу во времени (например, +180 для UTC+3).</value>
    public int TimeZoneInfo_Local_BaseUtcOffset_Minutes { get; } = timeZoneInfo_Local_BaseUtcOffset_Minutes;

    /// <summary>
    /// Получает или задаёт имя пользователя операционной системы.
    /// </summary>
    /// <value>Имя пользователя, под которым запущено приложение (например, 'JohnDoe').</value>
    public string System_Environment_UserName { get; } = system_Environment_UserName;

    /// <summary>
    /// Получает или задаёт модель устройства.
    /// </summary>
    /// <value>Описание аппаратной модели устройства.</value>
    public string DeviceModel { get; } = deviceModel;

    /// <summary>
    /// Получает или задаёт тип устройства.
    /// </summary>
    /// <value>Категория устройства: мобильное, настольное, игровая консоль и т.д.</value>
    public string DeviceType { get; } = deviceType;

    /// <summary>
    /// Получает или задаёт операционную систему устройства.
    /// </summary>
    /// <value>Название и версия ОС (например, 'Windows 10', 'iOS 17').</value>
    public string OperatingSystem { get; } = operatingSystem;

    /// <summary>
    /// Получает или задаёт тип процессора устройства.
    /// </summary>
    /// <value>Архитектура и модель процессора (например, 'Intel Core i7', 'Apple M1').</value>
    public string ProcessorType { get; } = processorType;

    /// <summary>
    /// Получает или задаёт количество логических ядер процессора.
    /// </summary>
    /// <value>Число, равное количеству потоков процессора.</value>
    public int ProcessorCount { get; } = processorCount;

    /// <summary>
    /// Получает или задаёт объём оперативной памяти устройства в мегабайтах.
    /// </summary>
    /// <value>Размер ОЗУ в мегабайтах (например, 16384 для 16 ГБ).</value>
    public int SystemMemorySize { get; } = systemMemorySize;

    /// <summary>
    /// Получает или задаёт название графического устройства.
    /// </summary>
    /// <value>Модель видеокарты или встроенного GPU.</value>
    public string GraphicsDeviceName { get; } = graphicsDeviceName;

    /// <summary>
    /// Получает или задаёт объём видеопамяти в мегабайтах.
    /// </summary>
    /// <value>Размер видеопамяти (VRAM) в МБ.</value>
    public int GraphicsMemorySize { get; } = graphicsMemorySize;

    /// <summary>
    /// Получает или задаёт уникальный идентификатор устройства.
    /// </summary>
    /// <value>Уникальная строка, идентифицирующая устройство (например, IMEI, UUID).</value>
    public string DeviceUniqueIdentifier { get; } = deviceUniqueIdentifier;

    /// <summary>
    /// Получает или задаёт признак поддержки графической системой инстансинга.
    /// </summary>
    /// <value><c>true</c>, если поддерживается; иначе <c>false</c>.</value>
    public bool SystemInfo_supportsInstancing { get; } = systemInfo_supportsInstancing;

    /// <summary>
    /// Получает или задаёт информацию о поддержке текстур с размерами, не являющимися степенью двойки (NPOT).
    /// </summary>
    /// <value>Строка, описывающая уровень поддержки NPOT (например, 'Full', 'Restricted', 'None').</value>
    public string SystemInfo_npotSupport { get; } = systemInfo_npotSupport;
}
