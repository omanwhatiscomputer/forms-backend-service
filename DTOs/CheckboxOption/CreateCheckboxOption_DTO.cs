using System;

namespace BackendService.DTOs.CheckboxOption;

public class CreateCheckboxOption_DTO
{
    public Guid Id {get; set;} = Guid.NewGuid();
    public Guid BlockId {get; set;}
    public string Content {get; set;}
    public bool IncludesImage {get; set;}
    public string ImageUrl {get; set;}
}
