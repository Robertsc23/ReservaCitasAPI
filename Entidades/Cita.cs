using System.ComponentModel.DataAnnotations;

namespace ReservaCitasAPI.Entidades
{
    public class Cita
    {
        public int Id { get; set; }

        [Required]
        public string PacienteId { get; set; }
        public ApplicationUser Paciente { get; set; }

        [Required]
        public string DoctorId { get; set; }
        public ApplicationUser Doctor { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }

        public string Estado { get; set; } = "Pendiente";

        public string Motivo { get; set; } = string.Empty;

    }
}
