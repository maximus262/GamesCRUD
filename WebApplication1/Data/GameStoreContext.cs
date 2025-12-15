using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace WebApplication1.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) : IdentityDbContext(options)
{ 
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();

}