using System;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Entities;

public class AuthorizedUser
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string NormalizedName { get; set; }
    public string Email { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorNormalizedName { get; set; }
    public string AuthorEmail { get; set; }
    public Guid FormTemplateId { get; set; }
    public FormTemplate FormTemplate { get; set; } = null!;
}
