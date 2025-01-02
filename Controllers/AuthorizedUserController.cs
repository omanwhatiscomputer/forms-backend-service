using System;
using AutoMapper;
using BackendService.Data;
using BackendService.DTOs;
using BackendService.DTOs.AuthorizedUSer;
using BackendService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Controllers;


[ApiController]
[Route("api/formauth")]
public class AuthorizedUserController(IDbContextWrapper dbContextWrapper, IMapper mapper) : ControllerBase
{
    // Add auth user
    // remove auth user
    public async Task<ActionResult<AuthorizedUser_DTO>> RegisterUserToForm(CreateAuthorizedUser_DTO caUser_dto)
    {
        var user = await dbContextWrapper.Context.Users.FirstOrDefaultAsync(x => x.UserId == caUser_dto.AuthorId);
        if(user == null) return NotFound();

        var authorizedUser = mapper.Map<AuthorizedUser>(caUser_dto);
        dbContextWrapper.Context.AuthorizedUsers.Add(authorizedUser);
        var (statusCode, message) = await dbContextWrapper.SaveChangesAsync();
        
        if(statusCode != 201){
            return StatusCode(statusCode, message);
        }
        return Ok(mapper.Map<AuthorizedUser_DTO>(authorizedUser));
        
    }


}
