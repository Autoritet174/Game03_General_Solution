using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.GameData;

[Table("SmithingMaterials", Schema = nameof(GameData))]
[Index(nameof(Name), IsUnique = true)]
public class SmithingMaterial
{
    /// <summary> Уникальный идентификатор. </summary>
    public int Id { get; set; }

    /// <summary> Уникальное наименование на английском. </summary>
    [MaxLength(255)]
    public required string Name { get; set; }

    /// <summary> Наименование на русском. </summary>
    [MaxLength(255)]
    public string? NameRu { get; set; }

    /// <summary> Навигационное свойство к <see cref="MaterialDamagePercent"/>. </summary>
    public ICollection<MaterialDamagePercent> MaterialDamagePercents { get; set; } = [];
}
