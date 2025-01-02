using System;

namespace BackendService.DTOs;

public class FormResponseObject_DTO
{
    public Guid Id { get; set; }
    public Guid ParentTemplateId { get; set; }
    public Guid RespondentId { get; set; }
    public DateTime RespondedAt { get; set; }
    public string Title { get; set; }
    public List<BlockResponse_DTO> BlockResponses { get; set; }
}
