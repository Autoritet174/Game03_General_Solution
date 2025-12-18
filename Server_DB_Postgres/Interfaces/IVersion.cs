namespace Server_DB_Postgres.Interfaces;

/// <summary> Интерфейс для переменной Version используемой для оптимистической блокировки. </summary>
public interface IVersion
{
    /// <summary> Версия записи для оптимистической блокировки. </summary>
    long Version { get; set; }
}
