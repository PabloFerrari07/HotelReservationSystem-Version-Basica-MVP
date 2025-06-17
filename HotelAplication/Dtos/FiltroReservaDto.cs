namespace HotelAplication.Dtos
{
    public class FiltroReservaDto
    {
        public string? Estado { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdHabitacion { get; set; }
    }
}
