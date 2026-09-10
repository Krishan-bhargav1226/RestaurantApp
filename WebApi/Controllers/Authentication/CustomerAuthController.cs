using Application.Applications.Auth.Customer;
using Application.Dtos.Auth.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers;

[Route("api/CustomerAuth")]
[ApiController]
public class CustomerAuthController : ControllerBase
{
    private readonly ICustomerAuthApplication _customerAuthApplication;
    private readonly IEmailService _emailService;

    public CustomerAuthController(ICustomerAuthApplication customerAuthApplication, IEmailService emailService)
    {
        _customerAuthApplication = customerAuthApplication;
        _emailService = emailService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCustomerDto input)
    {
        var result = await _customerAuthApplication.RegisterAsync(input);
        var otp = await _customerAuthApplication.GenerateRegistrationOtpAsync(result.Email);
        await SendRegistrationOtpEmailAsync(result.Email, result.FullName, otp);

        return Ok(new
        {
            message = "Registration successful. OTP has been sent to your email.",
            data = result
        });
    }

    [AllowAnonymous]
    [HttpPost("verify-registration-otp")]
    public async Task<IActionResult> VerifyRegistrationOtp(VerifyCustomerOtpDto input)
    {
        var result = await _customerAuthApplication.VerifyRegistrationOtpAsync(input);

        return Ok(new
        {
            message = "Email verified successfully. Registration is complete. Please login to receive an access token.",
            data = result
        });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCustomerDto input)
    {
        var result = await _customerAuthApplication.LoginAsync(input);
        return Ok(new { message = "Login successful", data = result });
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshCustomerTokenDto input)
    {
        return Ok(await _customerAuthApplication.RefreshTokenAsync(input));
    }

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotCustomerPasswordDto input)
    {
        var otp = await _customerAuthApplication.ForgotPasswordAsync(input.Email);
        var body = $"<html><body style='font-family:Arial,sans-serif;'><h3>Password Reset</h3><p>Your RestaurantApp password reset OTP is:</p><h2>{otp}</h2><p>This OTP will expire in 10 minutes.</p></body></html>";
        await _emailService.SendEmailAsync(input.Email, "RestaurantApp Password Reset OTP", body);

        return Ok(new { message = "OTP has been sent to your email." });
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetCustomerPasswordDto input)
    {
        await _customerAuthApplication.ResetPasswordAsync(input);
        return Ok(new { message = "Password has been reset successfully." });
    }

    private async Task SendRegistrationOtpEmailAsync(string email, string fullName, string otp)
    {
        var body = $"<html><body style='font-family:Arial,sans-serif;'><h3>Welcome to RestaurantApp, {fullName}!</h3><p>Your registration OTP is:</p><h2>{otp}</h2><p>This OTP will expire in 10 minutes.</p><p>If you did not create this account, you can ignore this email.</p></body></html>";
        await _emailService.SendEmailAsync(email, "RestaurantApp Registration OTP", body);
    }
}