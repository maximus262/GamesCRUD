using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        [Precision(18,2)]
        public decimal Price { get; set; }
        public Genre? Genre { get; set; }
        public DateOnly ReleaseDate { get; set; }
    }
}
