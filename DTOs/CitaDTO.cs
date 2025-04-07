namespace ReservaCitasAPI.DTOs
{
    public class CitaDTO
    {
        public int Id { get; set; }
        public string DoctorNombre { get; set; }
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; }
        public string Motivo { get; set; }

    }
}
