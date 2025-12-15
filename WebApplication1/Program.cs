using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Register EF Core DbContext using connection string from appsettings
var connString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<GameStoreContext>(options =>
    options.UseSqlServer(connString));

builder.Services.AddScoped<IGameService, GameService>();
builder.Services
    .AddIdentityApiEndpoints<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<GameStoreContext>();

var app = builder.Build();



//app.UseHttpsRedirection();
app.UseAuthorization();

// Use controller routes (replace in-memory endpoints)
app.MapControllers();

app.MapIdentityApi<IdentityUser>();
app.MigrateDb();
app.Run();
