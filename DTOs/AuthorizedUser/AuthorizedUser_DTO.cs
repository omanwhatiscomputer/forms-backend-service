using System;

namespace BackendService.DTOs;

public class AuthorizedUser_DTO
{
    public Guid UserId { get; set; }
    public Guid FormTemplateId { get; set; }
    public string NormalizedName { get; set; }
    public string Email { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorNormalizedName { get; set; }
    public string AuthorEmail { get; set; }
}
