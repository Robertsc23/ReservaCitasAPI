using System.ComponentModel.DataAnnotations;

namespace ReservaCitasAPI.DTOs
{
    public class RegistroUsuarioDTO
    {
        [Required]
        public string NombreCompleto { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Rol { get; set; } // Paciente o Doctor
    }
}
