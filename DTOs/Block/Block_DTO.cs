using System;
using BackendService.DTOs.CheckboxOption;

namespace BackendService.DTOs;

public class Block_DTO
{
    public Guid Id { get; set; }
    public int Index { get; set; }
    public string BlockType { get; set; }
    public Guid ParentTemplateId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public List<CheckboxOption_DTO> CheckboxOptions { get; set; }
    public bool IsRequired { get; set; }

    public List<Question_DTO> QuestionGroup { get; set; }

}
