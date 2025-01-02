using System;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Entities;

public class BlockResponse
{
    [Key]
    public Guid Id { get; set; }
    public Guid BlockId { get; set; }
    public Block Block { get; set; } = null!;
    public InputType BlockType { get; set; }
    public Guid ParentTemplateId { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsRequired { get; set; }

    public Guid RespondentId { get; set; }
    public FormResponseObject FormResponseObject { get; set; }
    public Guid FormResponseObjectId { get; set; }

}
