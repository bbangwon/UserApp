using UserApp.Data;
using UserApp.Models;

namespace UserApp.Common
{
    public interface ILoginFailedManager
    {
        bool IsLoginFailed(string username);
        void ClearLoginFailed(string username);
        void UpdateLoginCount(string username);
    }

    public class LoginFailedManager : ILoginFailedManager
    {
        private readonly ILoginFailedRepository repository;

        public LoginFailedManager(ILoginFailedRepository repository)
        {
            this.repository = repository;
        }
        public void ClearLoginFailed(string username)
        {
            this.repository.ClearLogin(username);
        }

        //계정잠금
        public bool IsLoginFailed(string username)
        {
            if(this.repository.IsLoginUser(username))
            {
                //카운터가 5, 최근 10분내 로그인 시도?
                if (this.repository.IsFiveOverCount(username) && 
                    this.repository.IsLastLoginTenMinute(username))
                {
                    return true;    //계정 잠금
                }
                else if(this.repository.IsFiveOverCount(username) && 
                    !this.repository.IsLastLoginTenMinute(username))   //카운터가 5 이상이고 최근 10분이 지났으면 클리어
                {
                    this.repository.ClearLogin(username);
                    return false;
                }
                else
                {
                    return false;   //아직 계정 잠금 전
                }
            }
            else
            {
                //처음 로그인
                this.repository.AddLogin(new UserLogs
                {
                    Username= username,
                });
                return false;
            }
        }

        public void UpdateLoginCount(string username)
        {
            this.repository.UpdateLoginCount(username);
        }
    }

    public interface ILoginFailedRepository
    {
        UserLogs AddLogin(UserLogs model);
        void ClearLogin(string username);
        bool IsLoginUser(string username);
        void UpdateLoginCount(string username);
        bool IsFiveOverCount(string username);
        bool IsLastLoginTenMinute(string username);
    }

    public class LoginFailedRepository : ILoginFailedRepository
    {
        private readonly UserDbContext context;

        public LoginFailedRepository(UserDbContext context)
        {
            this.context = context;
        }

        public UserLogs AddLogin(UserLogs model)
        {
            context.UserLogs?.Add(model);
            context.SaveChanges();
            return model;
        }

        public void ClearLogin(string username)
        {            
            var userLog = context.UserLogs?.FirstOrDefault(ul => ul.Username == username);
            if(userLog != null)
            {
                userLog.FailedPasswordAttemptCount = 0;
                userLog.FailedPasswordAttemptWindowStart = DateTime.Now;
                context.UserLogs?.Update(userLog);
                context.SaveChanges();
            }
        }

        public bool IsFiveOverCount(string username)
        {
            var userLog = context.UserLogs?.FirstOrDefault(ul => ul.Username == username);
            if(userLog != null)
            {
                return userLog.FailedPasswordAttemptCount >= 5;
            }
            return false;            
        }

        public bool IsLastLoginTenMinute(string username)
        {
            var userLog = context.UserLogs?.FirstOrDefault(ul => ul.Username == username);
            if (userLog != null)
            {
                TimeSpan timeSpan = DateTime.Now - userLog.FailedPasswordAttemptWindowStart;
                return timeSpan.TotalSeconds <= 10 * 60;
            }
            return false;
        }

        public bool IsLoginUser(string username)
        {
            var userLogCount = context.UserLogs?.Count(ul => ul.Username == username);
            return userLogCount > 0;
        }

        public void UpdateLoginCount(string username)
        {
            var userLog = context.UserLogs?.FirstOrDefault(ul => ul.Username == username);
            if (userLog != null)
            {
                userLog.FailedPasswordAttemptCount++;
                userLog.FailedPasswordAttemptWindowStart = DateTime.Now;
                context.UserLogs?.Update(userLog);
                context.SaveChanges();
            }
        }
    }
}
