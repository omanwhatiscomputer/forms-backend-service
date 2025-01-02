using System;

namespace BackendService.DTOs;

public class UpdateUser_DTO
{
    public string FirstName {get; set;}
    public string LastName {get; set;}
    
    // public ICollection<AuthorizedUser_DTO> AuthorizedForms {get; set;}
    // public ICollection<UserTag_DTO> Tags { get; set; }
    // public List<Like_DTO> Likes {get; set;}
    public string UserType {get; set;}
    public string AccountStatus {get; set;}
    public string PreferredTheme {get; set;}
    public string PreferredLocale {get; set;}
}
