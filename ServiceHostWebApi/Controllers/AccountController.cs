using _01_FrameWork.Infrastructure;
using _01_FrameWork.Infrastructure.Models;
using AccountManagment.Contracts.UserContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ServiceHostWebApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AccountController : ControllerBase
{
    private readonly IUserApplication _userApplication;
    private readonly ISMSService _smsService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IUserApplication userApplication, ISMSService smsService, IJwtTokenGenerator jwtTokenGenerator,
        IMemoryCache memoryCache, ILogger<AccountController> logger)
    {
        _userApplication = userApplication;
        _smsService = smsService;
        _jwtTokenGenerator = jwtTokenGenerator;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    
    [HttpPost("login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Login([FromBody] LoginRequestCommand request, [FromQuery] string? ReturnUrl)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
            return BadRequest(new { Error = "شماره موبایل را وارد کنید." });

        var mobileNormalized = request.Username.Normalize_PersianNumbers();

        try
        {
            var smsResult = await _smsService.SendOTPAsync(mobileNormalized!);
            if (!smsResult.IsSuccedded)
            {
                _logger.LogWarning("خطا در ارسال پیامک OTP به موبایل {Mobile}: {Message}", mobileNormalized, smsResult.Message);
                return StatusCode(500, new { Error = "ارسال پیامک تایید انجام نشد." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطا در ارسال پیامک OTP");
            return StatusCode(500, new { Error = "خطای داخلی رخ داده است." });
        }

        var verifyUrl = "api/v1/auth/verify";
        if (!string.IsNullOrEmpty(ReturnUrl))
        {
            verifyUrl += $"?ReturnUrl={Uri.EscapeDataString(ReturnUrl)}";
        }

        return Ok(new
        {
            Message = "کد تایید پیامک شد.",
            VerifyUrl = verifyUrl
        });
    }


    [HttpPost("verify")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestCommand request, [FromQuery] string? ReturnUrl)
    {
        if (string.IsNullOrWhiteSpace(request.Mobile) || string.IsNullOrWhiteSpace(request.OtpCode))
            return BadRequest(new { Error = "شماره موبایل و کد تایید را وارد کنید." });

        var mobileNormalized = request.Mobile.Normalize_PersianNumbers();

        if (!_memoryCache.TryGetValue<OTPModel>(mobileNormalized!, out var otpModel) || !otpModel.IsValid(request.OtpCode))
        {
            return BadRequest(new { Error = "کد تایید معتبر نیست یا اشتباه است." });
        }

        try
        {
            var user = await _userApplication.GetUserWithRoleByUsernameAsync(mobileNormalized!);

            if (user == null)
            {
                var opResult = await _userApplication.Create(new CreateUserCommand { Username = mobileNormalized });
                if (!opResult.IsSuccedded)
                {
                    _logger.LogWarning("خطا در ساخت کاربر جدید با موبایل {Mobile}: {Message}", mobileNormalized, opResult.Message);
                    return BadRequest(new { Error = opResult.Message });
                }

                user = await _userApplication.GetUserWithRoleByUsernameAsync(mobileNormalized!);
                if (user == null)
                {
                    _logger.LogError("کاربر بلافاصله پس از ایجاد، پیدا نشد (خطا احتمالی).");
                    return StatusCode(500, new { Error = "خطا در پردازش کاربر." });
                }
            }

            _memoryCache.Remove(mobileNormalized!);

            var generatedToken = await _jwtTokenGenerator.GenerateTokensAsync(user);

            return Ok(new
            {
                Message = "ورود موفقیت‌آمیز بود.",
                Token = generatedToken,
                ReturnUrl = ReturnUrl ?? "/"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطا در تایید کد OTP یا تولید توکن");
            return StatusCode(500, new { Error = "خطای داخلی رخ داده است." });
        }
    }
}
