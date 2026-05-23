using System.ComponentModel.DataAnnotations;
using AppCore.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class AppRole : IdentityRole
{
    public string? Description { get; set; }
	
    public AppRole() { }
    public AppRole(string roleName, string? description = null) : base(roleName)
    {
        Description = description;
    }
}
	
public class AppUser:  IdentityUser, ISystemUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Department { get; set; }
    public required SystemUserStatus Status { get; set; }
    public DateTime CreatedAt { get; set;  }
    public DateTime? LastLoginAt { get; private set; }
    public DateTime? DeactivatedAt { get; private set; }
	    
    public void Activate()
    {
        if (Status != SystemUserStatus.Inactive)
        {
            return;
        }

        Status = SystemUserStatus.Active;
    }
	
    public void Deactivate(DateTime now)
    {
        if (Status != SystemUserStatus.Active)
        {
            return;
        }
        
        Status = SystemUserStatus.Inactive;
        DeactivatedAt = now;
    }
}