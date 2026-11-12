using ClinicaVet.Data;
using ClinicaVet.Models;
using ClinicaVet.Services;
using ClinicaVet.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVet.Controllers;

[Authorize]
public class TutoresController(ClinicaContext context, ICpfCripto cpfCripto) : Controller
{
    // Lista tutores e aplica filtro opcional por nome ou email.
    public async Task<IActionResult> Index(string? termo)
    {
        var query = context.Tutores.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(termo))
            query = query.Where(t => t.Nome.Contains(termo) || t.Email.Contains(termo));
        ViewData["Termo"] = termo;
        return View(await query.OrderBy(t => t.Nome).ToListAsync());
    }

    public IActionResult Create() => View(new TutorFormViewModel());

    // Mostra os dados públicos do tutor e seus pets, sem revelar o CPF.
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();
        var tutor = await context.Tutores.AsNoTracking().Include(t => t.Pets).FirstOrDefaultAsync(t => t.Id == id);
        return tutor is null ? NotFound() : View(tutor);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TutorFormViewModel model)
    {
        // CPF é obrigatório no cadastro e será protegido antes do INSERT.
        if (string.IsNullOrWhiteSpace(model.CpfNovo))
            ModelState.AddModelError(nameof(model.CpfNovo), "CPF é obrigatório.");
        if (!ModelState.IsValid) return View(model);
        context.Tutores.Add(new Tutor { Nome = model.Nome, Email = model.Email, Telefone = model.Telefone, CpfCriptografado = cpfCripto.Proteger(model.CpfNovo!) });
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();
        var tutor = await context.Tutores.FindAsync(id);
        return tutor is null ? NotFound() : View(new TutorFormViewModel { Id = tutor.Id, Nome = tutor.Nome, Email = tutor.Email, Telefone = tutor.Telefone });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TutorFormViewModel model)
    {
        // Atualiza somente campos permitidos pelo ViewModel; CPF continua protegido.
        if (id != model.Id) return NotFound();
        var tutor = await context.Tutores.FindAsync(id);
        if (tutor is null) return NotFound();
        if (!ModelState.IsValid) return View(model);
        tutor.Nome = model.Nome; tutor.Email = model.Email; tutor.Telefone = model.Telefone;
        if (!string.IsNullOrWhiteSpace(model.CpfNovo)) tutor.CpfCriptografado = cpfCripto.Proteger(model.CpfNovo);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();
        var tutor = await context.Tutores.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        return tutor is null ? NotFound() : View(tutor);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // Impede remover tutor que ainda possui pets relacionados.
        var tutor = await context.Tutores.FindAsync(id);
        if (tutor is null) return NotFound();
        if (await context.Pets.AnyAsync(p => p.TutorId == id))
        {
            TempData["Erro"] = "Não é possível excluir um tutor que possui pets.";
            return RedirectToAction(nameof(Index));
        }
        context.Tutores.Remove(tutor);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
