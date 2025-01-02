using System;

namespace BackendService.DTOs;

public class CreateQuestion_DTO
{
    public Guid BlockId{get; set;}
    public Guid Id {get; set;} = Guid.NewGuid();
    public string Type {get; set;}
    public string Content {get; set;}
}
