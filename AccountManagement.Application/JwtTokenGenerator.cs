using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AccountManagement.Infrastructure.EFCore.Context;
using AccountManagment.Contracts.UnitOfWork;
using AccountManagment.Contracts.UserContracts;
using AccountManagment.Domain.RefreshTokenAgg;
using AccountManagment.Domain.UserAgg;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _secretKey;
    private readonly int _expiryMinutes;
    private readonly AccountContext _accountContext;
    private readonly IAccountUnitOfWork _unitOfWork;

    public JwtTokenGenerator(IConfiguration configuration, AccountContext accountContext,IAccountUnitOfWork unitOfWork)
    {
        _issuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer");
        _audience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience");
        _secretKey = configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException("Jwt:SecretKey");
        if (!int.TryParse(configuration["Jwt:ExpiryMinutes"], out _expiryMinutes))
            _expiryMinutes = 60;

        _accountContext = accountContext ?? throw new ArgumentNullException(nameof(accountContext));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }


    public async Task<TokenResultViewModel> GenerateTokensAsync(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (user.Role == null) throw new InvalidOperationException("User role must be loaded.");


        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),              
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),              
        new Claim(ClaimTypes.Name, user.Username),                           
        new Claim(ClaimTypes.Role, user.Role.Name ?? string.Empty)
    };

        Console.WriteLine($"[DEBUG] Generating token for user.Id = {user.Id} username = {user.Username}");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var accessToken = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_expiryMinutes),
            signingCredentials: creds);

        string accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(accessTokenString);
        Console.WriteLine("[DEBUG] Token claims put into JWT:");
        foreach (var c in jwt.Claims)
        {
            Console.WriteLine($"[DEBUG]   {c.Type} = {c.Value}");
        }

        string refreshTokenString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var refreshToken = new RefreshToken(refreshTokenString, DateTime.UtcNow.AddDays(30), user.Id);

        _accountContext.RefreshTokens.Add(refreshToken);
        await _unitOfWork.CommitAsync();

        return new TokenResultViewModel
        {
            AccessToken = accessTokenString,
            RefreshToken = refreshTokenString
        };
    }


}

    //public async Task<TokenResultViewModel> GenerateTokensAsync(User user)
    //{
    //    if (user == null) throw new ArgumentNullException(nameof(user));

    //    var claims = new List<Claim>
    //    {
    //        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
    //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    //        new Claim(ClaimTypes.Name, user.Username),
    //        new Claim(ClaimTypes.Role, user.Role!.Name)
    //    };

    //    Console.WriteLine($"[DEBUG] Generating token for userId = {user.Id}");


    //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
    //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //    var accessToken = new JwtSecurityToken(
    //        issuer: _issuer,
    //        audience: _audience,
    //        claims: claims,
    //        expires: DateTime.UtcNow.AddMinutes(_expiryMinutes),
    //        signingCredentials: creds);

    //    string accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);


    //    string refreshTokenString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));


    //    var refreshToken = new RefreshToken(refreshTokenString, DateTime.UtcNow.AddDays(30), user.Id);


    //    _accountContext.RefreshTokens.Add(refreshToken);
    //    await _unitOfWork.CommitAsync();

    //    return new TokenResultViewModel
    //    {
    //        AccessToken = accessTokenString,
    //        RefreshToken = refreshTokenString
    //    };
    //}