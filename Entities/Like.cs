using System;

namespace BackendService.Entities;

public class Like
{
    public Guid Id {get; set;} = Guid.NewGuid();
    public Guid FormTemplateId {get; set;}
    public FormTemplate FormTemplate {get; set;} = null!;
    public Guid UserId {get; set;}
    public User User {get; set;} = null!;
}
