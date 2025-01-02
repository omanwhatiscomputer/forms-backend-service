using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Entities;

[PrimaryKey(nameof(UserId)), Index(nameof(Email), IsUnique = true)]
public class User
{
    [Key]
    public Guid UserId {get; set;} = Guid.NewGuid();
    [Required]
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string NormalizedName {get; set;}
    [Required, EmailAddress]
    public string Email {get; set;}
    public ICollection<FormTemplate> FormTemplates {get; set;} = new List<FormTemplate>();
    public ICollection<AuthorizedUser> AuthorizedForms {get; set;} = new List<AuthorizedUser>();
    public List<FormResponseObject> FormsRespondedTo {get; set;} = new List<FormResponseObject>();
    public Role UserType {get; set;} = Role.Regular;
    public Status AccountStatus {get; set;} = Status.Active;
    public required string PasswordHash {get; set;}
    public required string PasswordSalt {get; set;}
    public DateTime PasswordSaltRenewedAt {get; set;} = DateTime.UtcNow;
    public string PreferredTheme {get; set;} = "light";
    public string PreferredLocale {get; set;} = "en-US";
    public List<Like> Likes {get; set;} = [];
    public ICollection<UserTag> Tags { get; set; } = new List<UserTag>();
    public ICollection<Comment> Comments {get; set; } = new List<Comment>();
}
