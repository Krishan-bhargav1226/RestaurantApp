using Application.Applications.Auth.User;
using Application.Dtos.Auth.User;
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
        var result = await _userAuthApplication.RegisterAsync(input);
        var otp = await _userAuthApplication.GenerateRegistrationOtpAsync(result.Email);
        await SendRegistrationOtpEmailAsync(result.Email, result.FullName, otp);

        return Ok(new
        {
            message = "Registration successful. OTP has been sent to your email.",
            data = result
        });
    }

    [AllowAnonymous]
    [HttpPost("verify-registration-otp")]
    public async Task<IActionResult> VerifyRegistrationOtp(VerifyUserOtpDto input)
    {
        var result = await _userAuthApplication.VerifyRegistrationOtpAsync(input);

        return Ok(new
        {
            message = "Email verified successfully. Registration is complete. Please login to receive an access token.",
            data = result
        });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto input)
    {
        var result = await _userAuthApplication.LoginAsync(input);
        return Ok(new { message = "Login successful", data = result });
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshUserTokenDto input)
    {
        return Ok(await _userAuthApplication.RefreshTokenAsync(input));
    }

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotUserPasswordDto input)
    {
        var otp = await _userAuthApplication.ForgotPasswordAsync(input.Email);
        var body = $"<html><body style='font-family:Arial,sans-serif;'><h3>Password Reset</h3><p>Your RestaurantApp password reset OTP is:</p><h2>{otp}</h2><p>This OTP will expire in 10 minutes.</p></body></html>";
        await _emailService.SendEmailAsync(input.Email, "RestaurantApp Password Reset OTP", body);

        return Ok(new { message = "OTP has been sent to your email." });
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetUserPasswordDto input)
    {
        await _userAuthApplication.ResetPasswordAsync(input);
        return Ok(new { message = "Password has been reset successfully." });
    }

    [Authorize(Roles = "SuperAdmin,BranchAdmin")]
    [HttpPut("{userId:int}/assign-role")]
    public async Task<IActionResult> AssignRole(int userId, AssignRoleDto input)
    {
        var result = await _userAuthApplication.AssignRoleAsync(userId, input);
        return Ok(new { message = "User role updated successfully.", data = result });
    }

    private async Task SendRegistrationOtpEmailAsync(string email, string fullName, string otp)
    {
        var body = $"<html><body style='font-family:Arial,sans-serif;'><h3>Welcome to RestaurantApp, {fullName}!</h3><p>Your registration OTP is:</p><h2>{otp}</h2><p>This OTP will expire in 10 minutes.</p><p>If you did not create this account, you can ignore this email.</p></body></html>";
        await _emailService.SendEmailAsync(email, "RestaurantApp Registration OTP", body);
    }
}