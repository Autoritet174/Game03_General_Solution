using System;
using System.Text.Json.Serialization;

namespace General.DTO;

// Базовый класс для всех сообщений
[JsonDerivedType(typeof(EquipmentTakeOnMessage), typeDiscriminator: "takeon")]
[JsonDerivedType(typeof(EquipmentTakeOffMessage), typeDiscriminator: "takeoff")]
// Добавляйте новые команды сюда
public abstract class WebSocketMessage
{
    // Можно добавить общие свойства, например, идентификатор отправителя, если нужно
    // public string SenderId { get; set; }
}

/// <summary>
/// Сообщение "Надеть экипировку"
/// </summary>
/// <param name="heroId"></param>
/// <param name="equipmentId"></param>
/// <param name="inAltSlot"></param>
public class EquipmentTakeOnMessage(Guid heroId, Guid equipmentId, bool? inAltSlot = null) : WebSocketMessage
{
    public Guid HeroId { get; } = heroId;
    public Guid EquipmentId { get; } = equipmentId;
    public bool? InAltSlot { get; } = inAltSlot;
}

/// <summary>
/// Сообщение "Снять экипировку"
/// </summary>
/// <param name="equipmentId"></param>
public class EquipmentTakeOffMessage(Guid equipmentId) : WebSocketMessage
{
    public Guid EquipmentId { get; } = equipmentId;
}
