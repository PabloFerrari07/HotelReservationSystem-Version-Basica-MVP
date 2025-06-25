namespace HotelAplication.Models
{
    public class Usuario
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email {  get; set; }
        public string PasswordHash {  get; set; }

        public string Rol { get; set; }
        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    }
}
