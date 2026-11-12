using System.ComponentModel.DataAnnotations;

namespace ClinicaVet.ViewModels;

public class TutorFormViewModel
{
    // ViewModel separado para impedir que CpfCriptografado vá para a tela.
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(160)]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "CPF")]
    [StringLength(14)]
    public string? CpfNovo { get; set; }

    [StringLength(20)]
    public string? Telefone { get; set; }
}
