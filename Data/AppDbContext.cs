using Microsoft.EntityFrameworkCore;
using mapa_asp.net.Models;


namespace mapa_asp.net.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Maquina> Maquinas { get; set; }
    }
}