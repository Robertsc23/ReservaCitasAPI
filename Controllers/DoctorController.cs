using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservaCitasAPI.DTOs;
using ReservaCitasAPI.Entidades;

namespace ReservaCitasAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/doctores")]
    public class DoctorController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        public DoctorController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<DoctorDTO>>> Get()
        {
            var doctores = await userManager.Users
                .Where(u => u.Rol == "Doctor")
                .Select(d => new DoctorDTO
                {
                    Id = d.Id,
                    NombreCompleto = d.NombreCompleto,
                    Email = d.Email
                })
                .ToListAsync();

            return doctores;
        }
    }
}
