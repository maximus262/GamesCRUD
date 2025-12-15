using WebApplication1.Dtos;

namespace WebApplication1.Services;

public interface IGameService
{
    Task<IEnumerable<GameDto>> GetAllAsync();
    Task<GameDto?> GetByIdAsync(int id);
    Task<GameDto> CreateAsync(CreateGameDto newGame);
    Task<bool> UpdateAsync(int id, UpdateGameDto updatedGame);
    Task<bool> DeleteAsync(int id);
}