using System;

namespace BackendService.DTOs;

public class CreateUserTag_DTO
{
    public Guid Id {get; set;} = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string TagName { get; set; }

}
