using System;
using AutoMapper;
using BackendService.Data;
using BackendService.DTOs;
using BackendService.RequestHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Controllers;

[ApiController]
[Route("api/auth")]
public class IdentityController(IDbContextWrapper dbContextWrapper, IMapper mapper): ControllerBase
{
    [AllowAnonymous]
    [HttpGet("status")]
    public async Task<ActionResult<User_DTO>> CheckAuthStatus(){
        var jwt = Request.Cookies["jwt"];
        var userId = Request.Cookies["userId"];
        if (string.IsNullOrEmpty(jwt)) return Unauthorized(new { isAuthenticated = false });

        var principal = JwtHelpers.ValidateJwt(jwt);
        if (principal == null) return Unauthorized(new { isAuthenticated = false });

        var id = principal.FindFirst("userId")?.Value;

        // check id equal and user with id exists
        var user = await dbContextWrapper.Context.Users.Include(u => u.Tags).FirstOrDefaultAsync(x => x.UserId == Guid.Parse(id));

        if(id != userId || user == null) return NotFound();
        return Ok(mapper.Map<User_DTO>(user));
    }

}
