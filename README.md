# Documentación técnica de ECommerce y PaymentService

## Descripción general

Este trabajo consiste en la implementación de una solución backend distribuida compuesta por dos proyectos relacionados:

- ECommerce: API principal del negocio, responsable de gestionar usuarios, productos, órdenes y autenticación.
- PaymentService: microservicio independiente encargado de simular el procesamiento de pagos y devolver una respuesta de aprobación o rechazo.

### Objetivo del trabajo

El objetivo es demostrar una arquitectura de backend basada en Clean Architecture, separación de responsabilidades, uso de CQRS con MediatR, autenticación JWT y comunicación HTTP entre servicios. La solución permite crear una orden en ECommerce, invocar al servicio de pagos y actualizar el estado de la orden según la respuesta recibida.

### Arquitectura distribuida

La solución está organizada como un sistema distribuido simple compuesto por dos aplicaciones independientes:

1. ECommerce expone endpoints HTTP para el negocio principal.
2. PaymentService expone un endpoint específico para procesar pagos.
3. ECommerce consume PaymentService mediante HTTP utilizando un cliente abstraído por una interfaz.

Este diseño permite que el servicio de pagos pueda evolucionar de forma independiente del negocio principal y que ECommerce mantenga una integración desacoplada con el exterior.

### Clean Architecture

La solución sigue el patrón Clean Architecture separando responsabilidades en capas:

- API: recibe las solicitudes HTTP.
- Application: contiene los casos de uso, handlers, comandos, queries y DTOs.
- Domain: contiene las entidades, enums y reglas de negocio del dominio.
- Infrastructure: implementa detalles técnicos como EF Core, repositorios, JWT, middleware y el cliente HTTP.

Esto reduce el acoplamiento entre capas y facilita la evolución del sistema.

### CQRS

El proyecto aplica CQRS separando operaciones de escritura y lectura en comandos y consultas:

- Commands: encargados de modificar estado, por ejemplo crear usuarios, productos u órdenes.
- Queries: encargados de consultar información, por ejemplo obtener productos u órdenes.

### MediatR

MediatR se utiliza como mecanismo de despacho de comandos y queries entre la capa API y la capa Application. La API no ejecuta la lógica directamente; delega la operación a un handler registrado en el contenedor de dependencias.

### JWT

La autenticación del sistema se implementa con JWT. El token se genera en ECommerce usando un servicio dedicado y se valida en la API mediante JwtBearer. El token transporta claims de usuario, email y rol, que se usan para autorizar operaciones protegidas.

### Comunicación HTTP

ECommerce se comunica con PaymentService mediante HTTP usando HttpClient. El cliente está registrado vía IHttpClientFactory y se configura con base address, timeout y reintentos. La integración está encapsulada detrás de una interfaz de aplicación, lo que mantiene la lógica de negocio desacoplada de la implementación concreta del cliente HTTP.

### Responsabilidad de cada proyecto

- ECommerce: gestionar la experiencia de negocio del e-commerce: autenticación, gestión de productos, creación de órdenes y coordinación del pago.
- PaymentService: procesar un pago simulando una regla de negocio simple y devolver un resultado de aprobación o rechazo.

---

## ECommerce

### Estructura y capas

#### Capa API

Ubicada en la carpeta ECommerce/src/ECommerce.Api.

Responsabilidades:
- Exponer los endpoints HTTP a través de controladores.
- Configurar Swagger y autenticación JWT.
- Delegar la lógica en handlers mediante MediatR.
- Manejar errores de forma centralizada con middleware global de excepciones.

Controladores incluidos:
- AuthController: registro e inicio de sesión.
- ProductController: consulta, creación y eliminación de productos.
- OrderController: consulta y creación de órdenes.

#### Capa Application

Ubicada en ECommerce/src/ECommerce.Application.

Responsabilidades:
- Contener los casos de uso del sistema.
- Definir comandos y queries.
- Implementar handlers con MediatR.
- Definir DTOs para operaciones de entrada y salida.
- Encapsular la lógica de aplicación.

Ejemplos incluidos:
- RegisterUserCommandHandler
- LoginCommandHandler
- CreateProductCommandHandler
- CreateOrderCommandHandler

#### Capa Domain

Ubicada en ECommerce/src/ECommerce.Domain.

Responsabilidades:
- Contener las entidades del negocio: Product, Order, OrderItem, User.
- Definir enums como OrderStatus.
- Representar reglas de negocio internas, como validación de stock y marcado de estados de orden.

Ejemplos:
- Product.ReduceStock(int quantity)
- Order.MarkAsPaid()
- Order.MarkPaymentRejected()
- Order.MarkPaymentProcessingFailed()

#### Capa Infrastructure

Ubicada en ECommerce/src/ECommerce.Infrastructure.

