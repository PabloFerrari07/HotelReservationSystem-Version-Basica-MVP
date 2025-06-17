namespace HotelAplication.Dtos
{
    public class DashboardAdminDto
    {
        public int TotalUsuarios { get; set; }
        public int TotalReservas { get; set; }
        public int ReservasActivas { get; set; }
        public int ReservasCanceladas { get; set; }
        public int ReservasFinalizadas { get; set; }
        public int HabitacionesDisponibles { get; set; }
        public int HabitacionesOcupadas { get; set; }
    }
}
