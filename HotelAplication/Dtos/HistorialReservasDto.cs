namespace HotelAplication.Dtos
{
    public class HistorialReservasDto
    {
        public List<ReservaDto> Activas { get; set; }
        public List<ReservaDto> Canceladas { get; set; }
        public List<ReservaDto> Finalizadas { get; set; }
    }
}
