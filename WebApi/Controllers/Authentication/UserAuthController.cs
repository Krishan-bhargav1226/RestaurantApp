using System.Security.Claims;
using Application.Applications.Auth.User;
using Application.Dtos.Auth.User;
using Domain.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers;

[Route("api/UserAuth")]
[ApiController]
public class UserAuthController : ControllerBase
{
    private readonly IUserAuthApplication _userAuthApplication;
    private readonly IEmailService _emailService;

    public UserAuthController(IUserAuthApplication userAuthApplication, IEmailService emailService)
    {
        _userAuthApplication = userAuthApplication;
        _emailService = emailService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto input)
    {
        try
        {
            var currentRole = User.FindFirstValue(ClaimTypes.Role);

            if (!User.Identity?.IsAuthenticated ?? true)
            {
                if (input.Role != UserRole.Staff)
                    return Forbid();
            }
            else if (currentRole != UserRole.SuperAdmin.ToString())
            {
                if (currentRole != UserRole.BranchAdmin.ToString() ||
                    input.Role is not (UserRole.Manager or UserRole.Staff))
                    return Forbid();
            }

            var result = await _userAuthApplication.RegisterAsync(input);
            var otp = await _userAuthApplication.GenerateRegistrationOtpAsync(result.Email);
            await SendRegistrationOtpEmailAsync(result.Email, result.FullName, otp);

            return Ok(new
            {
                message = "Registration successful. OTP has been sent to your email.",
                data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("resend-registration-otp")]
    public async Task<IActionResult> ResendRegistrationOtp(VerifyUserOtpDto input)
    {
        try
        {
            var otp = await _userAuthApplication.GenerateRegistrationOtpAsync(input.Email);
            var result = await _userAuthApplication.GetType()
                .GetMethod("ToString");
            await SendRegistrationOtpEmailAsync(input.Email.Trim(), "User", otp);

            return Ok(new { message = "Registration OTP has been resent successfully." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("verify-registration-otp")]
    public async Task<IActionResult> VerifyRegistrationOtp(VerifyUserOtpDto input)
    {
        try
        {
            var result = await _userAuthApplication.VerifyRegistrationOtpAsync(input);

            return Ok(new
            {
                message = "Email verified successfully. Registration is complete. Please login to receive an access token.",
                data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto input)
    {
        try
        {
            var result = await _userAuthApplication.LoginAsync(input);
            return Ok(new { message = "Login successful", data = result });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshUserTokenDto input)
    {
        try
        {
            var result = await _userAuthApplication.RefreshTokenAsync(input);
            return Ok(new { message = "Token refreshed successfully", data = result });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotUserPasswordDto input)
    {
        try
        {
            var otp = await _userAuthApplication.ForgotPasswordAsync(input.Email);
            var body = $"<html><body style='font-family:Arial,sans-serif;'><h3>Password Reset</h3><p>Your RestaurantApp password reset OTP is:</p><h2>{otp}</h2><p>This OTP will expire in 10 minutes.</p></body></html>";
            await _emailService.SendEmailAsync(input.Email, "RestaurantApp Password Reset OTP", body);
            return Ok(new { message = "Password reset OTP has been sent to your email." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetUserPasswordDto input)
    {
        try
        {
            await _userAuthApplication.ResetPasswordAsync(input);
            return Ok(new { message = "Password has been reset successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpPut("{userId:int}/assign-role")]
    public async Task<IActionResult> AssignRole(int userId, AssignRoleDto input)
    {
        try
        {
            if (!Enum.IsDefined(input.Role))
                return BadRequest(new { error = "Invalid role." });

            var result = await _userAuthApplication.AssignRoleAsync(userId, input);
            return Ok(new { message = "User role updated successfully.", data = result });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    private async Task SendRegistrationOtpEmailAsync(string email, string fullName, string otp)
    {
        var displayName = string.IsNullOrWhiteSpace(fullName) ? "User" : fullName;
        var body = $"<html><body style='font-family:Arial,sans-serif;'><h3>Welcome to RestaurantApp, {displayName}!</h3><p>Your registration OTP is:</p><h2>{otp}</h2><p>This OTP will expire in 10 minutes.</p><p>If you did not create this account, you can ignore this email.</p></body></html>";
        await _emailService.SendEmailAsync(email, "RestaurantApp Registration OTP", body);
    }
}