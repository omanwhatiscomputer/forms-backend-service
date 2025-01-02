using System;
using System.ComponentModel.DataAnnotations;


namespace BackendService.Entities;

public class Block
{
    public Guid Id { get; set; }
    public InputType BlockType { get; set; } = InputType.SingleLine;
    public int Index { get; set; }
    public Guid ParentTemplateId { get; set; }
    public FormTemplate ParentTemplate { get; set; } = null!;
    public string Title { get; set; }
    public bool IsRequired { get; set; }
    public string Description { get; set; }
    public List<CheckboxOption> CheckboxOptions { get; set; }
    public List<Question> QuestionGroup { get; set; }
    public List<BlockResponse> BlockResponses { get; set; } = new List<BlockResponse>();


}


