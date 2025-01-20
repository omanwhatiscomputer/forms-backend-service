using System;


namespace BackendService.DTOs;

public class FormTemplate_DTO
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public string AccessControl { get; set; }
    public ICollection<AuthorizedUser_DTO> AuthorizedUsers { get; set; } = new List<AuthorizedUser_DTO>();
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Description { get; set; }
    public string BannerUrl { get; set; }
    public Topic_DTO Topic { get; set; }
    public List<Like_DTO> Likes { get; } = [];
    public List<FormTemplateTag_DTO> Tags { get; set; } = new List<FormTemplateTag_DTO>();
    public List<Comment_DTO> Comments { get; set; } = [];
    public ICollection<FormResponseObject_DTO> Responses { get; set; } = [];
    public List<Block_DTO> Blocks { get; set; }
}
