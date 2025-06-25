using Microsoft.EntityFrameworkCore;

namespace HotelAplication.Models
{
    public class HotelContext : DbContext
    {
        public HotelContext(DbContextOptions<HotelContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Habitacion> Habitaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Usuario)
                .WithMany(u => u.Reservas) // Asegurate de tener esto en el modelo Usuario
                .HasForeignKey(r => r.IdUsuario)
                .OnDelete(DeleteBehavior.SetNull); // Cambiado de Restrict a SetNull

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Habitacion)
                .WithMany()
                .HasForeignKey(r => r.IdHabitacion)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
