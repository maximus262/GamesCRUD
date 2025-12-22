using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos;

namespace WebApplication1.Services;

public class GenreService : IGenreService
{
    private readonly GameStoreContext _dbContext;

    public GenreService(GameStoreContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<GenreDto>> GetAllAsync()
    {
        return await (_dbContext.Genres
             .Select(g => new GenreDto(
                 g.Id,
                 g.Name
             ))
             .AsNoTracking()
             .ToListAsync());
        ;
    }
    public async Task<GenreDto?> GetByIdAsync(int id)
    {
        var genre = await _dbContext.Genres
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == id);
        return genre is null ? null : new GenreDto(genre.Id, genre.Name);
    }
}

