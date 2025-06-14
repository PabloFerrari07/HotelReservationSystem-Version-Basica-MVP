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
                .WithMany() // O con .WithMany(u => u.Reservas) si tenés la lista de reservas en Usuario
                .HasForeignKey(r => r.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict); // O DeleteBehavior.Cascade si querés que se elimine en cascada

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Habitacion)
                .WithMany() // O con .WithMany(h => h.Reservas) si tenés la lista en Habitacion
                .HasForeignKey(r => r.IdHabitacion)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
