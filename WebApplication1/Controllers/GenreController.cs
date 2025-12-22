using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Services;

namespace WebApplication1.Controllers;



[Route("api/[controller]")]
[ApiController]
public class GenreController( IGenreService genreService) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetAll()
    {
        var genres = await genreService.GetAllAsync();
        return Ok(genres);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GenreDto>> GetById(int id)
    {
        var genre = await genreService.GetByIdAsync(id);
        
        return Ok(genre);
    }
}

