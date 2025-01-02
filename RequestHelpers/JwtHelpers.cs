using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using BackendService.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace BackendService.RequestHelpers;

public class JwtHelpers
{

    public static ClaimsPrincipal ValidateJwt(string jwt){
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY"))),
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
        try
        {
            // Validate the token and return the claims principal
            var principal = tokenHandler.ValidateToken(jwt, validationParameters, out _);
            return principal;
        }
        catch (SecurityTokenException ex)
        {
            // Log the exception or handle it
            Console.WriteLine($"Token validation failed: {ex.Message}");
            return null;
        }
    }
    public static string GenerateJwtToken(TokenGenerationRequest_DTO request, IConfiguration config, TimeSpan tokenLifetime)
    {
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY"));
        
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Sub, request.Email),
            new (JwtRegisteredClaimNames.Email, request.Email),
            new("userid", request.UserId.ToString())
        };
        
        foreach (var claimPair in request.CustomClaims)
        {
            var jsonElement = (JsonElement)claimPair.Value;
            var valueType = jsonElement.ValueKind switch
            {
                JsonValueKind.True => ClaimValueTypes.Boolean,
                JsonValueKind.False => ClaimValueTypes.Boolean,
                JsonValueKind.Number => ClaimValueTypes.Double,
                _ => ClaimValueTypes.String
            };
            
            var claim = new Claim(claimPair.Key, claimPair.Value.ToString(), valueType);
            claims.Add(claim);
        }
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(tokenLifetime),
            Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
            IssuedAt = DateTime.UtcNow,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);
        return jwt;
    }
}