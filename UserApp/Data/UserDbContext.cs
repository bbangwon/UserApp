using Microsoft.EntityFrameworkCore;
using UserApp.Models;

namespace UserApp.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) 
            : base(options)
        {
            
        }

        public DbSet<UserViewModel>? UserViewModels { get; set; }
        public DbSet<UserLogs>? UserLogs { get; set; }
    }
}
