# ECommerce Clean Architecture API

Backend de e-commerce desarrollado con ASP.NET Core 8 aplicando Clean Architecture, Entity Framework Core, JWT Authentication y principios de arquitectura escalable.

---

# Tecnologías utilizadas

- ASP.NET Core 8
- C#
- Entity Framework Core 8
- SQLite
- JWT Authentication
- Swagger / OpenAPI
- Git
- GitHub

---

# Arquitectura utilizada

El proyecto sigue el patrón **Clean Architecture** separando responsabilidades en distintas capas.

## Estructura del proyecto


ECommerce-main
│
├── ECommerce.Api
│   ├── Controllers
│   ├── Models
│   ├── Program.cs
│   └── appsettings.json
│
├── ECommerce.Application
│   ├── Interfaces
│   └── UseCases
│      └── Products
│        ├── Commands
│        └── Queries
│
├── ECommerce.Domain
│   └── Entities
│
└── ECommerce.Infrastructure
    ├── Repositories
    ├── Services
    ├── Migrations
    ├── Middleware
    └── Persistence

    
CAPAS:

Api

Contiene:

Controllers
endpoints HTTP
configuración Swagger
configuración JWT
configuración general de la aplicación

Application

Contiene:

interfaces
contratos
casos de uso
commands
queries
lógica de aplicación

Domain

Contiene:

entidades
reglas de negocio
lógica del dominio

Infrastructure

Contiene:

Entity Framework Core
DbContext
Repositories
Servicios JWT
Migraciones
acceso a datos
middleware global

Métodos y patrones utilizados:
Clean Architecture

Repository Pattern:

Se implementó el patrón Repository para abstraer el acceso a datos.

Ejemplo:

IProductRepository
ProductRepository
IUserRepository
UserRepository

Dependency Injection:

Uso de inyección de dependencias nativa de ASP.NET Core mediante:

builder.Services.AddInfrastructure(builder.Configuration);

Commands y Queries:

Se implementaron casos de uso separados para lectura y escritura.

Entity Framework Core:

Se utilizó EF Core para:

mapeo objeto-relacional
persistencia
consultas
migraciones

Fluent API:

Configuración avanzada de entidades mediante:

IEntityTypeConfiguration<T>

Permite:

configurar relaciones
índices
restricciones
tipos SQL

JWT Authentication

El proyecto implementa autenticación basada en JWT (JSON Web Token).

Funcionalidades:

registro de usuarios
login de usuarios
generación de token JWT
protección de endpoints
autorización por roles

Roles implementados:
Admin
User

Seguridad y autorización:

Se utilizaron políticas de autorización mediante roles.

Ejemplo:

[Authorize]
[Authorize(Roles = "Admin")]

Permite:

proteger endpoints
restringir acceso según rol
validar usuarios autenticados

Middleware global de excepciones:

Se implementó manejo global de excepciones utilizando:

IExceptionHandler
ProblemDetails

Permite:

centralizar errores
respuestas HTTP consistentes
logging de excepciones

Funcionalidades implementadas:
Auth
Registro de usuarios
Login
Generación de JWT

Endpoints:

POST /api/Auth/register
POST /api/Auth/login

luego del login se genera un token JWT válido para acceder a endpoints protegidos.

Productos:
Obtener todos los productos
Obtener producto por ID
Crear producto
Eliminar producto

Endpoints:

GET /api/Product
GET /api/Product/{id}
POST /api/Product
DELETE /api/Product/{id}

Protección de endpoints:

Algunos endpoints requieren autenticación JWT.

Uso en Swagger
Ejecutar login
Copiar el token generado
Presionar botón Authorize
Ingresar:
Bearer TU_TOKEN

Base de datos

Se utilizó SQLite como motor de base de datos.

Archivo generado:

ecommerce.db

Migraciones

Se utilizaron migraciones de EF Core para generar automáticamente la base de datos.

Comandos utilizados:

dotnet ef migrations add InitialCreate
dotnet ef database update

Swagger / OpenAPI:

Documentación automática de endpoints REST mediante Swagger UI.

Disponible en:

https://localhost:xxxx/swagger

Cómo ejecutar el proyecto
1. Clonar repositorio
git clone https://github.com/TU_USUARIO/ECommerce.git

2. Entrar al proyecto
cd ECommerce

3. Restaurar paquetes
dotnet restore

4. Aplicar migraciones
dotnet ef database update --project ECommerce.Infrastructure --startup-project Ecommerce.Api

5. Ejecutar la API
dotnet run --project Ecommerce.Api

6. Abrir Swagger
https://localhost:xxxx/swagger


Cómo probar autenticación JWT:

Registrar usuario:

Endpoint:

POST /api/Auth/register

Ejemplo:

{
  "email": "admin@test.com",
  "name": "Admin",
  "password": "Admin123!",
  "role": "Admin"
}

Login:

Endpoint:

POST /api/Auth/login

Ejemplo:

{
  "email": "admin@test.com",
  "password": "Admin123!"
}
Usar token JWT
Copiar token
Presionar Authorize
Ingresar:
Bearer TU_TOKEN

una vez autenticado un usuario "User" puede consultar productos y un usuario "Admin" puede crear y eliminar productos.

Objetivo del proyecto

Proyecto realizado con fines de aprendizaje y práctica profesional de desarrollo backend utilizando tecnologías modernas del ecosistema .NET.