using ClinicaVet.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVet.Data;

public class ClinicaContext(DbContextOptions<ClinicaContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Tutor> Tutores => Set<Tutor>();
    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<Agendamento> Agendamentos => Set<Agendamento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<Tutor>().HasIndex(x => x.Email);
        modelBuilder.Entity<Pet>().HasOne(x => x.Tutor).WithMany(x => x.Pets)
            .HasForeignKey(x => x.TutorId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Agendamento>().HasOne(x => x.Pet).WithMany(x => x.Agendamentos)
            .HasForeignKey(x => x.PetId).OnDelete(DeleteBehavior.Restrict);
    }
}
