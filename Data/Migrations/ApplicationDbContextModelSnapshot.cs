﻿// <auto-generated />
using System;
using BackendService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BackendService.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BackendService.Entities.AuthorizedUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AuthorEmail")
                        .HasColumnType("text");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<string>("AuthorNormalizedName")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<Guid>("FormTemplateId")
                        .HasColumnType("uuid");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FormTemplateId");

                    b.HasIndex("UserId");

                    b.ToTable("AuthorizedUsers");
                });

            modelBuilder.Entity("BackendService.Entities.Block", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("BlockType")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("Index")
                        .HasColumnType("integer");

                    b.Property<bool>("IsRequired")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ParentTemplateId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ParentTemplateId");

                    b.ToTable("Blocks");
                });

            modelBuilder.Entity("BackendService.Entities.BlockResponse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BlockId")
                        .HasColumnType("uuid");

                    b.Property<int>("BlockType")
                        .HasColumnType("integer");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<Guid>("FormResponseObjectId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsRequired")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ParentTemplateId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RespondentId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("BlockId");

                    b.HasIndex("FormResponseObjectId");

                    b.ToTable("BlockResponses");
                });

            modelBuilder.Entity("BackendService.Entities.CheckboxOption", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BlockId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<bool>("IncludesImage")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("BlockId");

                    b.ToTable("CheckboxOptions");
                });

            modelBuilder.Entity("BackendService.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CommenterId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ParentTemplateId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CommenterId");

                    b.HasIndex("ParentTemplateId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("BackendService.Entities.FormResponseObject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ParentTemplateId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("RespondedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("RespondentId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ParentTemplateId");

                    b.HasIndex("RespondentId");

                    b.ToTable("FormResponseObjects");
                });

            modelBuilder.Entity("BackendService.Entities.FormTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessControl")
                        .HasColumnType("integer");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<string>("BannerUrl")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<Guid>("TopicId")
                        .HasColumnType("uuid");

                    b.Property<string>("TopicName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("TopicName");

                    b.ToTable("FormTemplates");
                });

            modelBuilder.Entity("BackendService.Entities.FormTemplateTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("FormTemplateId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserTagId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FormTemplateId");

                    b.HasIndex("UserTagId");

                    b.ToTable("FormTags");
                });

            modelBuilder.Entity("BackendService.Entities.Like", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("FormTemplateId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FormTemplateId");

                    b.HasIndex("UserId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("BackendService.Entities.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BlockId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("BlockId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("BackendService.Entities.Topic", b =>
                {
                    b.Property<string>("TopicName")
                        .HasColumnType("text");

                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.HasKey("TopicName");

                    b.HasIndex("TopicName")
                        .IsUnique();

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("BackendService.Entities.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccountStatus")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PasswordSalt")
                        .HasColumnType("text");

                    b.Property<DateTime>("PasswordSaltRenewedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PreferredLocale")
                        .HasColumnType("text");

                    b.Property<string>("PreferredTheme")
                        .HasColumnType("text");

                    b.Property<int>("UserType")
                        .HasColumnType("integer");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BackendService.Entities.UserTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("TagName")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TagName")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("UserTags");
                });

            modelBuilder.Entity("BackendService.Entities.AuthorizedUser", b =>
                {
                    b.HasOne("BackendService.Entities.FormTemplate", "FormTemplate")
                        .WithMany("AuthorizedUsers")
                        .HasForeignKey("FormTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackendService.Entities.User", "User")
                        .WithMany("AuthorizedForms")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FormTemplate");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BackendService.Entities.Block", b =>
                {
                    b.HasOne("BackendService.Entities.FormTemplate", "ParentTemplate")
                        .WithMany("Blocks")
                        .HasForeignKey("ParentTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParentTemplate");
                });

            modelBuilder.Entity("BackendService.Entities.BlockResponse", b =>
                {
                    b.HasOne("BackendService.Entities.Block", "Block")
                        .WithMany("BlockResponses")
                        .HasForeignKey("BlockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackendService.Entities.FormResponseObject", "FormResponseObject")
                        .WithMany("BlockResponses")
                        .HasForeignKey("FormResponseObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Block");

                    b.Navigation("FormResponseObject");
                });

            modelBuilder.Entity("BackendService.Entities.CheckboxOption", b =>
                {
                    b.HasOne("BackendService.Entities.Block", "Block")
                        .WithMany("CheckboxOptions")
                        .HasForeignKey("BlockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Block");
                });

            modelBuilder.Entity("BackendService.Entities.Comment", b =>
                {
                    b.HasOne("BackendService.Entities.User", "Commenter")
                        .WithMany("Comments")
                        .HasForeignKey("CommenterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackendService.Entities.FormTemplate", "ParentTemplate")
                        .WithMany("Comments")
                        .HasForeignKey("ParentTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Commenter");

                    b.Navigation("ParentTemplate");
                });

            modelBuilder.Entity("BackendService.Entities.FormResponseObject", b =>
                {
                    b.HasOne("BackendService.Entities.FormTemplate", "ParentTemplate")
                        .WithMany("Responses")
                        .HasForeignKey("ParentTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackendService.Entities.User", "Respondent")
                        .WithMany("FormsRespondedTo")
                        .HasForeignKey("RespondentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParentTemplate");

                    b.Navigation("Respondent");
                });

            modelBuilder.Entity("BackendService.Entities.FormTemplate", b =>
                {
                    b.HasOne("BackendService.Entities.User", "Author")
                        .WithMany("FormTemplates")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackendService.Entities.Topic", "Topic")
                        .WithMany()
                        .HasForeignKey("TopicName");

                    b.Navigation("Author");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("BackendService.Entities.FormTemplateTag", b =>
                {
                    b.HasOne("BackendService.Entities.FormTemplate", "FormTemplate")
                        .WithMany("Tags")
                        .HasForeignKey("FormTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackendService.Entities.UserTag", "UserTag")
                        .WithMany("FormTemplateTags")
                        .HasForeignKey("UserTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FormTemplate");

                    b.Navigation("UserTag");
                });

            modelBuilder.Entity("BackendService.Entities.Like", b =>
                {
                    b.HasOne("BackendService.Entities.FormTemplate", "FormTemplate")
                        .WithMany("Likes")
                        .HasForeignKey("FormTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackendService.Entities.User", "User")
                        .WithMany("Likes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FormTemplate");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BackendService.Entities.Question", b =>
                {
                    b.HasOne("BackendService.Entities.Block", "Block")
                        .WithMany("QuestionGroup")
                        .HasForeignKey("BlockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Block");
                });

            modelBuilder.Entity("BackendService.Entities.UserTag", b =>
                {
                    b.HasOne("BackendService.Entities.User", "User")
                        .WithMany("Tags")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BackendService.Entities.Block", b =>
                {
                    b.Navigation("BlockResponses");

                    b.Navigation("CheckboxOptions");

                    b.Navigation("QuestionGroup");
                });

            modelBuilder.Entity("BackendService.Entities.FormResponseObject", b =>
                {
                    b.Navigation("BlockResponses");
                });

            modelBuilder.Entity("BackendService.Entities.FormTemplate", b =>
                {
                    b.Navigation("AuthorizedUsers");

                    b.Navigation("Blocks");

                    b.Navigation("Comments");

                    b.Navigation("Likes");

                    b.Navigation("Responses");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("BackendService.Entities.User", b =>
                {
                    b.Navigation("AuthorizedForms");

                    b.Navigation("Comments");

                    b.Navigation("FormTemplates");

                    b.Navigation("FormsRespondedTo");

                    b.Navigation("Likes");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("BackendService.Entities.UserTag", b =>
                {
                    b.Navigation("FormTemplateTags");
                });
#pragma warning restore 612, 618
        }
    }
}
