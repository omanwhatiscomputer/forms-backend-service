using System;

namespace BackendService.DTOs.FormTemplate;

public class FormTemplateIndex_DTO
{
    public string FormTemplateId { get; set; }
    public string Title { get; set; }
    public Guid AuthorId { get; set; }
    public string Description { get; set; }
    public string BannerUrl { get; set; }
}
