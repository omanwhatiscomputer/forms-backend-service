using System;
using BackendService.Entities;

namespace BackendService.DTOs;

public class Question_DTO
{
    public Guid Id { get; set; }
    public Guid BlockId { get; set; }
    public string Type { get; set; }
    public string Content { get; set; }
}
