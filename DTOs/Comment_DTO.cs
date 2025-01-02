using System;

namespace BackendService.DTOs;

public class Comment_DTO
{
    public Guid Id {get; set;}
    public string Content {get; set;}
    public DateTime CreatedAt {get; set;}
}
