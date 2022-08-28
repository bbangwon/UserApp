namespace UserApp.Models
{
    public interface IUserRepository
    {
        void AddUser(string userId, string password);
        bool IsCorrectUser(string userId, string password);
    }
}
