namespace HotelAplication.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }

        public Usuario Usuario { get; set; }

        public int IdHabitacion { get; set; }
        public Habitacion Habitacion { get; set; }  

        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }   

        public string Estado { get; set; }//activa-cancelada-finalizada
    }
}
