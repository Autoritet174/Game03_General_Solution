using System;

namespace General.DTO.Interfaces;

public interface ISoftDelete
{
    DateTimeOffset? deletedAt { get; set; }
}
