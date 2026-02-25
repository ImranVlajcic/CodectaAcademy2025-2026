using ExpenseTracker.Domain.AccountData;

namespace ExpenseTracker.Application.AccountFolders.Interface.Application
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(Account account);
        string GenerateRefreshToken();
        int? ValidateRefreshToken(string token);
    }
}
