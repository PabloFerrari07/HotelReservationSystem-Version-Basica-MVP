namespace HotelAplication.Dtos
{
    public class RegistroDto
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; } = "Cliente"; // Default
    }
}
