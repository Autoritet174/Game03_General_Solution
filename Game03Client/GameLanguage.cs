using System.Collections.Generic;

namespace Game03Client;

/// <summary>
/// Язык игры.
/// </summary>
/// <param name="nameEnglish">Наименование языка на английском.</param>
/// <param name="nameLocale">Наименование языка на это же языке.</param>
/// <param name="nameShort">Наименование языка на английском в две буквы.</param>
public class GameLanguage(string nameEnglish, string nameLocale, string nameShort)
{

    /// <summary>
    /// Наименование языка на английском языке.
    /// </summary>
    public string nameEnglish { get; } = nameEnglish;

    /// <summary>
    /// Наименование языка на этом же языке.
    /// </summary>
    public string nameLocale { get; } = nameLocale;

    /// <summary>
    /// Двухбуквенное имя в нижнем регистре.
    /// </summary>
    public string nameShort { get; } = nameShort.ToLower();

    /// <summary>
    /// Английский.
    /// </summary>
    public static GameLanguage en { get; } = new GameLanguage("English", "English", nameof(en));

    /// <summary>
    /// Русский.
    /// </summary>
    public static GameLanguage ru { get; } = new GameLanguage("Russian", "Русский", nameof(ru));

    /// <summary>
    /// Все языки доступные в игре.
    /// </summary>
    public static IEnumerable<GameLanguage> allLanguages { get; } = [en, ru];
}
