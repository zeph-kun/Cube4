using Cube_4.models;
using Microsoft.EntityFrameworkCore;

namespace Cube_4.Datas
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        
        public DbSet<Commande> Commandes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        
        }
    }
}