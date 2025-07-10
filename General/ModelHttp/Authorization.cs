namespace General.ModelHttp;

/// <summary>
/// Данные авторизации и характеристики аппаратного устройства.
/// </summary>
/// <param name="email">Электронная почта пользователя для авторизации. Не может быть null или пустой.</param>
/// <param name="password">Пароль пользователя для авторизации. Не может быть null или пустой.</param>
/// <param name="deviceModel">Модель устройства, на котором выполняется приложение (например, "MacBookPro18,3").</param>
/// <param name="deviceType">Тип устройства (например, "Desktop", "Handheld").</param>
/// <param name="operatingSystem">Операционная система устройства (например, "Windows 10", "Android 13").</param>
/// <param name="processorType">Тип процессора (например, "Apple M2", "Intel Core i7").</param>
/// <param name="processorCount">Количество логических ядер процессора. Должно быть больше 0.</param>
/// <param name="systemMemorySize">Объем оперативной памяти устройства в мегабайтах. Должен быть больше 0.</param>
/// <param name="graphicsDeviceName">Название графического процессора (например, "NVIDIA GeForce RTX 3060").</param>
/// <param name="graphicsMemorySize">Объем видеопамяти в мегабайтах. Должен быть больше или равен 0.</param>
public class Authorization(string email, string password, string deviceModel, string deviceType, string operatingSystem,
    string processorType, int processorCount, int systemMemorySize, string graphicsDeviceName, int graphicsMemorySize)
{
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
    public string DeviceModel { get; set; } = deviceModel;
    public string DeviceType { get; set; } = deviceType;
    public string OperatingSystem { get; set; } = operatingSystem;
    public string ProcessorType { get; set; } = processorType;
    public int ProcessorCount { get; set; } = processorCount;
    public int SystemMemorySize { get; set; } = systemMemorySize;
    public string GraphicsDeviceName { get; set; } = graphicsDeviceName;
    public int GraphicsMemorySize { get; set; } = graphicsMemorySize;
}
