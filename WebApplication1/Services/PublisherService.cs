using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos;

namespace WebApplication1.Services;

public class PublisherService(GameStoreContext dbContext) : IPublisherService
{
    public async Task<IEnumerable<PublisherDto>> GetAllAsync()
    {
        return await dbContext.Publishers
            .Select(p => new PublisherDto(p.Id, p.Name, p.Country)) // Mapping here
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<PublisherDto?> GetByIdAsync(int id)
    {
        var p = await dbContext.Publishers
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        return p is null ? null : new PublisherDto(p.Id, p.Name, p.Country);
    }

    public async Task<PublisherDto> CreateAsync(CreatePublisherDto newPublisher)
    {
        // Use the full namespace for the Entity to avoid ambiguity
        var entity = new WebApplication1.Entities.Publisher
        {
            Name = newPublisher.Name,
            Country = newPublisher.Country
        };

        dbContext.Publishers.Add(entity);
        await dbContext.SaveChangesAsync();

        return new PublisherDto(entity.Id, entity.Name, entity.Country);
    }
}