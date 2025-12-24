using System;
using System.Collections.Generic;
using System.Text;

namespace Game03Client.DTO;

public class DtoEquipmentType(int id, string name, int massPhysical, int massMagical)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
    public int MassPhysical { get; } = massPhysical;
    public int MassMagical { get; } = massMagical;
    
}
