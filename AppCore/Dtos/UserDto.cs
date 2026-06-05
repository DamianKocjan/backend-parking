using AppCore.Models;

namespace AppCore.Dtos;

public record UserDto()
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public SystemUserStatus Status { get; set; }
    public string Email { get; set; }
    public string Department { get; set; }
    public IEnumerable<string> Roles { get; set; }
}