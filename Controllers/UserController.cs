using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendService.Data;
using BackendService.DTOs;
using BackendService.DTOs.User;
using BackendService.Entities;
using BackendService.RequestHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IDbContextWrapper dbContextWrapper, IMapper mapper, IConfiguration config) : ControllerBase
{

    // TODO: configure delete endpoint

    [HttpPost("logout")]
    public ActionResult Logout()
    {
        Response.Cookies.Append("jwt", "", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddDays(-1)
        });
        Response.Cookies.Append("userId", "", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddDays(-1)
        });
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<User_DTO>> Register(CreateUser_DTO createU_dto)
    {
        
        byte[] salt;
        string passwordHash = PasswordHelpers.HashPassword(createU_dto.Password, out salt);
        var user = mapper.Map<User>(createU_dto);
        user.PasswordSalt = PasswordHelpers.SaltConvertBytes2HexString(salt);
        user.PasswordHash = passwordHash;
        user.NormalizedName = (user.FirstName + " " + user?.LastName).Trim();
        dbContextWrapper.Context.Users.Add(user);

        var (statusCode, message) = await dbContextWrapper.SaveChangesAsync();
        if(statusCode != 201){
            return StatusCode(statusCode, message);
        }

        string token = UserHelpers.GetUserToken(user, config);
        UserHelpers.AppendCookies(Response, config, user, token);

        var user_dto = mapper.Map<User_DTO>(user);
        // return Ok(user_dto);
        return CreatedAtAction(nameof(Login), new {user.UserId}, user_dto);
    }


    [AllowAnonymous]
    [HttpPost("auth")]
    public async Task<ActionResult<User_DTO>> Login(LogIn_DTO logIn_dto)
    {
        var user = await dbContextWrapper.Context.Users.FirstOrDefaultAsync(x => x.Email == logIn_dto.Email);
        if(user == null) return NotFound("User not found!");
        if (!PasswordHelpers.HashesMatch(logIn_dto.Password, user.PasswordSalt, user.PasswordHash))
        {
            return Unauthorized("Invalid Credentials");
        }
        
        string token = UserHelpers.GetUserToken(user, config);

        if (!PasswordHelpers.PasswordSaltIsValid(user.PasswordSaltRenewedAt, config)){
            byte[] newSalt;
            string newPasswordHash = PasswordHelpers.HashPassword(logIn_dto.Password, out newSalt);
            user.PasswordSalt = PasswordHelpers.SaltConvertBytes2HexString(newSalt);
            user.PasswordHash = newPasswordHash;
            user.PasswordSaltRenewedAt = DateTime.UtcNow;
        }

        

        var result = await dbContextWrapper.Context.SaveChangesAsync() > 0;
        if(!result) return BadRequest("Could not save changes to the DB.");

        UserHelpers.AppendCookies(Response, config, user, token);

        var user_dto = mapper.Map<User_DTO>(user);
        

        return Ok(user_dto);
    }



    [HttpPut("{id}")]
    public async Task<ActionResult<User_DTO>> UpdateUserById(Guid id, UpdateUser_DTO updateU_dto)
    {
        var user = await dbContextWrapper.Context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if(user==null) return NotFound();

        mapper.Map<UpdateUser_DTO, User>(updateU_dto, user);
        if (updateU_dto.FirstName != null || updateU_dto.LastName != null){
            user.NormalizedName = (user.FirstName + " " + user.LastName).Trim();
        }

        var (statusCode, message) = await dbContextWrapper.SaveChangesAsync();
        if(statusCode != 201){
            return StatusCode(statusCode, message);
        }
        var user_dto = mapper.Map<User_DTO>(user);
        return Ok(user_dto);
    }

    // TODO: Add other collections to the user DTO as you implement them: similar to userTags/FormTemplates
    // i.e. Implement lazy loading: .Include() 
    [HttpGet("{id}")]
    public async Task<ActionResult<User_DTO>> GetUserById(Guid id)
    {
        var user = await dbContextWrapper.Context.Users.Include(u => u.Tags).Include(u => u.FormTemplates).FirstOrDefaultAsync(x => x.UserId == id);
        if(user == null) return NotFound();
        var user_dto = mapper.Map<User_DTO>(user);
        return Ok(user_dto);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<UserIndex_DTO>>> GetAllUsers()
    {
        var users = await dbContextWrapper.Context.Users.ProjectTo<UserIndex_DTO>(mapper.ConfigurationProvider).ToListAsync();
        return Ok(users);
    }

}
