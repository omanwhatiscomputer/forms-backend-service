using System;


namespace BackendService.DTOs;

public class Like_DTO
{
    public Guid Id {get; set;} = Guid.NewGuid();
    public Guid UserId {get; set;}
    public string NormalizedName {get; set;}
}