Responsabilidades:
- Implementar persistencia con Entity Framework Core y SQLite.
- Definir repositorios para Product, User y Order.
- Implementar el servicio JWT.
- Implementar el cliente HTTP de integración con PaymentService.
- Registrar el middleware global de excepciones.

### Flujo de creación de una orden

El flujo real de creación de una orden en ECommerce es el siguiente:

1. El cliente autentica y llama al endpoint POST /api/Order.
2. OrderController obtiene el userId desde el claim NameIdentifier del token JWT.
3. El controller envía un CreateOrderCommand mediante MediatR.
4. CreateOrderCommandHandler valida que la solicitud tenga al menos un item.
5. Para cada producto del pedido:
   - verifica que la cantidad sea mayor que cero,
   - busca el producto en el repositorio,
   - valida su existencia,
   - reduce el stock mediante la entidad Product,
   - agrega el item a la orden.
6. Una vez construida la orden, el handler invoca al PaymentClient para contactar a PaymentService.
7. Según la respuesta del servicio:
   - si el pago es Approved, la orden se marca como Paid,
   - si el pago es Rejected, la orden se marca como PaymentRejected,
   - si hay error de red o timeout, la orden se marca como PaymentProcessingFailed.
8. La orden se persiste mediante IOrderRepository.AddAsync.
9. El handler devuelve un OrderResponse con el estado de la orden y un mensaje descriptivo.

Este flujo demuestra cómo la lógica de negocio, la integración externa y la persistencia quedan separadas en distintas capas.

---

## PaymentService

### Estructura y capas

#### Capa API

Ubicada en PaymentService/src/PaymentService.Api.

Responsabilidades:
- Exponer el endpoint HTTP de procesamiento de pagos.
- Configurar Swagger y autenticación JWT.
- Delegar la operación al handler correspondiente a través de MediatR.

Controlador incluido:
- PaymentsController

#### Capa Application

Ubicada en PaymentService/src/PaymentService.Application.

Responsabilidades:
- Definir el comando ProcessPaymentCommand.
- Implementar ProcessPaymentCommandHandler.
- Definir los DTOs de request y response del pago.
- Encapsular la lógica de aplicación para el procesamiento del pago.

#### Capa Domain

Ubicada en PaymentService/src/PaymentService.Domain.

Responsabilidades:
- Definir la entidad Payment.
- Definir el enum PaymentStatus.
- Representar el resultado del pago como un concepto del dominio.

#### Capa Infrastructure

Ubicada en PaymentService/src/PaymentService.Infrastructure.

Responsabilidades:
- Actualmente es muy ligera.
- No implementa persistencia ni integración con un gateway real de pagos.

### Regla de negocio

La regla de negocio implementada en PaymentService es simple y explícita:

- si Amount <= 50000, el pago se considera Approved;
- si Amount > 50000, el pago se considera Rejected.

La lógica se encuentra en ProcessPaymentCommandHandler y se representa mediante la constante ApprovalLimit = 50000m.

### Endpoint

El endpoint expuesto por PaymentService es:

- POST /api/payments/process

Recibe un objeto con el siguiente formato:

```json
{
  "amount": 50000
}
```

Y devuelve una respuesta como:

```json
{
  "status": "Approved",
  "transactionId": "..."
}
```

---

## Comunicación entre ECommerce y PaymentService

El flujo de comunicación es el siguiente:

Cliente
↓
API ECommerce
↓
CreateOrderCommandHandler
↓
PaymentClient
↓
PaymentService
↓
Respuesta
↓
Actualización del estado de la orden

### Descripción paso a paso

1. El cliente invoca un endpoint en ECommerce.
2. El controlador recibe la request y la delega al handler correspondiente.
3. El handler crea la orden y valida la disponibilidad de stock.
4. Luego invoca al PaymentClient, que realiza una llamada HTTP a PaymentService.
5. PaymentService procesa el pago según la regla de negocio.
6. ECommerce interpreta la respuesta del servicio.
7. La orden se actualiza con un estado final: Paid, PaymentRejected o PaymentProcessingFailed.
8. La orden queda persistida en la base de datos de ECommerce.

---

## Configuración

### Puertos

Los puertos configurados en los launch settings son:

- ECommerce:
  - HTTP: http://localhost:5136
  - HTTPS: https://localhost:7279;http://localhost:5136
- PaymentService:
  - HTTP: http://localhost:5119
  - HTTPS: https://localhost:7000;http://localhost:5119

### Appsettings

#### ECommerce

Archivo: ECommerce/src/ECommerce.Api/appsettings.json

Configuraciones relevantes:
- ConnectionStrings:DefaultConnection -> Data Source=ecommerce.db
- Services:Payment -> http://localhost:5119
- Services:PaymentTimeoutSeconds -> 10
- Jwt:Key, Jwt:Issuer, Jwt:Audience, Jwt:ExpirationHours

