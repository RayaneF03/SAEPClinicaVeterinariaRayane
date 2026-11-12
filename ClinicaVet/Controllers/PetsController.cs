using ClinicaVet.Data;
using ClinicaVet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVet.Controllers;

[Authorize]
public class PetsController(ClinicaContext context) : Controller
{
    // Lista pets já carregando o tutor para exibição.
    public async Task<IActionResult> Index() => View(await context.Pets.Include(p => p.Tutor).AsNoTracking().OrderBy(p => p.Nome).ToListAsync());

    public async Task<IActionResult> Create()
    {
        // Prepara a lista de tutores para o campo select da view.
        await CarregarTutores();
        return View(new Pet());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();
        var pet = await context.Pets.AsNoTracking().Include(p => p.Tutor).FirstOrDefaultAsync(p => p.Id == id);
        return pet is null ? NotFound() : View(pet);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Pet pet)
    {
        // A validação exige um TutorId antes de salvar o pet.
        if (!ModelState.IsValid) { await CarregarTutores(pet.TutorId); return View(pet); }
        context.Pets.Add(pet); await context.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();
        var pet = await context.Pets.FindAsync(id);
        if (pet is null) return NotFound();
        await CarregarTutores(pet.TutorId); return View(pet);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Pet pet)
    {
        if (id != pet.Id) return NotFound();
        if (!ModelState.IsValid) { await CarregarTutores(pet.TutorId); return View(pet); }
        context.Update(pet); await context.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();
        var pet = await context.Pets.AsNoTracking().Include(p => p.Tutor).FirstOrDefaultAsync(p => p.Id == id);
        return pet is null ? NotFound() : View(pet);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // Preserva o histórico e impede excluir pets com agendamentos.
        var pet = await context.Pets.FindAsync(id);
        if (pet is null) return NotFound();
        if (await context.Agendamentos.AnyAsync(a => a.PetId == id))
        {
            TempData["Erro"] = "Não é possível excluir um pet com agendamentos.";
            return RedirectToAction(nameof(Index));
        }
        context.Pets.Remove(pet);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task CarregarTutores(int? selecionado = null) =>
        // Monta o select com o tutor atualmente selecionado.
        ViewData["TutorId"] = new SelectList(await context.Tutores.OrderBy(t => t.Nome).ToListAsync(), "Id", "Nome", selecionado);
}
