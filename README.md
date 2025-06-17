🏨 Sistema de Reservas de Hotel – C# .NET API
Este proyecto es una API RESTful desarrollada con C# y .NET que permite gestionar reservas de habitaciones en un hotel. Incluye autenticación con JWT, control de roles (cliente/admin), validaciones con FluentValidation, lógica de negocio estructurada en servicios, y una arquitectura escalable para futuras expansiones.

🚀 Funcionalidades implementadas
✅ Autenticación y usuarios
Registro con validación de campos y rol automático (cliente/admin).

Login con generación de token JWT.

Gestión de usuarios (solo admin): ver, editar, eliminar.

✅ Reservas de habitaciones
Crear reserva si hay disponibilidad.

Cancelar reserva si está activa.

Ver reservas por usuario (historial con estados: activas, canceladas, finalizadas).

Ver próximas reservas personales.

Dashboard del usuario con resumen de actividad.

✅ Habitaciones
Crear, editar y eliminar habitaciones (admin).

Validación para evitar números de habitación duplicados.

Consultar habitaciones disponibles entre fechas.

✅ Panel de administración (Admin)
Ver historial de reservas de todos los usuarios.

Filtrar reservas por usuario, estado, fecha y habitación.

Ver ocupación del hotel por fecha (habitaciones reservadas por día).

⚙️ Tecnologías y herramientas
.NET 8

Entity Framework Core

JWT (JSON Web Tokens)

FluentValidation

SQL Server

AutoMapper

Arquitectura por capas (Controller - Service - DTO - Model - Validator)

│
├── Controllers
│   ├── AuthController.cs
│   ├── ReservaController.cs
│   └── HabitacionController.cs
│
├── Services
│   ├── UsuarioService.cs
│   ├── ReservaService.cs
│   └── HabitacionService.cs
│
├── Dtos
│   ├── RegistroDto.cs
│   ├── LoginDto.cs
│   ├── HabitacionDto.cs
│   ├── ReservaDto.cs
│   └── ...
│
├── Validators
│   ├── RegisterValidator.cs
│   ├── HabitacionValidator.cs
│   └── ReservaValidator.cs
│
├── Models
│   ├── Usuario.cs
│   ├── Reserva.cs
│   └── Habitacion.cs
│
└── Program.cs



🔒 Roles disponibles
admin: acceso total (reservas, usuarios, habitaciones, estadísticas).

cliente: solo puede ver sus reservas, perfil y habitaciones disponibles.
