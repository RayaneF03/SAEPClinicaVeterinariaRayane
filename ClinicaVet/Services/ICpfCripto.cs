namespace ClinicaVet.Services;

public interface ICpfCripto
{
    // Contrato usado pelos controllers e pelo seed para proteger dados pessoais.
    string Proteger(string cpf);
    string Desproteger(string cifra);
}
