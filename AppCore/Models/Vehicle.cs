using AppCore.Dtos;
using AppCore.ValueObjects;

namespace AppCore.Models;

public class Vehicle : EntityBase
{
    public string LicensePlate { get; set; }
    public string Brand  { get; set; }
    public string Color { get; set; }
    public IEnumerable<ParkingSession> ParkingSessions { get; set; }
    
    public static implicit operator VehicleDto(Vehicle entity) =>
        new (
            entity.Id,
            entity.LicensePlate,
            entity.Brand,
            entity.Color
        );
}