namespace ClinicaVet.Services;

public interface ICpfCripto
{
    string Proteger(string cpf);
    string Desproteger(string cifra);
}
