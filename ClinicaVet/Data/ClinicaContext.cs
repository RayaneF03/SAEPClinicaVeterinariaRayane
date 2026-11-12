using ClinicaVet.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVet.Data;

public class ClinicaContext(DbContextOptions<ClinicaContext> options) : DbContext(options)
{
    // Cada DbSet representa uma tabela consultável pelo Entity Framework.
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Tutor> Tutores => Set<Tutor>();
    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<Agendamento> Agendamentos => Set<Agendamento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Email de usuário não pode se repetir; email de tutor recebe índice para busca.
        modelBuilder.Entity<Usuario>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<Tutor>().HasIndex(x => x.Email);
        // Um tutor pode ter vários pets; o pet precisa apontar para um tutor existente.
        modelBuilder.Entity<Pet>().HasOne(x => x.Tutor).WithMany(x => x.Pets)
            .HasForeignKey(x => x.TutorId).OnDelete(DeleteBehavior.Restrict);
        // Um pet pode ter vários agendamentos; exclusões são restritas para preservar histórico.
        modelBuilder.Entity<Agendamento>().HasOne(x => x.Pet).WithMany(x => x.Agendamentos)
            .HasForeignKey(x => x.PetId).OnDelete(DeleteBehavior.Restrict);
    }
}
