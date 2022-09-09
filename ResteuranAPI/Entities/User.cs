using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace ResteuranAPI.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Nationality { get; set; }
    public DateTime DateOfBirth { get; set; }
    
    public string PasswordHash { get; set; }
    
    public int RoleId { get; set; }
    public virtual Role Role { get; set; }
}

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
}