using AppCore.Dtos;

namespace AppCore.Models;

public enum GateType
{
    Entry,
    Exit,
    Both
}

public class ParkingGate : EntityBase
{
    public string Name { get; set; }
    public GateType Type { get; set; }
    public string Location { get; set; }
    public bool IsOperational { get; set; }
    
    public static implicit operator ParkingGateDto(ParkingGate entity) =>
        new (
            entity.Id,
            entity.Name,
            entity.Type.ToString(),
            entity.Location,
            entity.IsOperational
        );
}