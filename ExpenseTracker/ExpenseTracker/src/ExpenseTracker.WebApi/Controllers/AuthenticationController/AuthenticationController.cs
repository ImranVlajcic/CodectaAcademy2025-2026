using ExpenseTracker.Application.AccountFolders.Interface.Application;
using ExpenseTracker.Contracts.AccountContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Contracts.AccountContracts;

namespace ExpenseTracker.WebApi.Controllers.AuthenticationController
{
    [Route("api/[controller]")]
    public class AuthenticationController : ApiControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(
            IAuthenticationService authenticationService,
            ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(
            [FromBody] Contracts.AccountContracts.RegisterRequest request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Register endpoint called for email: {Email}", request.Email);

            var result = await _authenticationService.RegisterAsync(
                request.Username,
                request.Email,
                request.Password,
                request.RealName,
                request.RealSurname,
                request.PhoneNumber ?? string.Empty,
                cancellationToken);

            return result.Match(
                authResult => Ok(new AuthenticationResponse(
                    authResult.UserId,
                    authResult.Username,
                    authResult.Email,
                    authResult.RealName,
                    authResult.RealSurname,
                    authResult.AccessToken,
                    authResult.RefreshToken,
                    authResult.AccessTokenExpiry,
                    authResult.RefreshTokenExpiry)),
                errors => Problem(errors));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(
            [FromBody] Contracts.AccountContracts.LoginRequest request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login endpoint called for email: {Email}", request.Email);

            var result = await _authenticationService.LoginAsync(
                request.Email,
                request.Password,
                cancellationToken);

            return result.Match(
                authResult => Ok(new AuthenticationResponse(
                    authResult.UserId,
                    authResult.Username,
                    authResult.Email,
                    authResult.RealName,
                    authResult.RealSurname,
                    authResult.AccessToken,
                    authResult.RefreshToken,
                    authResult.AccessTokenExpiry,
                    authResult.RefreshTokenExpiry)),
                errors => Problem(errors));
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(
            [FromBody] RefreshTokenRequest request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Refresh token endpoint called");

            var result = await _authenticationService.RefreshTokenAsync(
                request.RefreshToken,
                cancellationToken);

            return result.Match(
                authResult => Ok(new AuthenticationResponse(
                    authResult.UserId,
                    authResult.Username,
                    authResult.Email,
                    authResult.RealName,
                    authResult.RealSurname,
                    authResult.AccessToken,
                    authResult.RefreshToken,
                    authResult.AccessTokenExpiry,
                    authResult.RefreshTokenExpiry)),
                errors => Problem(errors));
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                _logger.LogWarning("Logout called without valid userId claim");
                return Unauthorized();
            }

            _logger.LogInformation("Logout endpoint called for user: {UserId}", userId);

            var result = await _authenticationService.RevokeTokenAsync(userId, cancellationToken);

            return result.Match(
                _ => NoContent(),
                errors => Problem(errors));
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            var emailClaim = User.FindFirst("email")?.Value;
            var usernameClaim = User.FindFirst("username")?.Value;
            var realNameClaim = User.FindFirst("given_name")?.Value;
            var realSurnameClaim = User.FindFirst("family_name")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            return Ok(new CurrentUserResponse(
                userId,
                usernameClaim ?? "",
                emailClaim ?? "",
                realNameClaim ?? "",
                realSurnameClaim ?? ""));
        }
    }
}
