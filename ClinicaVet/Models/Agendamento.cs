using System.ComponentModel.DataAnnotations;

namespace ClinicaVet.Models;

public class Agendamento
{
    // Identificador único do agendamento.
    public int Id { get; set; }

    [Required]
    public DateTime DataHora { get; set; }

    [Required, StringLength(300)]
    public string Servico { get; set; } = string.Empty;

    [Required]
    public int PetId { get; set; }

    public Pet? Pet { get; set; }
}
    // Data e hora em que o serviço ocorrerá.
    // Descrição do serviço veterinário.
    // Chave estrangeira obrigatória para o pet.
