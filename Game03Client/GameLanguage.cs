using System.Collections.Generic;

namespace Game03Client;

public class GameLanguage(string nameEnglish, string nameLocale, string nameShort)
{

    /// <summary>
    /// Наименование языка на английском языке.
    /// </summary>
    public string NameEnglish { get; } = nameEnglish;

    /// <summary>
    /// Наименование языка на этом же языке.
    /// </summary>
    public string NameLocale { get; } = nameLocale;

    /// <summary>
    /// Двухбуквенное имя в нижнем регистре.
    /// </summary>
    public string NameShort { get; } = nameShort.ToLower();

    /// <summary>
    /// Английский.
    /// </summary>
    public static GameLanguage En { get; } = new GameLanguage("English", "English", nameof(En));

    /// <summary>
    /// Русский.
    /// </summary>
    public static GameLanguage Ru { get; } = new GameLanguage("Russian", "Русский", nameof(Ru));

    /// <summary>
    /// Все языки доступные в игре.
    /// </summary>
    public static IEnumerable<GameLanguage> AllLanguages { get; } = [En, Ru];
}
