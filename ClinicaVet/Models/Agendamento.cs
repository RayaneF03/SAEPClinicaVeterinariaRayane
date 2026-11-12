using System.ComponentModel.DataAnnotations;

namespace ClinicaVet.Models;

public class Agendamento
{
    public int Id { get; set; }

    [Required]
    public DateTime DataHora { get; set; }

    [Required, StringLength(300)]
    public string Servico { get; set; } = string.Empty;

    [Required]
    public int PetId { get; set; }

    public Pet? Pet { get; set; }
}
