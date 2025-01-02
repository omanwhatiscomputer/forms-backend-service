using System;

namespace BackendService.Entities;

public class CheckboxOption
{
    public Guid Id {get; set;}
    public Guid BlockId {get; set;}
    public Block Block {get; set;} = null!;
    public string Content {get; set;}
    public bool IncludesImage {get; set;}
    public string ImageUrl {get; set;}
    
}
