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
}