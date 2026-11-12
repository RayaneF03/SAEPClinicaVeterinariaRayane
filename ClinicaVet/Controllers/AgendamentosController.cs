using ClinicaVet.Data;
using ClinicaVet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVet.Controllers;

[Authorize]
public class AgendamentosController(ClinicaContext context) : Controller
{
    public async Task<IActionResult> Index() => View(await context.Agendamentos.Include(a => a.Pet).ThenInclude(p => p!.Tutor).AsNoTracking().OrderBy(a => a.DataHora).ToListAsync());

    public async Task<IActionResult> Create() { await CarregarPets(); return View(new Agendamento { DataHora = DateTime.Now.AddHours(1) }); }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();
        var item = await context.Agendamentos.AsNoTracking().Include(a => a.Pet).ThenInclude(p => p!.Tutor).FirstOrDefaultAsync(a => a.Id == id);
        return item is null ? NotFound() : View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Agendamento item)
    {
        if (!ModelState.IsValid) { await CarregarPets(item.PetId); return View(item); }
        context.Agendamentos.Add(item); await context.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();
        var item = await context.Agendamentos.FindAsync(id);
        if (item is null) return NotFound();
        await CarregarPets(item.PetId); return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Agendamento item)
    {
        if (id != item.Id) return NotFound();
        if (!ModelState.IsValid) { await CarregarPets(item.PetId); return View(item); }
        context.Update(item); await context.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();
        var item = await context.Agendamentos.AsNoTracking().Include(a => a.Pet).ThenInclude(p => p!.Tutor).FirstOrDefaultAsync(a => a.Id == id);
        return item is null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await context.Agendamentos.FindAsync(id);
        if (item is null) return NotFound();
        context.Agendamentos.Remove(item);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task CarregarPets(int? selecionado = null) =>
        ViewData["PetId"] = new SelectList(await context.Pets.Include(p => p.Tutor).OrderBy(p => p.Nome).Select(p => new { p.Id, Nome = p.Nome + " - " + p.Tutor!.Nome }).ToListAsync(), "Id", "Nome", selecionado);
}
