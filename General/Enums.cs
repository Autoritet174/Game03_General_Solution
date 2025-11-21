namespace General;

/// <summary>
/// Статический класс-контейнер для всех перечислений, используемых в общих данных игры.
/// </summary>
public static class Enums
{
    /// <summary>
    /// Определяет уровень редкости игрового объекта или сущности.
    /// </summary>
    public enum RarityLevel : int
    {
        /// <summary>
        /// Обычный уровень редкости (самый низкий).
        /// </summary>
        Common = 1,

        /// <summary>
        /// Необычный уровень редкости.
        /// </summary>
        Uncommon = 2,

        /// <summary>
        /// Редкий уровень редкости.
        /// </summary>
        Rare = 3,

        /// <summary>
        /// Эпический уровень редкости.
        /// </summary>
        Epic = 4,

        /// <summary>
        /// Легендарный уровень редкости.
        /// </summary>
        Legendary = 5,

        /// <summary>
        /// Мифический уровень редкости (самый высокий).
        /// </summary>
        Mythic = 6,
    }
}
