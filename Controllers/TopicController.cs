using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendService.Data;
using BackendService.DTOs;
using BackendService.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BackendService.Controllers;

[ApiController]
[Route("api/topic")]
public class TopicController(IDbContextWrapper dbContextWrapper, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Topic_DTO>> CreateTopic(CreateTopic_DTO createTopic_dto)
    {
        var topic = mapper.Map<Topic>(createTopic_dto);
        _ = dbContextWrapper.Context.Topics.Add(topic);

        var (statusCode, message) = await dbContextWrapper.SaveChangesAsync();

        
        if(statusCode != 201){
            return StatusCode(statusCode, message);
        }
        return Ok(mapper.Map<Topic_DTO>(topic)); 
    }

    [HttpGet]
    public async Task<ActionResult<List<Topic_DTO>>> GetAllTopics()
    {
        var query = dbContextWrapper.Context.Topics.OrderBy(t => t.TopicName).AsQueryable();
        var topics = await query.ProjectTo<Topic_DTO>(mapper.ConfigurationProvider).ToListAsync();
        return Ok(topics);
    }

    [HttpDelete("{topic}")]
    public async Task<ActionResult<Topic_DTO>> DeleteTopicByName(string _topic)
    {
        var topic = await dbContextWrapper.Context.Topics.FirstOrDefaultAsync(t => t.TopicName == _topic.Trim());
        if(topic == null) return NotFound();

        dbContextWrapper.Context.Topics.Remove(topic);
        var (statusCode, message) = await dbContextWrapper.SaveChangesAsync();
        
        if(statusCode != 201) return BadRequest("An unexpected error occurred while deleting topic!");
        var topic_dto = mapper.Map<Topic_DTO>(topic);

        return Ok(topic_dto);
    }
}
