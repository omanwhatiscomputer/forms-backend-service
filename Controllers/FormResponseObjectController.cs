using System;
using System.Text.Json;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendService.Data;
using BackendService.DTOs;
using BackendService.DTOs.FormResponseObject;
using BackendService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Controllers;
[ApiController]
[Route("api/formresponse")]
public class FormResponseObjectController(IDbContextWrapper dbContextWrapper, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<FormResponseObject_DTO>> CreateFormResponseObject(CreateFormResponseObject_DTO createFRO_dto)
    {
        var user = await dbContextWrapper.Context.Users.FirstOrDefaultAsync(x => x.UserId == createFRO_dto.RespondentId);
        if (user == null) return NotFound();

        var formResponseObj = mapper.Map<FormResponseObject>(createFRO_dto);

        // foreach(var blockResponse in formResponseObj.BlockResponses){
        //     blockResponse.FormResponseObjectId = formResponseObj.Id;
        // }

        dbContextWrapper.Context.FormResponseObjects.Add(formResponseObj);
        var (statusCode, message) = await dbContextWrapper.SaveChangesAsync();
        if (statusCode != 201)
        {
            return StatusCode(statusCode, message);
        }
        return Ok(mapper.Map<FormResponseObject_DTO>(formResponseObj));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<FormResponseObject_DTO>> UpdateFormResponseObjectByROId(Guid id, UpdateFormResponseObject_DTO updateFRO_dto)
    {
        var formResponseObj = await dbContextWrapper.Context.FormResponseObjects.Include(fro => fro.BlockResponses).FirstOrDefaultAsync(fro => fro.Id == id);
        if (formResponseObj == null)
        {
            return NotFound($"FormResponseObject with ID {id} not found.");
        }

        dbContextWrapper.Context.BlockResponses.RemoveRange(formResponseObj.BlockResponses);
        mapper.Map<UpdateFormResponseObject_DTO, FormResponseObject>(updateFRO_dto, formResponseObj);
        var (statusCode, message) = await dbContextWrapper.SaveChangesAsync();
        if (statusCode != 201)
        {
            return StatusCode(statusCode, message);
        }
        return Ok(mapper.Map<FormResponseObject_DTO>(formResponseObj));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FormResponseObject_DTO>> GetFormTemplateById(Guid id)
    {
        var formResponseObject = await dbContextWrapper.Context.FormResponseObjects
            .Include(fr => fr.BlockResponses)
            .FirstOrDefaultAsync(ft => ft.Id == id);

        if (formResponseObject == null) return NotFound();

        var fro_dto = mapper.Map<FormResponseObject_DTO>(formResponseObject);



        return Ok(fro_dto);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<FormResponseObject_DTO>>> GetAllResponseObjects()
    {
        var fts = await dbContextWrapper.Context.FormResponseObjects.Include(fro => fro.BlockResponses).ProjectTo<FormResponseObject_DTO>(mapper.ConfigurationProvider).ToListAsync();
        return Ok(fts);
    }
    [HttpGet("alluserresponses/{id}")]
    public async Task<ActionResult<List<FormResponseObject_DTO>>> GetAllFormResponsesByRespondentId(Guid id)
    {
        var res = await dbContextWrapper.Context.FormResponseObjects.Where(fro => fro.RespondentId == id).ProjectTo<FormResponseObject_DTO>(mapper.ConfigurationProvider).ToListAsync();
        return Ok(res);
    }

    [HttpGet("allftresponses/{id}")]
    public async Task<ActionResult<List<FormResponseObject_DTO>>> GetAllFormResponsesByParentTemplateId(Guid id)
    {
        var res = await dbContextWrapper.Context.FormResponseObjects.Include(fro => fro.BlockResponses).Where(fro => fro.ParentTemplateId == id).ProjectTo<FormResponseObject_DTO>(mapper.ConfigurationProvider).ToListAsync();
        return Ok(res);
    }

    [HttpGet("formresponseanalytics/{formId}/{blockId}")]
    public async Task<ActionResult> GetFormTemplateResponseAnalytics(Guid formId, Guid blockId)
    {
        var block = await dbContextWrapper.Context.Blocks.FirstOrDefaultAsync(b => b.Id == blockId && b.ParentTemplateId == formId);
        var blockResponses = await dbContextWrapper.Context.BlockResponses.Where(br => br.BlockId == blockId && br.ParentTemplateId == formId).ProjectTo<BlockResponse_DTO>(mapper.ConfigurationProvider).ToListAsync();

        if (block.BlockType == InputType.Integer)
        {
            var arr = blockResponses.Select(br => float.Parse(br.Content));
            var result = new { value = arr.Average().ToString(), type = "Average" };
            return Ok(result);
        }
        else if (block.BlockType == InputType.SingleLine)
        {
            var arr = blockResponses.Select(br => br.Content.ToLower().Trim());
            var result = new
            {
                value = arr.GroupBy(v => v)
                    .OrderByDescending(g => g.Count())
                    .First()
                    .Key,
                type = "Popular answer"
            };
            return Ok(new { value = result, type = "Mode" });

        }
        else if (block.BlockType == InputType.MultiLine)
        {
            var arr = blockResponses.Select(br => br.Content.ToLower().Trim().Split(" "));
            string[] words = [];
            foreach (var item in arr)
            {
                words = words.Concat(item).ToArray();
            }
            string res = JsonSerializer.Serialize(words.GroupBy(w => w).ToDictionary(group => group.Key, group => group.Count()).OrderByDescending(kvp => kvp.Value).Take(5).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            return Ok(new { value = res, type = "Top Words" });
        }
        else if (block.BlockType == InputType.CheckboxSingle)
        {
            var arr = blockResponses.Select(br => br.Content);
            var result = new
            {
                value = arr.GroupBy(v => v)
                    .OrderByDescending(g => g.Count())
                    .First()
                    .Key,
                type = "Popular answer"
            };
            return Ok(result);
        }
        else
        {
            var arr = blockResponses.Select(br => JsonSerializer.Deserialize<List<string>>(br.Content));
            string[] choices = [];
            foreach (var item in arr)
            {
                choices = choices.Concat(item).ToArray();
            }
            string res = JsonSerializer.Serialize(choices.GroupBy(w => w).ToDictionary(group => group.Key, group => group.Count()).OrderByDescending(kvp => kvp.Value).Take(5).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            return Ok(new { value = res, type = "Top Choices" });
        }
    }
    [HttpGet("allftresponseindexes/{id}")]
    public async Task<ActionResult<List<FormResponseObjectIndex_DTO>>> GetAllFormResponseIndexesByParentTemplateId(Guid id)
    {
        var res = await dbContextWrapper.Context.FormResponseObjects.Where(fro => fro.ParentTemplateId == id).ProjectTo<FormResponseObjectIndex_DTO>(mapper.ConfigurationProvider).ToListAsync();
        return Ok(res);
    }
}
