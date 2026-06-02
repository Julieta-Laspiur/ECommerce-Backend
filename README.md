# ECommerce Clean Architecture API

Backend de e-commerce desarrollado con ASP.NET Core 8 aplicando Clean Architecture, CQRS, Entity Framework Core y autenticación JWT.

Tecnologías utilizadas
ASP.NET Core 8
C#
Entity Framework Core 8
SQLite
JWT Authentication
Swagger / OpenAPI
Git
GitHub
Arquitectura utilizada

El proyecto sigue el patrón Clean Architecture separando responsabilidades en distintas capas.

Estructura del proyecto
src
│
├── ECommerce.Api
│   ├── Controllers
│   ├── Program.cs
│   └── appsettings.json
│
├── ECommerce.Application
│   ├── DTOs
│   ├── Interfaces
│   └── UseCases
│       ├── Products
│       │   ├── Commands
│       │   └── Queries
│       └── Auth
│
├── ECommerce.Domain
│   └── Entities
│
└── ECommerce.Infrastructure
    ├── Persistence
    ├── Repositories
    ├── Services
    ├── Middleware
    └── Migrations
Capas
API

Contiene:

Controllers
endpoints HTTP
configuración Swagger
configuración JWT
configuración general de la aplicación

Los controllers únicamente reciben la solicitud HTTP y delegan la lógica a los casos de uso.

Application

Contiene:

interfaces
DTOs
casos de uso
commands
queries
lógica de aplicación

Aquí se implementa la lógica principal del sistema.

Domain

Contiene:

entidades
reglas de negocio
lógica del dominio

La capa Domain no depende de ninguna otra capa.

Infrastructure

Contiene:

Entity Framework Core
DbContext
Repositories
Servicios JWT
Middleware global
Migraciones
acceso a datos
Patrones y conceptos utilizados
Clean Architecture

Separación de responsabilidades para lograr:

mantenibilidad
escalabilidad
bajo acoplamiento
CQRS (Command Query Responsibility Segregation)

Se separaron las operaciones de lectura y escritura mediante:

Commands

Operaciones que modifican datos.

Ejemplo:

CreateProductCommandHandler
Queries

Operaciones de lectura.

Ejemplo:

GetAllProductsQueryHandler
Repository Pattern

Se implementó el patrón Repository para abstraer el acceso a datos.

Ejemplos:

IProductRepository
ProductRepository
IUserRepository
UserRepository
Dependency Injection

Uso de inyección de dependencias nativa de ASP.NET Core.

Ejemplo:

builder.Services.AddInfrastructure(builder.Configuration);
Entity Framework Core

Se utilizó EF Core para:

persistencia
consultas
migraciones
mapeo objeto-relacional
Fluent API

Configuración avanzada de entidades mediante:

IEntityTypeConfiguration<T>

Permite:

configurar relaciones
restricciones
índices
tipos SQL
Autenticación JWT

El proyecto implementa autenticación basada en JWT.

Funcionalidades
registro de usuarios
login
generación de token JWT
autorización por roles
protección de endpoints
Roles implementados
Admin

Puede:

crear productos
eliminar productos
User

Puede:

consultar productos
Seguridad y autorización

Se utilizaron políticas de autorización mediante roles.

Ejemplos:

[Authorize]
[Authorize(Roles = "Admin")]
Middleware global de excepciones

Se implementó manejo global de excepciones utilizando:

IExceptionHandler
ProblemDetails

Permite:

centralizar errores
respuestas HTTP consistentes
manejo global de excepciones
Funcionalidades implementadas
Auth
Endpoints
POST /api/Auth/register
POST /api/Auth/login

Luego del login se genera un token JWT válido para acceder a endpoints protegidos.

Productos
Endpoints
GET /api/Product
GET /api/Product/{id}
POST /api/Product
DELETE /api/Product/{id}
Protección de endpoints

Algunos endpoints requieren autenticación JWT.

Uso en Swagger
Ejecutar login
Copiar el token generado
Presionar botón "Authorize"
Ingresar:
Bearer TU_TOKEN
Base de datos

Se utilizó SQLite como motor de base de datos.

Archivo generado:

ecommerce.db
Migraciones

Se utilizaron migraciones de EF Core para generar automáticamente la base de datos.

Comandos utilizados
dotnet ef migrations add InitialCreate
dotnet ef database update
Swagger / OpenAPI

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
dotnet ef database update --project src/ECommerce.Infrastructure --startup-project src/ECommerce.Api
5. Ejecutar la API
dotnet run --project src/ECommerce.Api
6. Abrir Swagger
https://localhost:xxxx/swagger
Cómo probar autenticación JWT
Registrar usuario
Endpoint
POST /api/Auth/register
Ejemplo
{
  "email": "admin@test.com",
  "name": "Admin",
  "password": "Admin123!",
  "role": "Admin"
}
Login
Endpoint
POST /api/Auth/login
Ejemplo
{
  "email": "admin@test.com",
  "password": "Admin123!"
}
Usar token JWT
Copiar token
Presionar Authorize
Ingresar:
Bearer TU_TOKEN
Objetivo del proyecto

Proyecto realizado con fines de aprendizaje y práctica profesional de desarrollo backend utilizando tecnologías modernas del ecosistema .NET.