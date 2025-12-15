using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Entities;

namespace WebApplication1.Services;

public class GameService(GameStoreContext dbContext) : IGameService
{
    public async Task<IEnumerable<GameDto>> GetAllAsync()
    {
        return await dbContext.Games
            .Select(g => new GameDto(
                g.Id,
                g.Name,
                g.Genre != null ? g.Genre.Name : string.Empty,
                g.Price,
                g.ReleaseDate))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<GameDto?> GetByIdAsync(int id)
    {
        var game = await dbContext.Games
            .Include(game => game.Genre)
            .FirstOrDefaultAsync(game => game.Id == id);

        if (game is null) return null;

        return new GameDto(
            game.Id,
            game.Name,
            game.Genre != null ? game.Genre.Name : string.Empty,
            game.Price,
            game.ReleaseDate);
    }

    public async Task<GameDto> CreateAsync(CreateGameDto newGame)
    {
        var genreEntity = await GetOrCreateGenreAsync(newGame.Genre);

        var entity = new Game
        {
            Name = newGame.Name,
            Genre = genreEntity,
            Price = newGame.Price,
            ReleaseDate = newGame.ReleaseDate
        };

        dbContext.Games.Add(entity);
        await dbContext.SaveChangesAsync();

        return new GameDto(
            entity.Id,
            entity.Name,
            entity.Genre != null ? entity.Genre.Name : string.Empty,
            entity.Price,
            entity.ReleaseDate);
    }

    public async Task<bool> UpdateAsync(int id, UpdateGameDto updatedGame)
    {
        var existingGame = await dbContext.Games.FindAsync(id);

        if (existingGame is null) return false;

        var genreEntity = await GetOrCreateGenreAsync(updatedGame.Genre);

        existingGame.Name = updatedGame.Name;
        existingGame.Genre = genreEntity;
        existingGame.Price = updatedGame.Price;
        existingGame.ReleaseDate = updatedGame.ReleaseDate;

        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var game = await dbContext.Games.FindAsync(id);
        if (game is null) return false;

        dbContext.Games.Remove(game);
        await dbContext.SaveChangesAsync();
        return true;
    }

    // Helper method to avoid code repetition for Genre checks
    private async Task<Genre?> GetOrCreateGenreAsync(string genreName)
    {
        if (string.IsNullOrWhiteSpace(genreName)) return null;

        var genreEntity = await dbContext.Genres.FirstOrDefaultAsync(g => g.Name == genreName);

        if (genreEntity is null)
        {
            genreEntity = new Genre { Name = genreName };
            dbContext.Genres.Add(genreEntity);
            // Note: We don't necessarily need to SaveChanges here if we save the Game later,
            // but it's safe to let EF Core track it.
        }

        return genreEntity;
    }
}