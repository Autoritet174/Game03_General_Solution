using System;

namespace General.DTO.Interfaces;

/// <summary> Интерфейс для времени создания. </summary>
public interface ICreatedAt
{
    /// <summary> UTC дата время создания. Контролируется на уровне EF автоматически при сохранении по интерфейсу <see cref="ICreatedAt"/>. </summary>
    DateTimeOffset CreatedAt { get; set; }
}
