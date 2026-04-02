using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Linq;

namespace Server_DB_Postgres.Entities.GameData;

[Table(nameof(DbContextGame.Npcs), Schema = nameof(GameData))]
[Index(nameof(Name), IsUnique = true)]
public class Npc
{
    public int Id { get; init; }

    /// <summary> Наименование на английском языке. </summary>
    [MaxLength(256)]
    public required string Name { get; set; }

    /// <summary> Уровень редкости. </summary>
    [Default(1)]
    public int Rarity { get; set; }

    /// <summary> Ранг: 0 - без ранга, 1 - элитный, 2 - босс. </summary>
    [Default(0)]
    public int Rank { get; set; }

    /// <summary> Основной стат который повышает урон. Сила(1) или Ловкость(2) или Интеллект(3). </summary>
    [Default(0)]
    public int MainStat { get; set; }
}
