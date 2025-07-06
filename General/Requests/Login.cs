using System.ComponentModel.DataAnnotations;

namespace General.Requests;


public class Login
{
    [EmailAddress]
    public string? Email { get; set; }

    public string? Password { get; set; }
}