using System;
using AutoMapper;
using BackendService.Data;
using BackendService.DTOs;
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

}
