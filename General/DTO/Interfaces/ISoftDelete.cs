using System;

namespace General.DTO.Interfaces;

public interface ISoftDelete
{
    DateTimeOffset? DeletedAt { get; set; }
}
