using System;

namespace BackendService.Entities;

public class Comment
{
    public Guid Id {get; set;}
    public string Content {get; set;}
    public FormTemplate ParentTemplate {get; set;}
    public Guid ParentTemplateId {get; set;}
    public Guid CommenterId {get; set;}
    public User Commenter {get; set;}
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
}
