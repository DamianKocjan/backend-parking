using AppCore.Models;

namespace AppCore.Dtos;

public record ParkingGateDto(
    Guid Id,
    string Name,
    string Type,
    string Location,
    bool IsOperational
);

public record CreateGateDto(
    string Name,
    string Type,
    string Location
)
{
    public ParkingGate ToEntity()
    {
        var type = Enum.Parse<GateType>(Type);
        
        return new ParkingGate()
        {
            Name = Name,
            Type = type,
            Location = Location,
            IsOperational = true
        };
    }
};

public record UpdateGateDto(
    Guid Id,
    string Name,
    string Type
)
{
    public ParkingGate ToEntity()
    {
        var type = Enum.Parse<GateType>(Type);
        return new ParkingGate()
        {
            Id = Id,
            Name = Name,
            Type = type
        };
    }
}