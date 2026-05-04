using System.ComponentModel.DataAnnotations;

namespace General.DTO.Entities.GameData;

/// <summary> Data Transfer Object. Представляет базовую сущность игрового героя с основными характеристиками. </summary>
public class DtoBattlefield(int id, string name)
{
    public int Id { get; } = id;

    public string Name { get; } = name;
}
