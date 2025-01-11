using System;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Entities;

public class FormTemplate
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AuthorId { get; set; }
    public User Author { get; set; } = null!;
    public Access AccessControl { get; set; } = Access.PublicAccess;
    public ICollection<AuthorizedUser> AuthorizedUsers { get; set; } = new List<AuthorizedUser>();
    public required string Title { get; set; }

    public string Description { get; set; }
    public string BannerUrl { get; set; } = "placeholderImageUrl";

    public Guid TopicId { get; set; }
    public Topic Topic { get; set; } = null!;
    public List<Like> Likes { get; set; } = [];
    public ICollection<FormTemplateTag> Tags { get; set; } = new List<FormTemplateTag>();
    public ICollection<FormResponseObject> Responses { get; set; } = [];
    public List<Comment> Comments { get; set; } = [];
    [Required]
    public List<Block> Blocks { get; set; } = new List<Block>();

    public byte[] RowVersion { get; set; }
}
