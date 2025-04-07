using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservaCitasAPI.DTOs;
using ReservaCitasAPI.Entidades;
using System.Security.Claims;

namespace ReservaCitasAPI.Controllers
{
    
        [Authorize]
        [ApiController]
        [Route("api/citas")]
        public class CitaController : ControllerBase
        {
            private readonly ApplicationDbContext dbContext;
            private readonly IMapper mapper;

            public CitaController(ApplicationDbContext dbContext, IMapper mapper)
            {
                this.dbContext = dbContext;
                this.mapper = mapper;
            }

            [HttpGet("mis-citas")]
            [Authorize(Roles = "Paciente")]
            public async Task<ActionResult<List<CitaDTO>>> GetMisCitas()
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var citas = await dbContext.Citas
                    .Include(c => c.Doctor)
                    .Where(c => c.PacienteId == userId)
                    .ToListAsync();

                return mapper.Map<List<CitaDTO>>(citas);
            }

            [HttpPost]
            [Authorize(Roles = "Paciente")]
            public async Task<ActionResult<CitaDTO>> Post([FromBody] CitaCreacionDTO dto)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var cita = mapper.Map<Cita>(dto);
                cita.PacienteId = userId;
                cita.Estado = "Pendiente";

                dbContext.Add(cita);
                await dbContext.SaveChangesAsync();

                var citaDTO = mapper.Map<CitaDTO>(cita);
                return CreatedAtAction(nameof(GetMisCitas), new { id = citaDTO.Id }, citaDTO);
            }

            [HttpPut("{id:int}/confirmar")]
            [Authorize(Roles = "Doctor")]
            public async Task<IActionResult> Confirmar(int id)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var cita = await dbContext.Citas.FirstOrDefaultAsync(c => c.Id == id && c.DoctorId == userId);
                if (cita is null) return NotFound();

                cita.Estado = "Confirmada";
                await dbContext.SaveChangesAsync();

                return NoContent();
            }

            [HttpDelete("{id:int}")]
            [Authorize(Roles = "Paciente,Admin")]
            public async Task<IActionResult> Cancelar(int id)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var cita = await dbContext.Citas.FirstOrDefaultAsync(c => c.Id == id && c.PacienteId == userId);
                if (cita is null) return NotFound();

                dbContext.Remove(cita);
                await dbContext.SaveChangesAsync();

                return NoContent();
            }
        }
    
}
