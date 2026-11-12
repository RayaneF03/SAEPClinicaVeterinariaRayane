using System.ComponentModel.DataAnnotations;

namespace ClinicaVet.Models;

public class Tutor
{
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
