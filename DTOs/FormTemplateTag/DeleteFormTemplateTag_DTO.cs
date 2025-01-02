using System;

namespace BackendService.DTOs;

public class DeleteFormTemplateTag_DTO
{
    public Guid AuthorId {get; set;}
    public string TagName {get; set;}
    public Guid FormTemplateId {get; set;}

}
