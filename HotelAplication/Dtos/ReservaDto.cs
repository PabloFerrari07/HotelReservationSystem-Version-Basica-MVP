namespace HotelAplication.Dtos
{
    public class ReservaDto
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public int NumeroHabitacion { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public string Estado { get; set; }
    }

}
