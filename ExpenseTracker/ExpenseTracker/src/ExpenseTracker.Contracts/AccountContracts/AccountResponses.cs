using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Contracts.AccountContracts
{
    public record AuthenticationResponse(
        int UserId,
        string Username,
        string Email,
        string RealName,
        string RealSurname,
        string AccessToken,
        string RefreshToken,
        DateTime AccessTokenExpiry,
        DateTime RefreshTokenExpiry);

    public record CurrentUserResponse(
        int UserId,
        string Username,
        string Email,
        string RealName,
        string RealSurname);
}
