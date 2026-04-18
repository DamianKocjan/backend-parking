using AppCore.Dtos;

namespace AppCore.Models;

public class ParkingSession : EntityBase
{
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
    public string GateName { get; set; }
    public DateTime EntryTime { get; set; }
    public DateTime? ExitTime { get; set; }
    public decimal? ParkingFee { get; set; }
    public ParkingTariff PricedBy { get; set; }
    public ParkingGate RegisteredAt { get; set; }
    public bool IsActive { get; set; }
    
    public static implicit operator ParkingSessionHistoryDto(ParkingSession entity) =>
        new (
            entity.Id,
            entity.Vehicle,
            entity.GateName,
            entity.EntryTime,
            entity.ExitTime,
            entity.ExitTime.HasValue ? (entity.ExitTime.Value - entity.EntryTime) : null,
            entity.ParkingFee,
            entity.IsActive
        );
    
    public static implicit operator ActiveParkingSessionDto(ParkingSession entity) =>
        new (
            entity.Id,
            entity.Vehicle,
            entity.GateName,
            entity.EntryTime,
            DateTime.Now - entity.EntryTime
        );
    
    public static implicit operator ParkingEntryResultDto(ParkingSession entity) =>
        new (
            entity.Id,
            entity.Vehicle,
            entity.GateName,
            entity.EntryTime,
            "Vehicle entry recorded successfully."
        );
    
    public static implicit operator ParkingExitResultDto(ParkingSession entity) =>
        new (
            entity.Id,
            entity.Vehicle,
            entity.GateName,
            entity.EntryTime,
            entity.ExitTime ?? DateTime.Now,
            (entity.ExitTime ?? DateTime.Now) - entity.EntryTime,
            entity.PricedBy?.FreeParkingDuration ?? TimeSpan.Zero,
            entity.ParkingFee ?? 0m,
            (entity.ParkingFee ?? 0m) > 0m,
            "Vehicle exit recorded successfully."
        );
}