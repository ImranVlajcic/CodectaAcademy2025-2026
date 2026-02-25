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

    // Login request from client
    public record LoginRequest(
        string Email,
        string Password);

    // Refresh token request from client
    public record RefreshTokenRequest(
        string RefreshToken);
}
