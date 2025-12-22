namespace WebApplication1.Services;

using WebApplication1.Dtos;

public interface IPublisherService
{
    Task<IEnumerable<PublisherDto>> GetAllAsync();
    Task<PublisherDto?> GetByIdAsync(int id);
    Task<PublisherDto> CreateAsync(CreatePublisherDto newPublisher);
}