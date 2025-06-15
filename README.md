🏨 HotelReservationSystem - Sistema básico de reservas de hotel.
Este es un proyecto full backend desarrollado con C# y .NET, que permite a usuarios registrarse, iniciar sesión, ver habitaciones disponibles y crear reservas. Los administradores pueden gestionar habitaciones y ver todas las reservas.

🚀 Tecnologías utilizadas
ASP.NET Core 8

Entity Framework Core

SQL Server

JWT (Json Web Tokens) para autenticación

Swagger para pruebas de endpoints

✅ Funcionalidades implementadas (Versión básica MVP)
Usuario Cliente	Admin
Registro e inicio de sesión	Registro e inicio de sesión
Ver habitaciones disponibles	CRUD de habitaciones
Crear reservas	Ver todas las reservas
Ver sus propias reservas	-
Cancelar sus reservas	-

🔐 Roles y seguridad
Autenticación con JWT

Control de acceso por roles (cliente/admin)

Validaciones básicas al crear reservas (por ejemplo, solo fechas futuras)

📁 Estructura del proyecto
HotelAplication/
│
├── Controllers/
│   └── AuthController.cs
│   └── HabitacionesController.cs
│   └── ReservasController.cs
│
├── Services/
│   └── AuthService.cs
│   └── HabitacionService.cs
│   └── ReservaService.cs
│
├── Models/
├── Dtos/
├── Mappings/
└── Program.cs

🧪 Swagger para testing de endpoints
Puedes probar el sistema desde Swagger en:

bash
Copiar
Editar
https://localhost:port/swagger
📌 Estado del proyecto
✅ Versión básica completamente funcional
🔄 Próximas mejoras (versión 2.0):

Validación de solapamiento de reservas

Búsqueda de habitaciones disponibles por fecha

Mejor manejo de errores y validaciones en cascada

Panel de administración avanzado

Frontend en React para el cliente

Posibilidad de añadir imágenes y descripción de habitaciones

🧠 ¿Qué aprendí con este proyecto?
Implementar lógica de negocio real con roles y relaciones

Control de flujo seguro con JWT

Validaciones de datos al guardar en base de datos

Buenas prácticas con Entity Framework Core

Modularización y separación de responsabilidades en servicios
