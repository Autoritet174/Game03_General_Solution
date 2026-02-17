using System;
using System.Text.Json.Serialization;

namespace General.DTO;

[JsonDerivedType(typeof(DtoWSResponseS2C), nameof(DtoWSResponseS2C))]
[JsonDerivedType(typeof(DtoWSEquipmentTakeOnC2S), nameof(DtoWSEquipmentTakeOnC2S))]
[JsonDerivedType(typeof(DtoWSEquipmentTakeOffC2S), nameof(DtoWSEquipmentTakeOffC2S))]
[JsonDerivedType(typeof(DtoWSLogMessageS2C), nameof(DtoWSLogMessageS2C))]
public abstract class DtoWS(Guid? messageId = null)
{
    /// <summary> Идентификатор сообщения. </summary>
    public Guid? MessageId { get; } = messageId;
}

/// <summary> Ответ сервера клиенту о том успешна ли была выполнена команда в предыдущем сообщениии. </summary>
public class DtoWSResponseS2C(bool success, Guid inReplyTo, string? errorMessage = null) : DtoWS
{
    public bool Success { get; } = success;

    /// <summary> Идентификатор сообщения на которое мы отвечаем. </summary>
    public Guid InReplyTo { get; } = inReplyTo;
    public string? ErrorMessage { get; } = errorMessage;
}

/// <summary> Сообщение "Надеть экипировку". </summary>
public class DtoWSEquipmentTakeOnC2S(Guid heroId, Guid equipmentId, bool? inAltSlot = null, Guid? messageId = null) : DtoWS(messageId)
{
    public Guid HeroId { get; } = heroId;
    public Guid EquipmentId { get; } = equipmentId;
    public bool? InAltSlot { get; } = inAltSlot;
}

/// <summary> Сообщение "Снять экипировку". </summary>
public class DtoWSEquipmentTakeOffC2S(Guid equipmentId, Guid? messageId = null) : DtoWS(messageId)
{
    public Guid EquipmentId { get; } = equipmentId;
}

public class DtoWSLogMessageS2C(string message) : DtoWS
{
    public string Message { get; } = message;
}
