using System;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Entities;

[PrimaryKey(nameof(TopicName)), Index(nameof(TopicName), IsUnique = true)]
public class Topic
{

    public Guid Id {get; set;} = Guid.NewGuid();
    public string TopicName {get; set;}
}
