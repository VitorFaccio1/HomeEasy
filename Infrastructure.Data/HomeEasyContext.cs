using Infrastructure.Data.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class HomeEasyContext : DbContext
    {
        public HomeEasyContext(DbContextOptions<HomeEasyContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
    }
}