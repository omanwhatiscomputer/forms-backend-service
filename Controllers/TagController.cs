using System;
using AutoMapper;
using BackendService.Data;
using BackendService.DTOs;
using BackendService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Controllers;

[ApiController]
[Route("api/tag")]
public class TagController(IDbContextWrapper dbContextWrapper, IMapper mapper) : ControllerBase
{

    // ALL DONE:
    // register tag with user id :: DONE
    // delete tag by Tag name with user id :: before deleting remove tags from existing forms
    // add tag to form (tag, userid -to-> formid)
    // remove tag from form (tag, userid -to-> formid)

    [HttpPost]
    public async Task<ActionResult<UserTag_DTO>> RegisterTagWithUserId(CreateUserTag_DTO createUT_dto)
    {
        var userTag = mapper.Map<UserTag>(createUT_dto);
        dbContextWrapper.Context.UserTags.Add(userTag);

        var user = await dbContextWrapper.Context.Users.FirstOrDefaultAsync(x => x.UserId == createUT_dto.UserId);
        if (user == null) return NotFound();

        var (statusCode, message) = await dbContextWrapper.SaveChangesAsync();
        if (statusCode != 201)
        {
            return StatusCode(statusCode, message);
        }
        return Ok(mapper.Map<UserTag_DTO>(userTag));
    }

    [HttpPost("form")]
    public async Task<ActionResult<FormTemplateTag_DTO>> AddTagToForm(CreateFormTemplateTag_DTO createFTT_dto)
    {
        var user = await dbContextWrapper.Context.Users.FirstOrDefaultAsync(x => x.UserId == createFTT_dto.AuthorId);
        if (user == null) return NotFound();

        var userTag = await dbContextWrapper.Context.UserTags.FirstOrDefaultAsync(x =>
                x.UserId == createFTT_dto.AuthorId &&
                x.TagName == createFTT_dto.TagName
        );
        if (userTag == null) return NotFound();


        var formTemplateTag = new FormTemplateTag() { UserTagId = userTag.Id, FormTemplateId = createFTT_dto.FormTemplateId };
        dbContextWrapper.Context.FormTags.Add(formTemplateTag);

        var (statusCode, message) = await dbContextWrapper.SaveChangesAsync();
        if (statusCode != 201)
        {
            return StatusCode(statusCode, message);
        }
        return Ok(mapper.Map<FormTemplateTag_DTO>(formTemplateTag));
    }

    [HttpDelete("form")]
    public async Task<ActionResult<FormTemplateTag_DTO>> RemoveTagFromForm(DeleteFormTemplateTag_DTO deleteFTT_dto)
    {
        var user = await dbContextWrapper.Context.Users.FirstOrDefaultAsync(x => x.UserId == deleteFTT_dto.AuthorId);
        if (user == null) return NotFound();

        var userTag = await dbContextWrapper.Context.UserTags.FirstOrDefaultAsync(x =>
                x.UserId == deleteFTT_dto.AuthorId &&
                x.TagName == deleteFTT_dto.TagName
        );
        if (userTag == null) return NotFound();

        var formTemplateTag = await dbContextWrapper.Context.FormTags.FirstOrDefaultAsync(x =>
            x.FormTemplateId == deleteFTT_dto.FormTemplateId &&
            x.UserTagId == userTag.Id
        );
        if (formTemplateTag == null) return NotFound();

        dbContextWrapper.Context.FormTags.Remove(formTemplateTag);
        var (statusCode, message) = await dbContextWrapper.SaveChangesAsync();
        if (statusCode != 201)
        {
            return StatusCode(statusCode, message);
        }
        return Ok(mapper.Map<FormTemplateTag_DTO>(formTemplateTag));
    }

    [HttpDelete]
    public async Task<ActionResult<UserTag_DTO>> DeleteUserTag(DeleteUserTag_DTO deleteUT_dto)
    {
        var user = await dbContextWrapper.Context.Users.FirstOrDefaultAsync(x => x.UserId == deleteUT_dto.UserId);
        if (user == null) return NotFound();

        var userTag = await dbContextWrapper.Context.UserTags.FirstOrDefaultAsync(x =>
                x.UserId == deleteUT_dto.UserId &&
                x.TagName == deleteUT_dto.TagName
        );
        if (userTag == null) return NotFound();

        var formTags = await dbContextWrapper.Context.FormTags.Where(x => x.UserTagId == userTag.Id).ToListAsync();
        dbContextWrapper.Context.FormTags.RemoveRange(formTags);
        dbContextWrapper.Context.UserTags.Remove(userTag);

        var (statusCode, message) = await dbContextWrapper.SaveChangesAsync();
        if (statusCode != 201)
        {
            return StatusCode(statusCode, message);
        }
        return Ok(mapper.Map<UserTag_DTO>(userTag));
    }

    // getAlltagsByUserId
    // getAllTagsByFormId
    [HttpGet("{id}")]
    public async Task<ActionResult<List<UserTag_DTO>>> GetAllTagsByUserId(Guid id)
    {
        var user = await dbContextWrapper.Context.Users.FirstOrDefaultAsync(x => x.UserId == id);
        if (user == null) return NotFound();

        var userTags = await dbContextWrapper.Context.UserTags.Where(tag => tag.UserId == id)
            .Select(tag => new UserTag_DTO { Id = tag.Id, TagName = tag.TagName, UserId = tag.UserId })
            .ToListAsync();
        return Ok(userTags);
    }

    [HttpGet("form/{id}")]
    public async Task<ActionResult<List<UserTag_DTO>>> GetAllTagsByFormId(Guid id)
    {
        var template = await dbContextWrapper.Context.FormTemplates.FirstOrDefaultAsync(x => x.Id == id);
        if (template == null) return NotFound();

        var templateTags = await dbContextWrapper.Context.FormTags.Where(tag => tag.FormTemplateId == id)
            .Select(tag => new FormTemplateTag_DTO { UserTagId = tag.UserTagId, Id = tag.Id, UserTag = mapper.Map<UserTag_DTO>(tag.UserTag), FormTemplateId = tag.FormTemplateId })
            .ToListAsync();
        return Ok(templateTags);
    }

}
