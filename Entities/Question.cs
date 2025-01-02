using System;

namespace BackendService.Entities;

public class Question
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BlockId { get; set; }
    public Block Block { get; set; } = null!;
    public QuestionType Type { get; set; } = QuestionType.RichText;
    public string Content { get; set; }
}
