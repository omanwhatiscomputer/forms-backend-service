using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendService.Data;
using BackendService.DTOs;
using BackendService.DTOs.FormTemplate;
using BackendService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Controllers;
[ApiController]
[Route("api/form")]
public class FormTemplateController(IDbContextWrapper dbContextWrapper, IMapper mapper) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<FormTemplate_DTO>> GetFormTemplateById(Guid id)
    {
        var formTemplate = await dbContextWrapper.Context.FormTemplates
            .Include(ft => ft.Tags).ThenInclude(t => t.UserTag)
            .Include(ft => ft.AuthorizedUsers)
            .Include(ft => ft.Blocks).ThenInclude(b => b.QuestionGroup)
            .Include(ft => ft.Blocks).ThenInclude(b => b.CheckboxOptions)
            .Include(ft => ft.Comments)
            .Include(ft => ft.Likes)
            .FirstOrDefaultAsync(ft => ft.Id == id);

        if (formTemplate == null) return NotFound();


        var topic = await dbContextWrapper.Context.Topics.FirstOrDefaultAsync(t => t.Id == formTemplate.TopicId);


        var ft_dto = mapper.Map<FormTemplate_DTO>(formTemplate);
        ft_dto.Topic = mapper.Map<Topic_DTO>(topic);
        return Ok(ft_dto);
    }

    // RegisterFormTemplateWithUserId
    [HttpPost]
    public async Task<ActionResult<FormTemplate_DTO>> RegisterFormTemplateWithUserId(CreateFormTemplate_DTO createFT_dto)
    {
        dbContextWrapper.Context.ChangeTracker.Clear();

        var user = await dbContextWrapper.Context.Users.FirstOrDefaultAsync(x => x.UserId == createFT_dto.AuthorId);
        if (user == null) return NotFound();

        var formTemplate = mapper.Map<FormTemplate>(createFT_dto);

        var topic = await dbContextWrapper.Context.Topics.FirstOrDefaultAsync(t => t.TopicName == createFT_dto.Topic);

        formTemplate.TopicId = topic.Id;

        // if (createFT_dto.Tags.Count() > 0)
        // {
        //     dbContextWrapper.Context.FormTags.AddRange(mapper.Map<List<FormTemplateTag>>(createFT_dto.Tags));
        // }

        // if (createFT_dto.AuthorizedUsers.Count() > 0)
        // {
        //     dbContextWrapper.Context.AuthorizedUsers.AddRange(mapper.Map<ICollection<AuthorizedUser>>(createFT_dto.AuthorizedUsers));
        // }

        dbContextWrapper.Context.FormTemplates.Add(formTemplate);
        var (statusCode, message) = await dbContextWrapper.SaveChangesAsync();
        if (statusCode != 201)
        {
            return StatusCode(statusCode, message);
        }
        return Ok(mapper.Map<FormTemplate_DTO>(formTemplate));
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<FormTemplate_DTO>> UpdateFormTemplateByTemplateId(Guid id, UpdateFormTemplate_DTO updateFT_dto)
    {


        using var transaction = await dbContextWrapper.Context.Database.BeginTransactionAsync();
        try
        {
            dbContextWrapper.Context.ChangeTracker.Clear();

            var formTemplate = await dbContextWrapper.Context.FormTemplates

                .FirstOrDefaultAsync(ft => ft.Id == id);

            if (formTemplate == null)
            {
                return NotFound($"FormTemplate with ID {id} not found.");
            }

            formTemplate.Description = updateFT_dto.Description;
            formTemplate.Title = updateFT_dto.Title;

            var topic = await dbContextWrapper.Context.Topics
                .FirstOrDefaultAsync(t => t.TopicName == updateFT_dto.Topic);

            if (topic == null)
            {
                return NotFound("Topic not found.");
            }


            formTemplate.TopicId = topic.Id;
            formTemplate.AccessControl = (Access)Enum.Parse(typeof(Access), updateFT_dto.AccessControl);

            // Update Tags
            var existingTags = await dbContextWrapper.Context.FormTags
                .Where(ftt => ftt.FormTemplateId == id)
                .ToListAsync();

            dbContextWrapper.Context.FormTags.RemoveRange(existingTags);

            var newTags = mapper.Map<List<FormTemplateTag>>(updateFT_dto.Tags);
            foreach (var tag in newTags)
            {
                tag.FormTemplateId = id;
                dbContextWrapper.Context.FormTags.Add(tag);
            }

            // Update Authorized Users
            var existingUsers = await dbContextWrapper.Context.AuthorizedUsers
                .Where(au => au.FormTemplateId == id)
                .ToListAsync();

            dbContextWrapper.Context.AuthorizedUsers.RemoveRange(existingUsers);

            var newUsers = mapper.Map<List<AuthorizedUser>>(updateFT_dto.AuthorizedUsers);
            foreach (var user in newUsers)
            {
                user.FormTemplateId = id;
                dbContextWrapper.Context.AuthorizedUsers.Add(user);
            }

            // Update Blocks
            var existingBlocks = await dbContextWrapper.Context.Blocks
                .Where(b => b.ParentTemplateId == id)
                .ToListAsync();

            var updatedBlockIds = updateFT_dto.Blocks.Select(b => b.Id).ToHashSet();
            var blocksToRemove = existingBlocks.Where(b => !updatedBlockIds.Contains(b.Id)).ToList();

            dbContextWrapper.Context.Blocks.RemoveRange(blocksToRemove);

            foreach (var blockDto in updateFT_dto.Blocks)
            {
                var block = existingBlocks.FirstOrDefault(b => b.Id == blockDto.Id);
                if (block != null)
                {
                    // Update existing block
                    block.Title = blockDto.Title ?? block.Title;
                    block.Description = blockDto.Description ?? block.Description;
                    block.IsRequired = blockDto.IsRequired;
                    block.Index = blockDto.Index;

                    // Update questions and options
                    var existingQuestions = await dbContextWrapper.Context.Questions
                        .Where(q => q.BlockId == block.Id)
                        .ToListAsync();
                    dbContextWrapper.Context.Questions.RemoveRange(existingQuestions);

                    var newQuestions = mapper.Map<List<Question>>(blockDto.QuestionGroup);
                    foreach (var question in newQuestions)
                    {
                        question.BlockId = block.Id;
                        dbContextWrapper.Context.Questions.Add(question);
                    }

                    var existingOptions = await dbContextWrapper.Context.CheckboxOptions
                        .Where(co => co.BlockId == block.Id)
                        .ToListAsync();
                    dbContextWrapper.Context.CheckboxOptions.RemoveRange(existingOptions);

                    var newOptions = mapper.Map<List<CheckboxOption>>(blockDto.CheckboxOptions);
                    foreach (var option in newOptions)
                    {
                        option.BlockId = block.Id;
                        dbContextWrapper.Context.CheckboxOptions.Add(option);
                    }
                }
                else
                {
                    // Add new block
                    var newBlock = mapper.Map<Block>(blockDto);
                    newBlock.ParentTemplateId = id;
                    dbContextWrapper.Context.Blocks.Add(newBlock);
                }
            }

            // Update the FormTemplate entity
            dbContextWrapper.Context.Entry(formTemplate).State = EntityState.Modified;

            var (statusCode, message) = await dbContextWrapper.SaveChangesAsync();
            if (statusCode != 201)
            {
                await transaction.RollbackAsync();
                return StatusCode(statusCode, message);
            }

            await transaction.CommitAsync();
            return Ok(mapper.Map<FormTemplate_DTO>(formTemplate));
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<FormTemplateIndex_DTO>>> GetAllFormTemplates()
    {
        var fts = await dbContextWrapper.Context.FormTemplates.ProjectTo<FormTemplateIndex_DTO>(mapper.ConfigurationProvider).ToListAsync();
        return Ok(fts);
    }
    [HttpGet("public")]
    public async Task<ActionResult<List<FormTemplateIndex_DTO>>> GetAllPublicFormTemplates()
    {
        var fts = await dbContextWrapper.Context.FormTemplates.Where(ft => ft.AccessControl == Access.PublicAccess).ProjectTo<FormTemplateIndex_DTO>(mapper.ConfigurationProvider).ToListAsync();
        return Ok(fts);
    }
    [HttpGet("private")]
    public async Task<ActionResult<List<FormTemplateIndex_DTO>>> GetAllPrivateFormTemplates()
    {
        var fts = await dbContextWrapper.Context.FormTemplates.Where(ft => ft.AccessControl == Access.RestrictedAccess).ProjectTo<FormTemplateIndex_DTO>(mapper.ConfigurationProvider).ToListAsync();
        return Ok(fts);
    }
    [HttpGet("private/{id}")]
    public async Task<ActionResult<List<FormTemplateIndex_DTO>>> GetAllPrivateFormTemplatesByAuthorizedUserId(Guid id)
    {
        var fts = await dbContextWrapper.Context.FormTemplates.Include(ft => ft.AuthorizedUsers)
            .Where(ft => ft.AccessControl == Access.RestrictedAccess && ft.AuthorizedUsers.Any(au => au.UserId == id))
            .ProjectTo<FormTemplateIndex_DTO>(mapper.ConfigurationProvider)
            .ToListAsync();
        return Ok(fts);
    }
}
