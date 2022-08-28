namespace UserApp.Models
{
    public class UserLogs
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public int FailedPasswordAttemptCount { get; set; } = 0;
        public DateTime FailedPasswordAttemptWindowStart { get; set; } = DateTime.Now;
    }
}
