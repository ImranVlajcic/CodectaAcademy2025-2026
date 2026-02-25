namespace ExpenseTracker.Domain.AccountData

{
    public class Account
    {
        public int userID { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string passwordHash { get; set; }
        public string realName { get; set; }
        public string realSurname { get; set; }
        public string phoneNumber { get; set; }


        public DateTime createdAt { get; set; }
        public DateTime? lastLoginAt { get; set; }
        public bool isActive { get; set; }
        public string? refreshToken { get; set; }
        public DateTime? refreshTokenExpiryTime { get; set; }

        public Account() {
            username = string.Empty;
            email = string.Empty;
            passwordHash = string.Empty;
            realName = string.Empty;
            realSurname = string.Empty;
            phoneNumber = string.Empty;
            isActive = true;
            createdAt = DateTime.UtcNow;
        }

        public Account (int userId, string username, string email, string passwordHash, string realName, string realSurname, string phoneNumber)
        {
            this.userID = userId;
            this.username = username;
            this.email = email;
            this.passwordHash = passwordHash;
            this.realName = realName;
            this.realSurname = realSurname;
            this.phoneNumber = phoneNumber;
            this.isActive = true;
            this.createdAt = DateTime.UtcNow;
        }
    }
}
