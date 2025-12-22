using WebApplication1.Dtos;

namespace WebApplication1.Services;

    public interface IGenreService
    {
        Task<IEnumerable<GenreDto>> GetAllAsync();
        Task<GenreDto?> GetByIdAsync(int id);
}

