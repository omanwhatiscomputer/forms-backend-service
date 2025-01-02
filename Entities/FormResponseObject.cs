using System;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Entities;

public class FormResponseObject
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ParentTemplateId { get; set; }
    public FormTemplate ParentTemplate { get; set; }
    public Guid RespondentId { get; set; }
    public User Respondent { get; set; } = null!;
    public DateTime RespondedAt { get; set; } = DateTime.UtcNow;
    public List<BlockResponse> BlockResponses { get; set; } = new List<BlockResponse>();

}
