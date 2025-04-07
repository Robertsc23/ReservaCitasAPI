using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReservaCitasAPI.DTOs;
using ReservaCitasAPI.Entidades;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReservaCitasAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;

        public AuthController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult> Registrar([FromBody] RegistroUsuarioDTO dto)
        {
            var usuario = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                NombreCompleto = dto.NombreCompleto,
                Rol = dto.Rol
            };

            var resultado = await userManager.CreateAsync(usuario, dto.Password);

            if (!resultado.Succeeded)
                return BadRequest(resultado.Errors);

            // Crear rol si no existe
            var roleManager = HttpContext.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();
            if (!await roleManager.RoleExistsAsync(dto.Rol))
            {
                await roleManager.CreateAsync(new IdentityRole(dto.Rol));
            }

            await userManager.AddToRoleAsync(usuario, dto.Rol);

            return Ok("Usuario registrado correctamente");
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginDTO dto)
        {
            var resultado = await signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);

            if (!resultado.Succeeded)
                return Unauthorized("Login inválido");

            var usuario = await userManager.FindByEmailAsync(dto.Email);
            var roles = await userManager.GetRolesAsync(usuario);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim("nombre", usuario.NombreCompleto),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id)
            };

            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(1);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiracion = expiration
            });
        }
    }
}
