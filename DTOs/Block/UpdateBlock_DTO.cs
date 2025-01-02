using System;
using BackendService.DTOs.CheckboxOption;

namespace BackendService.DTOs;

public class UpdateBlock_DTO
{

    public Guid Id { get; set; } = Guid.NewGuid();
    public int Index { get; set; }
    public Guid ParentTemplateId { get; set; }
    public string BlockType { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public List<UpdateCheckboxOption_DTO> CheckboxOptions { get; set; }

    public bool IsRequired { get; set; }

    public List<UpdateQuestion_DTO> QuestionGroup { get; set; }

}
