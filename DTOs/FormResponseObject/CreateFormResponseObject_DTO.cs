using System;

namespace BackendService.DTOs;

public class CreateFormResponseObject_DTO
{

    public Guid Id { get; set; }
    public Guid ParentTemplateId { get; set; }
    public Guid RespondentId { get; set; }
    public DateTime RespondedAt { get; set; }
    public List<CreateBlockResponse_DTO> BlockResponses { get; set; }
}
