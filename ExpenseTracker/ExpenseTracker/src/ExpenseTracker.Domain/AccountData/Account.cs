namespace ExpenseTracker.Domain.AccountData

{
    public class Account
    {
        public int userID { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string accPassword { get; set; }
        public string realName { get; set; }
        public string realSurname { get; set; }
        public string phoneNumber { get; set; }

        public Account() { }

        public Account (int userId, string username, string email, string accPassword, string realName, string realSurname, string phoneNumber)
        {
            this.userID = userId;
            this.username = username;
            this.email = email;
            this.accPassword = accPassword;
            this.realName = realName;
            this.realSurname = realSurname;
            this.phoneNumber = phoneNumber;
        }
    }
}
