using System;

namespace BackendService.DTOs;

public class DeleteUserTag_DTO
{
    public Guid UserId { get; set; }
    public string TagName { get; set; }
}
