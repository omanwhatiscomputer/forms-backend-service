using System;



namespace BackendService.DTOs;

public class User_DTO
{
    public Guid UserId {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string NormalizedName {get; set;}
    public string Email {get; set;}
    public string UserType {get; set;}
    public string AccountStatus {get; set;}
    public string PreferredTheme {get; set;} = "light";
    public string PreferredLocale {get; set;} = "en-US";
    public ICollection<FormTemplate_DTO> FormTemplates {get; set;} = new List<FormTemplate_DTO>();
    public ICollection<AuthorizedUser_DTO> AuthorizedForms {get; set;} = new List<AuthorizedUser_DTO>();
    public List<FormResponseObject_DTO> FormsRespondedTo {get; set;} = new List<FormResponseObject_DTO>();
    public List<Like_DTO> Likes {get; set;} = [];
    public ICollection<UserTag_DTO> Tags { get; set; }
}
