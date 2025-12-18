namespace Server_DB_Postgres.Interfaces;

/// <summary> Интерфейс для времени обновления. </summary>
public interface IUpdatedAt
{
    /// <summary> UTC дата время обновления. Контролируется на уровне EF автоматически при сохранении по интерфейсу <see cref="IUpdatedAt"/>. </summary>
    DateTimeOffset UpdatedAt { get; set; }
}
