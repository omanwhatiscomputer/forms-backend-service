using System;

using BackendService.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Data;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<FormTemplate> FormTemplates { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<UserTag> UserTags { get; set; }
    public DbSet<FormTemplateTag> FormTags { get; set; }
    public DbSet<AuthorizedUser> AuthorizedUsers { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Block> Blocks { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<CheckboxOption> CheckboxOptions { get; set; }
    public DbSet<FormResponseObject> FormResponseObjects { get; set; }
    public DbSet<BlockResponse> BlockResponses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserId)
            .IsUnique();



        modelBuilder.Entity<User>().HasMany(u => u.FormTemplates).WithOne(f => f.Author).HasForeignKey(f => f.AuthorId);
        modelBuilder.Entity<User>().HasMany(u => u.FormsRespondedTo).WithOne(r => r.Respondent).HasForeignKey(r => new { r.RespondentId, r.ParentTemplateId });
        modelBuilder.Entity<User>().HasMany(u => u.Tags).WithOne(t => t.User).HasForeignKey(t => t.UserId);
        modelBuilder.Entity<User>().HasMany(u => u.Likes).WithOne(l => l.User).HasForeignKey(l => l.UserId);
        modelBuilder.Entity<User>().HasMany(u => u.AuthorizedForms).WithOne(af => af.User).HasForeignKey(af => af.UserId);
        modelBuilder.Entity<User>().HasMany(u => u.Comments).WithOne(c => c.Commenter).HasForeignKey(c => c.CommenterId);

        modelBuilder.Entity<FormTemplate>().HasMany(f => f.Likes).WithOne(l => l.FormTemplate).HasForeignKey(l => l.FormTemplateId);
        modelBuilder.Entity<FormTemplate>().HasMany(f => f.Tags).WithOne(t => t.FormTemplate).HasForeignKey(t => t.FormTemplateId).OnDelete(DeleteBehavior.Cascade); ;
        modelBuilder.Entity<FormTemplate>().HasMany(f => f.Blocks).WithOne(b => b.ParentTemplate).HasForeignKey(b => b.ParentTemplateId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<FormTemplate>().HasOne(f => f.Topic);
        modelBuilder.Entity<FormTemplate>().HasMany(f => f.Responses).WithOne(r => r.ParentTemplate).HasForeignKey(r => r.ParentTemplateId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<FormTemplate>().HasMany(f => f.Comments).WithOne(c => c.ParentTemplate).HasForeignKey(c => c.ParentTemplateId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<FormTemplate>().HasMany(f => f.AuthorizedUsers).WithOne(au => au.FormTemplate).HasForeignKey(au => au.FormTemplateId).OnDelete(DeleteBehavior.Cascade); ;


        modelBuilder.Entity<Block>().HasMany(b => b.QuestionGroup).WithOne(q => q.Block).HasForeignKey(q => q.BlockId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FormResponseObject>().HasMany(fro => fro.BlockResponses).WithOne(br => br.FormResponseObject).HasForeignKey(br => br.FormResponseObjectId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Block>().HasMany(b => b.BlockResponses).WithOne(br => br.Block).HasForeignKey(br => br.BlockId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<FormResponseObject>().HasOne(fro => fro.Respondent).WithMany(u => u.FormsRespondedTo).HasForeignKey(u => u.RespondentId);
        modelBuilder.Entity<Block>().HasMany(b => b.CheckboxOptions).WithOne(co => co.Block).HasForeignKey(co => co.BlockId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserTag>().HasMany(ut => ut.FormTemplateTags).WithOne(ftt => ftt.UserTag).HasForeignKey(ftt => ftt.UserTagId).OnDelete(DeleteBehavior.Cascade);

    }
}
