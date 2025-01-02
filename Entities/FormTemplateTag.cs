

namespace BackendService.Entities;

public class FormTemplateTag
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AuthorId { get; set; }

    public Guid FormTemplateId { get; set; }
    public FormTemplate FormTemplate { get; set; } = null!;

    public Guid UserTagId { get; set; }
    public UserTag UserTag { get; set; } = null!;
}
