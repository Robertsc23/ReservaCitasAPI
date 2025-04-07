using System.ComponentModel.DataAnnotations;

namespace ReservaCitasAPI.DTOs
{
    public class CitaCreacionDTO
    {
        [Required]
        public string DoctorId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime FechaHora { get; set; }

        public string Motivo { get; set; } = string.Empty;
    }
}
