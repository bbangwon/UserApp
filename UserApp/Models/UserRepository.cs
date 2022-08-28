using Microsoft.EntityFrameworkCore;
using UserApp.Data;

namespace UserApp.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext dbContext;

        public UserRepository(UserDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddUser(string userId, string password)
        {
            dbContext.UserViewModels?.Add(
                new UserViewModel
                {
                    UserId = userId,
                    Password = password,
                });
            dbContext.SaveChanges();
        }   
        
        public bool IsCorrectUser(string userId, string password)
        {
            return dbContext.UserViewModels?.Any(x => x.UserId == userId && x.Password == password) ?? false;
        }
    }
}
