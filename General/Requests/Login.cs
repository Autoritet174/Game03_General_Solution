using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace General.Requests;


public class Login
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}