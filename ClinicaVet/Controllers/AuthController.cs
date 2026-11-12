using System.Security.Claims;
using ClinicaVet.Data;
using ClinicaVet.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVet.Controllers;

public class AuthController(ClinicaContext db) : Controller
{
    // Exibe o formulário de login sem exigir autenticação.
    [AllowAnonymous]
    public IActionResult Login() => View(new LoginViewModel());

    [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        // Rejeita campos ausentes ou inválidos antes de consultar o banco.
        if (!ModelState.IsValid)
            return View(model);

        // Localiza o usuário e valida a senha contra o hash armazenado.
        var usuario = await db.Usuarios.SingleOrDefaultAsync(u => u.Email == model.Email);
        var valido = usuario is not null &&
            new PasswordHasher<Models.Usuario>().VerifyHashedPassword(usuario, usuario.SenhaHash, model.Senha) != PasswordVerificationResult.Failed;
        if (!valido)
        {
            ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
            return View(model);
        }

        // Claims ficam disponíveis em User.Identity durante toda a sessão.
        var claims = new[] { new Claim(ClaimTypes.Name, usuario!.Nome), new Claim(ClaimTypes.Email, usuario.Email) };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
            new AuthenticationProperties { IsPersistent = false, ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) });
        return RedirectToAction("Index", "Home");
    }

    [Authorize, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Sair()
    {
        // Remove o cookie de autenticação e encerra a sessão atual.
        await HttpContext.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }
}
