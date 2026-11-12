using System.ComponentModel.DataAnnotations;

namespace ClinicaVet.Models;

public class Pet
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Especie { get; set; }

    public DateOnly? DataNascimento { get; set; }

    [Required]
    public int TutorId { get; set; }

    public Tutor? Tutor { get; set; }
    public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}
