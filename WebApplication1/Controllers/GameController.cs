using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController(IGameService gameService) : ControllerBase
{
    // GET: api/games
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameDto>>> GetAll()
    {
        var games = await gameService.GetAllAsync();
        return Ok(games);
    }

    // GET: api/games/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GameDto>> GetById(int id)
    {
        var game = await gameService.GetByIdAsync(id);
        return game is null ? NotFound() : Ok(game);
    }

    // POST: api/games
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GameDto>> Create(CreateGameDto newGame)
    {
        var createdGame = await gameService.CreateAsync(newGame);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdGame.Id },
            createdGame);
    }

    // PUT: api/games/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateGameDto updatedGame)
    {
        var updated = await gameService.UpdateAsync(id, updatedGame);

        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/games/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await gameService.DeleteAsync(id);

        return deleted ? NoContent() : NotFound();
    }
}