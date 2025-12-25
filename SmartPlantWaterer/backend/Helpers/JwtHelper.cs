using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartPlantWaterer.Helpers
{
    public static class JwtHelper
    {
        private const string SecretKey = "THIS_IS_A_VERY_SECURE_SECRET_KEY_12345";

        private const string Issuer = "MyApp";

        private const string Audience = "MyAppUsers";

        public static string GenerateToken(string userName)
        {
            Claim[] claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, "admin"),
                new Claim("plant_access", "1,2,3,4"),
                new Claim("can_water", "true"),
                new Claim("can_ota", "true")
            ];

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(SecretKey));

            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtToken = new(issuer: Issuer, audience: Audience, claims: claims, expires: DateTime.UtcNow.AddHours(8), signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return token;
        }
    }
}
