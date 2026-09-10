using Application.Applications.Auth;
using Application.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthApplication _authApplication;
        private readonly IEmailService _emailService;

        public AuthController(IAuthApplication authApplication, IEmailService emailService)
        {
            _authApplication = authApplication;
            _emailService = emailService;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser(RegisterDto input)
        {
            try
            {
                var result = await _authApplication.RegisterUserAsync(input);
                var otp = await _authApplication.GenerateRegistrationOtpAsync(result.Email);
                await SendRegistrationOtpEmailAsync(result.Email, result.FullName, otp);
                return Ok(new { message = "Registration successful. OTP has been sent to your email.", data = result });
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("register-customer")]
        public async Task<IActionResult> RegisterCustomer(RegisterDto input)
        {
            try
            {
                var result = await _authApplication.RegisterCustomerAsync(input);
                var otp = await _authApplication.GenerateRegistrationOtpAsync(result.Email);
                await SendRegistrationOtpEmailAsync(result.Email, result.FullName, otp);
                return Ok(new { message = "Registration successful. OTP has been sent to your email.", data = result });
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("verify-registration-otp")]
        public async Task<IActionResult> VerifyRegistrationOtp(VerifyRegistrationOtpDto input)
        {
            try
            {
                var result = await _authApplication.VerifyRegistrationOtpAsync(input);
                return Ok(new { message = "Email verified successfully. Registration is complete.", data = result });
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto input)
        {
            try
            {
                var result = await _authApplication.LoginAsync(input);
                return Ok(new { message = "Login successful", data = result });
            }
            catch (UnauthorizedAccessException ex) { return Unauthorized(new { error = ex.Message }); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto input)
        {
            try { return Ok(await _authApplication.RefreshTokenAsync(input)); }
            catch (UnauthorizedAccessException ex) { return Unauthorized(new { error = ex.Message }); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto input)
        {
            try
            {
                if (!input.PhoneOrEmail.Contains("@"))
                    return BadRequest("Email based password reset is currently configured. Phone OTP can be added with an SMS service later.");
                var otp = await _authApplication.ForgotPasswordAsync(input.PhoneOrEmail);
                var body = $"<html><body style='font-family:Arial,sans-serif;'><h3>Password Reset</h3><p>Your RestaurantApp password reset OTP is:</p><h2>{otp}</h2><p>This OTP will expire in 10 minutes.</p></body></html>";
                await _emailService.SendEmailAsync(input.PhoneOrEmail, "RestaurantApp Password Reset OTP", body);
                return Ok("OTP has been sent to your email.");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto input)
        {
            try { await _authApplication.ResetPasswordAsync(input); return Ok("Password has been reset successfully."); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        private async Task SendRegistrationOtpEmailAsync(string email, string fullName, string otp)
        {
            var body = $"<html><body style='font-family:Arial,sans-serif;'><h3>Welcome to RestaurantApp, {fullName}!</h3><p>Your registration OTP is:</p><h2>{otp}</h2><p>This OTP will expire in 10 minutes.</p><p>If you did not create this account, you can ignore this email.</p></body></html>";
            await _emailService.SendEmailAsync(email, "RestaurantApp Registration OTP", body);
        }
    }
}