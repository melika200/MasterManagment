using _01_FrameWork.Infrastructure;
using _01_FrameWork.Infrastructure.Models;
using AccountManagment.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ServiceHostWebApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserApplication _userApplication;
    private readonly ISMSService _smsService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IUserApplication userApplication, ISMSService smsService, IJwtTokenGenerator jwtTokenGenerator,
        IMemoryCache memoryCache, ILogger<AuthController> logger)
    {
        _userApplication = userApplication;
        _smsService = smsService;
        _jwtTokenGenerator = jwtTokenGenerator;
        _memoryCache = memoryCache;
        _logger = logger;
    }

  

 

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Mobile))
            return BadRequest(new { Error = "شماره موبایل را وارد کنید." });

        var mobileNormalized = request.Mobile.Normalize_PersianNumbers();

       
        var smsResult = await _smsService.SendOTPAsync(mobileNormalized);
        if (!smsResult.IsSuccedded)
        {
            _logger.LogWarning("خطا در ارسال پیامک OTP به موبایل {Mobile}: {Message}", mobileNormalized, smsResult.Message);
            return StatusCode(500, new { Error = "ارسال پیامک تایید انجام نشد." });
        }

        return Ok(new
        {
            Message = "کد تایید پیامک شد.",
            VerifyUrl = $"api/v1/auth/auth/verify"
           
        });

    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Mobile) || string.IsNullOrWhiteSpace(request.OtpCode))
            return BadRequest(new { Error = "شماره موبایل و کد تایید را وارد کنید." });

        var mobileNormalized = request.Mobile.Normalize_PersianNumbers();

        if (!_memoryCache.TryGetValue<OTPModel>(mobileNormalized, out var otpModel) || !otpModel.IsValid(request.OtpCode))
        {
            return BadRequest(new { Error = "کد تایید معتبر نیست یا اشتباه است." });
        }

        var user = await _userApplication.GetByUsernameAsync(mobileNormalized);
        if (user == null)
        {
            var createResult = await _userApplication.Create(new CreateUserCommand
            {
                Username = mobileNormalized,
                Mobile = mobileNormalized,
                Fullname = "کاربر جدید",
                Password = null
            });

            if (!createResult.IsSuccedded)
            {
                _logger.LogWarning("خطا در ساخت کاربر جدید با موبایل {Mobile}: {Message}", mobileNormalized, createResult.Message);
                return BadRequest(new { Error = createResult.Message });
            }

            user = await _userApplication.GetByUsernameAsync(mobileNormalized);
            if (user == null)
            {
                _logger.LogError("کاربر بلافاصله پس از ایجاد، پیدا نشد (خطا احتمالی).");
                return StatusCode(500, new { Error = "خطا در پردازش کاربر." });
            }
        }

        _memoryCache.Remove(mobileNormalized);

        var generatedToken = _jwtTokenGenerator.GenerateToken(user);

        return Ok(new
        {
            Message = "ورود موفقیت‌آمیز بود.",
            Token = generatedToken,
            ReturnUrl = request.ReturnUrl ?? "/"
        });
    }
}
