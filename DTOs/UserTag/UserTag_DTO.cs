using System;
using Microsoft.EntityFrameworkCore;

namespace BackendService.DTOs;

public class UserTag_DTO
{
    public Guid Id {get; set;}
    public Guid UserId { get; set; }
    public string TagName { get; set; }
}
