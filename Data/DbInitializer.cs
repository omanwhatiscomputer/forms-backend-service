using System;
using BackendService.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Data;

public class DbInitializer
{
    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>()
            ?? throw new InvalidOperationException("Failed to retrieve ApplicationDbContext from the service provider.");

        context.Database.Migrate();

        SeedTopicsTable(context);

    }

    public static void SeedTopicsTable(ApplicationDbContext dbContext){
        if (dbContext.Topics.Any()){
            Console.WriteLine("Topics table already seeded!");
            return;
        }

        var topics = new List<Topic>{ 
            new() {Id = Guid.NewGuid(), TopicName= "Education"},
            new() {Id = Guid.NewGuid(), TopicName= "Quiz"},
            new() {Id = Guid.NewGuid(), TopicName= "Other"},
        };

        dbContext.AddRange(topics);
        dbContext.SaveChanges();
    }
}
