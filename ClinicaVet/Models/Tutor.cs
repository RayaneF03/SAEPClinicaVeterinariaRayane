using System.ComponentModel.DataAnnotations;

namespace ClinicaVet.Models;

public class Tutor
{
    // Identificador único do tutor.
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(160)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(500)]
    public string CpfCriptografado { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Telefone { get; set; }

    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
    // Dados básicos usados na listagem e na busca.
    // CPF protegido; nenhum CPF puro é armazenado nesta entidade.
    // Telefone é opcional.
    // Relação de um tutor para muitos pets.
