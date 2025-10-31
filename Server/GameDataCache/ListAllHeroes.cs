using Microsoft.EntityFrameworkCore;
using Server_DB_Data;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Server.GameDataCache;

public static class ListAllHeroes
{
    public static string Json { get; private set; } = string.Empty;

    public static async void Init()
    {
        using DbContext_Game03Data db = new();
        var data = await db.Heroes
          .AsNoTracking()
          .Select(h => new
          {
              h.Id, 
              h.Name,
              h.Rarity,
              h.BaseHealth,
              h.BaseAttack,
          })
          .ToListAsync();

        // Создаём JSON-массив
        JsonArray jsonArray = [];

        foreach (var item in data)
        {
            JsonObject obj = new()
            {
                ["id"] = item.Id,
                ["name"] = item.Name,
                ["rarity"] = (int)item.Rarity,
                ["baseHealth"] = (int)item.BaseHealth,
                ["baseAttack"] = (int)item.BaseAttack,
            };

            jsonArray.Add(obj);
        }

        // Обёртка массива в объект с ключом "heroes"
        JsonObject root = new()
        {
            ["heroes"] = jsonArray
        };

        Json = root.ToJsonString(new JsonSerializerOptions
        {
            WriteIndented = false//Debugger.IsAttached
        });
    }
}
