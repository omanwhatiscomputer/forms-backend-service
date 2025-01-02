using System;
using BackendService.DTOs.AuthorizedUser;

namespace BackendService.DTOs;

public class UpdateFormTemplate_DTO
{
    public Guid AuthorId { get; set; }
    public string AccessControl { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string BannerUrl { get; set; }
    public string Topic { get; set; }
    public List<CreateBlock_DTO> Blocks { get; set; }
    public List<CreateFormTemplateTag_DTO> Tags { get; set; }
    public ICollection<UpdateAuthorizedUser_DTO> AuthorizedUsers { get; set; } = new List<UpdateAuthorizedUser_DTO>();
}
