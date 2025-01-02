using System;


namespace BackendService.DTOs;

public class FormTemplateTag_DTO
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public Guid FormTemplateId { get; set; }
    public UserTag_DTO UserTag { get; set; }
    public string TagName { get; set; }
    public Guid UserTagId { get; set; }
}
