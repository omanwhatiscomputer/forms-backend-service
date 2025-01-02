using System;

namespace BackendService.DTOs;

public class CreateFormTemplateTag_DTO
{
    public Guid FormTemplateId {get; set;}
    public Guid AuthorId { get; set; }
    public string TagName { get; set; }
    public Guid UserTagId {get; set;}
}
