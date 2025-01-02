using System;

namespace BackendService.DTOs;

public class CreateUser_DTO
{
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string Email {get; set;}
    public string Password {get; set;}
    public string PreferredTheme {get; set;} = "light";
    public string PreferredLocale {get; set;} = "en-US";
}
