using System;

namespace BackendService.DTOs;

public class UpdateQuestion_DTO
{

    public Guid BlockId { get; set; }
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Content { get; set; }
}
