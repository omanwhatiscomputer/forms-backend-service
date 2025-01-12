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

    //updateFormTemplate -> TODO: add authorizeduserlist
    [HttpPut("{id}")]
    public async Task<ActionResult<FormTemplate_DTO>> UpdateFormTemplateByTemplateId(Guid id, UpdateFormTemplate_DTO updateFT_dto)
    {
        var formTemplate = await dbContextWrapper.Context.FormTemplates
            .Include(ft => ft.Tags).ThenInclude(ftt => ftt.UserTag)
            .Include(ft => ft.AuthorizedUsers)
            .Include(ft => ft.Blocks).ThenInclude(b => b.QuestionGroup)
            .Include(ft => ft.Blocks).ThenInclude(b => b.CheckboxOptions)
            .Include(ft => ft.Comments)
            .Include(ft => ft.Likes)
            .FirstOrDefaultAsync(ft => ft.Id == id);

        if (formTemplate == null)
        {
            return NotFound($"FormTemplate with ID {id} not found.");
        }

        dbContextWrapper.Context.Entry(formTemplate).State = EntityState.Modified;

        var topic = await dbContextWrapper.Context.Topics.FirstOrDefaultAsync(t => t.TopicName == updateFT_dto.Topic);

        if (topic == null)
        {
            return NotFound();
        }

        formTemplate.TopicId = topic.Id;
        formTemplate.AccessControl = (Access)Enum.Parse(typeof(Access), updateFT_dto.AccessControl);


        if (formTemplate.Tags.Count > 0)
        {
            dbContextWrapper.Context.FormTags.RemoveRange(formTemplate.Tags);
            // await dbContextWrapper.Context.SaveChangesAsync();

        }
        if (updateFT_dto.Tags.Count() > 0)
        {
            formTemplate.Tags = mapper.Map<List<FormTemplateTag>>(updateFT_dto.Tags);
            // await dbContextWrapper.Context.SaveChangesAsync();

        }


        if (formTemplate.AuthorizedUsers.Count > 0)
        {
            dbContextWrapper.Context.AuthorizedUsers.RemoveRange(formTemplate.AuthorizedUsers);
            // await dbContextWrapper.Context.SaveChangesAsync();

        }
        if (updateFT_dto.AuthorizedUsers.Count() > 0)
        {
            formTemplate.AuthorizedUsers = mapper.Map<List<AuthorizedUser>>(updateFT_dto.AuthorizedUsers);
            // await dbContextWrapper.Context.SaveChangesAsync();

        }

        var updatedBlockIds = updateFT_dto.Blocks.Select(b => b.Id).ToHashSet();
        var deletedBlocks = formTemplate.Blocks.Where(b => !updatedBlockIds.Contains(b.Id)).ToList();
        if (deletedBlocks.Count > 0)
        {
            dbContextWrapper.Context.Blocks.RemoveRange(deletedBlocks);
            // await dbContextWrapper.Context.SaveChangesAsync();
        }


        foreach (var blockDto in updateFT_dto.Blocks)
        {
            var existingBlock = formTemplate.Blocks.FirstOrDefault(b => b.Id == blockDto.Id);
            if (existingBlock != null)
            {

                existingBlock.Title = blockDto.Title ?? existingBlock.Title;
                existingBlock.Description = blockDto.Description ?? existingBlock.Description;
                existingBlock.IsRequired = blockDto.IsRequired;

                // Update question groups
                dbContextWrapper.Context.Questions.RemoveRange(existingBlock.QuestionGroup);
                // await dbContextWrapper.Context.SaveChangesAsync();
                var newQuestionGroups = mapper.Map<List<Question>>(blockDto.QuestionGroup);
                existingBlock.QuestionGroup = newQuestionGroups;
                // await dbContextWrapper.Context.SaveChangesAsync();

                // Update checkbox options
                dbContextWrapper.Context.CheckboxOptions.RemoveRange(existingBlock.CheckboxOptions);
                // await dbContextWrapper.Context.SaveChangesAsync();
                var newOptions = mapper.Map<List<CheckboxOption>>(blockDto.CheckboxOptions);
                existingBlock.CheckboxOptions = newOptions;
                // await dbContextWrapper.Context.SaveChangesAsync();
            }
            else
            {
                // Add new block
                var newBlock = mapper.Map<Block>(blockDto);
                formTemplate.Blocks.Add(newBlock);
            }
        }


        var (statusCode, message) = await dbContextWrapper.SaveChangesAsync();
        if (statusCode != 201)
        {
            return StatusCode(statusCode, message);
        }
        return Ok(mapper.Map<FormTemplate_DTO>(formTemplate));


    }

    [HttpGet("all")]
    public async Task<ActionResult<List<FormTemplateIndex_DTO>>> GetAllFormTemplates()
    {
        var fts = await dbContextWrapper.Context.FormTemplates.ProjectTo<FormTemplateIndex_DTO>(mapper.ConfigurationProvider).ToListAsync();
        return Ok(fts);
    }
}
