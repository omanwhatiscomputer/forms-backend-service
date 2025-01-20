using System;

namespace BackendService.DTOs.FormResponseObject;

public class FormResponseObjectIndex_DTO
{
    public Guid Id { get; set; }
    public Guid ParentTemplateId { get; set; }
    public Guid RespondentId { get; set; }
    public DateTime RespondedAt { get; set; }
    public string Title { get; set; }
    public string RespondentFullName { get; set; }
    public string RespondentEmail { get; set; }

}
