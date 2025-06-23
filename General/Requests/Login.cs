using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace General.Requests;


public class Login
{
    [EmailAddress]
    public string? Email { get; set; }

    public string? Password { get; set; }
}