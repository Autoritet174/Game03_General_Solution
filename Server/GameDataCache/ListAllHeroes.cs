using Microsoft.EntityFrameworkCore;
using Server.DB.Data;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Server.GameDataCache;

public static class ListAllHeroes
{
    public static string Json { get; private set; } = "";

    public static async void Init()
    {
        using DbData db = new();
        var data = await db.Heroes
          .AsNoTracking()
          .Select(h => new
          {
              //h.Id, 
              h.Name,
          })
          .ToListAsync();

        // Создаём JSON-массив
        JsonArray jsonArray = [];

        foreach (var item in data)
        {
            JsonObject obj = new()
            {
                //["id"] = item.Id,
                ["name"] = item.Name,
            };

            jsonArray.Add(obj);
        }

        // Обёртка массива в объект с ключом, например "heroes"
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
