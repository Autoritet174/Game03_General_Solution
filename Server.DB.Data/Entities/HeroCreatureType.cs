using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DB.Data.Entities;
// Сущность для таблицы связи
public class HeroCreatureType
{
    public Guid HeroId { get; set; }
    public Guid CreatureTypeId { get; set; }
    public DateTime CreatedAt { get; set; }

    public required Hero Hero { get; set; }
    public required CreatureType CreatureType { get; set; }
}
