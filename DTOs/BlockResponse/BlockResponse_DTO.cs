using System;

namespace BackendService.DTOs;

public class BlockResponse_DTO
{
    public Guid Id { get; set; }
    public Guid BlockId { get; set; }
    public Guid ParentTemplateId { get; set; }
    public string Content { get; set; }
    public string BlockType { get; set; }
    public bool IsRequired { get; set; }
    public Guid RespondentId { get; set; }
    public Guid FormResponseObjectId { get; set; }
}
