using ClinicaVet.Models;
using ClinicaVet.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVet.Data;

public static class SeedData
{
    // Insere dados iniciais somente quando usuários e tutores ainda não existem.
    public static void Inicializar(ClinicaContext db, ICpfCripto cpfCripto)
    {
        if (db.Usuarios.Any() || db.Tutores.Any())
            return;

        // Senhas são armazenadas apenas como hashes usando PasswordHasher.
        var usuarios = new[]
        {
            CriarUsuario("Administrador", "admin@clinicavet.local", "Admin123!"),
            CriarUsuario("Recepção", "recepcao@clinicavet.local", "Recepcao123!"),
            CriarUsuario("Veterinário", "vet@clinicavet.local", "Vet123!"),
        };
        // CPFs são protegidos antes de serem enviados ao banco.
        var tutores = new[]
        {
            new Tutor { Nome = "Ana Souza", Email = "ana@example.com", Telefone = "(11) 99999-0001", CpfCriptografado = cpfCripto.Proteger("111.111.111-11") },
            new Tutor { Nome = "Bruno Lima", Email = "bruno@example.com", Telefone = "(11) 99999-0002", CpfCriptografado = cpfCripto.Proteger("222.222.222-22") },
            new Tutor { Nome = "Carla Mendes", Email = "carla@example.com", Telefone = "(11) 99999-0003", CpfCriptografado = cpfCripto.Proteger("333.333.333-33") },
        };
        db.Usuarios.AddRange(usuarios);
        db.Tutores.AddRange(tutores);
        db.SaveChanges();

        // Pets recebem os IDs dos tutores recém-criados.
        var pets = new[]
        {
            new Pet { Nome = "Mel", Especie = "Cachorro", DataNascimento = new DateOnly(2020, 5, 10), TutorId = tutores[0].Id },
            new Pet { Nome = "Nina", Especie = "Gato", DataNascimento = new DateOnly(2021, 8, 2), TutorId = tutores[1].Id },
            new Pet { Nome = "Thor", Especie = "Cachorro", DataNascimento = new DateOnly(2019, 2, 20), TutorId = tutores[2].Id },
        };
        db.Pets.AddRange(pets);
        db.SaveChanges();
        // Agendamentos relacionam cada serviço ao pet correspondente.
        db.Agendamentos.AddRange(
            new Agendamento { DataHora = DateTime.Today.AddDays(1).AddHours(9), Servico = "Consulta clínica", PetId = pets[0].Id },
            new Agendamento { DataHora = DateTime.Today.AddDays(2).AddHours(14), Servico = "Vacinação", PetId = pets[1].Id },
            new Agendamento { DataHora = DateTime.Today.AddDays(3).AddHours(11), Servico = "Retorno", PetId = pets[2].Id });
        db.SaveChanges();
    }

    private static Usuario CriarUsuario(string nome, string email, string senha)
    {
        // Cria o usuário e transforma a senha em um hash irreversível.
        var usuario = new Usuario { Nome = nome, Email = email };
        usuario.SenhaHash = new PasswordHasher<Usuario>().HashPassword(usuario, senha);
        return usuario;
    }
}
