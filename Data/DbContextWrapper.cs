using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BackendService.Data;

public interface IDbContextWrapper : IDisposable
{
    ApplicationDbContext Context { get; }
    Task<(int, string)> SaveChangesAsync();
}

public class DbContextWrapper(ApplicationDbContext context) : IDbContextWrapper
{
    private readonly ApplicationDbContext _context = context;
    public ApplicationDbContext Context => _context;

    public async Task<(int, string)> SaveChangesAsync()
    {
        try
        {
            var result = await _context.SaveChangesAsync();
            return result > 0 ? (201, "Success!") : (500, "Internal Error!");
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
            {
                return (409, "Resource already exists!");
            }

            Console.WriteLine($"[Server ERROR]: Source - {ex.Source}");
            Console.WriteLine($"[Server ERROR]: HResult - {ex.HResult}");
            Console.WriteLine($"[Server ERROR]: InnerException - {ex.InnerException}");
            Console.WriteLine($"[Server ERROR]: StackTrace - {ex.StackTrace}");

            return (500, $"Internal Error: {ex.Message}");
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
