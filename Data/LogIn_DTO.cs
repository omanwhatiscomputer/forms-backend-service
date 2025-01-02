using System;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Data;


public class LogIn_DTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
