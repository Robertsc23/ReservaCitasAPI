using Microsoft.AspNetCore.Identity;

namespace ReservaCitasAPI.Entidades
{
    public class ApplicationUser: IdentityUser
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}
