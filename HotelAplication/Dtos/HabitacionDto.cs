using HotelAplication.Models;

namespace HotelAplication.Dtos
{
    public class HabitacionDto
    {
        public int? Id { get; set; }
        public int? Numero { get; set; }
        public string? Tipo { get; set; }
        public decimal? PrecioPorNoche { get; set; }
        public bool? Disponible { get; set; }

    }
}
