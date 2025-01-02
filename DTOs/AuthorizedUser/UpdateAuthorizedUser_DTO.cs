using System;

namespace BackendService.DTOs.AuthorizedUser;

public class UpdateAuthorizedUser_DTO
{
    public Guid UserId { get; set; }
    public string NormalizedName { get; set; }
    public string Email { get; set; }
    public Guid FormTemplateId { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorNormalizedName { get; set; }
    public string AuthorEmail { get; set; }
}
