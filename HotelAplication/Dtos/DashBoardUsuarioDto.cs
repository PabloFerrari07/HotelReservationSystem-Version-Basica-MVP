namespace HotelAplication.Dtos
{
    public class DashBoardUsuarioDto
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public int TotalReservas { get; set; }
        public int Activas { get; set; }
        public int Canceladas { get; set; }
        public int Finalizadas { get; set; }
        public List<ReservaDto> ProximasReservas { get; set; }
    }
}
