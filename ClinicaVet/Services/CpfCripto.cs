using Microsoft.AspNetCore.DataProtection;

namespace ClinicaVet.Services;

public sealed class CpfCripto(IDataProtectionProvider provider) : ICpfCripto
{
    // O protector usa uma chave gerenciada pelo ASP.NET Data Protection.
    private readonly IDataProtector _protector = provider.CreateProtector("ClinicaVet.Cpf.v1");

    // Protege o CPF antes de persistir.
    public string Proteger(string cpf) => _protector.Protect(cpf);

    // Desprotege o valor somente quando uma regra autorizada precisar dele.
    public string Desproteger(string cifra) => _protector.Unprotect(cifra);
}
