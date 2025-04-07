using Microsoft.IdentityModel.Tokens;
using ReservaCitasAPI.Entidades;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReservaCitasAPI.Utilidades
{
    public static class JwtHelper
    {
        public static string GenerarToken(ApplicationUser user, IList<string> roles, IConfiguration configuration, out DateTime expiracion)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("nombre", user.NombreCompleto),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var credenciales = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);

            expiracion = DateTime.UtcNow.AddHours(1);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiracion,
                signingCredentials: credenciales
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
