using AutoMapper;
using ReservaCitasAPI.DTOs;
using ReservaCitasAPI.Entidades;

namespace ReservaCitasAPI.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CitaCreacionDTO, Cita>();
            CreateMap<Cita, CitaDTO>()
                .ForMember(dest => dest.DoctorNombre, opt => opt.MapFrom(src => src.Doctor.NombreCompleto));

        }
    }
}
