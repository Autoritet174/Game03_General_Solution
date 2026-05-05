using System;

namespace General.DTO.Battlefield;

public class SpawnedNpc(int baseNpcId, Guid spawnId)
{
    public int BaseNpcId { get; } = baseNpcId;
    public Guid SpawnId { get; } = spawnId;
    public float Health { get; set; }
}
