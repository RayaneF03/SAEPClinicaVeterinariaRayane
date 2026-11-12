using System.ComponentModel.DataAnnotations;

namespace ClinicaVet.ViewModels;

public class LoginViewModel
{
    // Dados enviados pelo formulário de autenticação.
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Senha { get; set; } = string.Empty;
}
