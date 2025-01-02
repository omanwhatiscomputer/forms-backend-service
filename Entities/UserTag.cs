
using Microsoft.EntityFrameworkCore;

namespace BackendService.Entities;

[Index(nameof(TagName), IsUnique = true)]
public class UserTag
{
    public Guid Id {get; set;}

    public string TagName { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public List<FormTemplateTag> FormTemplateTags = new List<FormTemplateTag>();
    
}
