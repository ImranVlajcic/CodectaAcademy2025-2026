using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Contracts.AccountContracts
{
    public record RegisterRequest(
        string Username,
        string Email,
        string Password,
        string RealName,
        string RealSurname,
        string? PhoneNumber);

    public record LoginRequest(
        string Email,
        string Password);

    public record RefreshTokenRequest(
        string RefreshToken);
}
