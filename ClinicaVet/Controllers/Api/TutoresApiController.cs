using ClinicaVet.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVet.Controllers.Api;

[ApiController, Route("api/tutores"), Authorize]
public class TutoresApiController(ClinicaContext db) : ControllerBase
{
    // Retorna somente campos públicos; o CPF nunca é exposto pela API.
    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await db.Tutores.AsNoTracking().OrderBy(t => t.Nome)
        .Select(t => new { t.Id, t.Nome, t.Email, t.Telefone }).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        // Busca um tutor específico e responde 404 quando ele não existe.
        var tutor = await db.Tutores.AsNoTracking().Where(t => t.Id == id)
            .Select(t => new { t.Id, t.Nome, t.Email, t.Telefone }).FirstOrDefaultAsync();
        return tutor is null ? NotFound() : Ok(tutor);
    }
}
