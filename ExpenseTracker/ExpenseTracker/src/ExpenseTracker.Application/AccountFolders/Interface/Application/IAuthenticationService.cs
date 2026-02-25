using ErrorOr;

namespace ExpenseTracker.Application.AccountFolders.Interface.Application
{
    public interface IAuthenticationService
    {
        Task<ErrorOr<AuthenticationResult>> RegisterAsync(
            string username,
            string email,
            string password,
            string realName,
            string realSurname,
            string phoneNumber,
            CancellationToken token);

        Task<ErrorOr<AuthenticationResult>> LoginAsync(
            string email,
            string password,
            CancellationToken token);

        Task<ErrorOr<AuthenticationResult>> RefreshTokenAsync(
            string refreshToken,
            CancellationToken token);

        Task<ErrorOr<Success>> RevokeTokenAsync(
            int userId,
            CancellationToken token);
    }

    public record AuthenticationResult(
        int UserId,
        string Username,
        string Email,
        string RealName,
        string RealSurname,
        string AccessToken,
        string RefreshToken,
        DateTime AccessTokenExpiry,
        DateTime RefreshTokenExpiry);
}
