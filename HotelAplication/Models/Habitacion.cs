namespace HotelAplication.Models
{
    public class Habitacion
    {
        public int Id { get; set; }
        public int Numero {  get; set; }
        public string Tipo { get; set; }
        public decimal PrecioPorNoche { get; set; } 
        public bool Disponible {  get; set; }

        public ICollection<Reserva> Reservas { get; set; }
    }
}
