using System.ComponentModel.DataAnnotations;

namespace ClinicaVet.Models;

public class Usuario
{
    // Chave primária da tabela de usuários.
    public int Id { get; set; }

    // Nome mostrado após o login.
    [Required, StringLength(80)]
    public string Nome { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(160)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string SenhaHash { get; set; } = string.Empty;
}
    // Email usado como identificador de login.
    // Hash da senha; a senha original nunca é persistida.
