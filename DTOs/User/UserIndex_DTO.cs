using System;

namespace BackendService.DTOs.User;

public class UserIndex_DTO
{
    public string UserId {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string NormalizedName {get; set;}
    public string Email {get; set;}
    public string UserType {get; set;}
}