Estas opciones definen la base SQLite, la URL del servicio de pagos y la configuración del JWT.

Usuario de prueba:
"email": "admin@test.com",
"password": "Admin123!"

#### PaymentService

Archivo: PaymentService/src/PaymentService.Api/appsettings.json

Actualmente contiene solo configuración básica de logging y AllowedHosts. No hay configuración extra de negocio ni de base de datos en el código actual.

### HttpClient

En ECommerce, el cliente HTTP para PaymentService se registra en InfrastructureServiceExtensions con AddHttpClient<IPaymentClient, PaymentClient>(...).

El registro incluye:
- BaseAddress tomado de Services:Payment.
- Timeout configurado desde Services:PaymentTimeoutSeconds.
- Policy de reintentos con Polly para manejar excepciones de red y respuestas 5xx.

Esto permite centralizar la configuración del cliente externo y evitar la creación directa de HttpClient en la lógica de aplicación.

---

## Cómo ejecutar

### 1. Levantar PaymentService

Desde la raíz del repositorio:

```powershell
dotnet run --project PaymentService/src/PaymentService.Api --launch-profile http
```

El servicio queda disponible en http://localhost:5119.

### 2. Levantar ECommerce

Desde la raíz del repositorio:

```powershell
dotnet run --project ECommerce/src/ECommerce.Api --launch-profile http
```

La API queda disponible en http://localhost:5136.

### 3. Abrir Swagger

- ECommerce: http://localhost:5136/swagger
- PaymentService: http://localhost:5119/swagger

### 4. Crear una orden

Para crear una orden correctamente, se requiere:

1. Registrar un usuario o usar los usuarios sembrados por la base de datos.
2. Iniciar sesión en /api/Auth/login para obtener un token JWT.
3. Autorizar las peticiones en Swagger con el token Bearer.
4. Crear o verificar que exista un producto en /api/Product.
5. Enviar una petición POST a /api/Order con un cuerpo similar a:

```json
{
  "items": [
    {
      "productId": "<guid-del-producto>",
      "quantity": 1
    }
  ]
}
```

### 5. Verificar Approved

Para obtener una respuesta Approved, el total de la orden debe ser menor o igual a 50000, porque PaymentService evalúa el monto que recibe desde ECommerce.

### 6. Verificar Rejected

Para obtener una respuesta Rejected, el total de la orden debe ser mayor a 50000.

### 7. Apagar PaymentService

Detener el proceso de PaymentService simula una falla de comunicación. En ese caso, el handler de ECommerce captura la excepción y marca la orden como PaymentProcessingFailed.

### 8. Verificar manejo del error

Cuando PaymentService está caído o no responde, el flujo de ECommerce no rompe el proceso: el estado de la orden se actualiza a PaymentProcessingFailed y se devuelve un mensaje claro en la respuesta del endpoint de creación de orden.

---

## Decisiones de diseño

### Clean Architecture

Se eligió una arquitectura por capas para separar claramente la API, la aplicación, el dominio y la infraestructura. Esto mejora la mantenibilidad, reduce el acoplamiento y permite evolucionar cada parte del sistema sin afectar el conjunto completo.

### CQRS

La separación entre comandos y queries permite que las operaciones de escritura y lectura tengan responsabilidades claras y organizadas. Esto facilita la comprensión del flujo y evita mezclar lógica de negocio con consultas simples.

### DTO

Se utilizan DTOs para modelar las entradas y salidas de los endpoints. Esto evita que los contratos HTTP dependan directamente de las entidades del dominio y permite controlar la información expuesta al cliente.

### IHttpClientFactory

El cliente HTTP para PaymentService se registra mediante IHttpClientFactory en vez de crear HttpClient manualmente. Esto centraliza la configuración, facilita la reutilización del cliente, controla el timeout y permite añadir políticas de resiliencia.

### No exponer entidades

El diseño prioriza el uso de DTOs en los contratos de integración del sistema. De esta forma, los modelos de entrada y salida del negocio no dependen directamente de la estructura interna de las entidades del dominio, lo que reduce el acoplamiento de la API con el modelo de persistencia.

### Comunicación HTTP

La comunicación entre ECommerce y PaymentService se realiza sobre HTTP, lo que convierte la integración en un punto claro de interacción entre dos servicios independientes. Esto permite que el pago sea un componente externo al negocio principal, alineado con una arquitectura distribuida.

### Separación de responsabilidades

Cada pieza del sistema tiene una responsabilidad concreta:
- los controladores reciben las peticiones,
- los handlers aplican la lógica de caso de uso,
- los repositorios acceden a datos,
- los clientes HTTP integran servicios externos,
- las entidades del dominio contienen los comportamientos propios del negocio.
