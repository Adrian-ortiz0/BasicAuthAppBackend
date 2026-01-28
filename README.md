**ASP.NET Core (Backend) + Angular (Frontend)**

### Backend
- ASP.NET Core
- JWT Bearer Authentication
- BCrypt (hash de contraseñas)
- Entity Framework Core
- MySQL
- Clean Architecture

### Frontend
- Angular
- Reactive Forms
- HttpClient
- Signals
- LocalStorage

## Explicación de flujo

###  Creación de la entidad Usuario

En el backend se define la entidad User, la cual representa al usuario dentro del sistema.  
Esta entidad contiene la información mínima necesaria para autenticación y autorización:

- Username
- Email
- PasswordHash

La contraseña siempre se va aguardar salteada o hasheada, no se bien como decirle

---

###  Definición de DTOs

Para desacoplar la lógica interna del dominio de las entradas y salidas de la API, se crean Dtos

- **RegisterDto**  
  Utilizado para el registro de usuarios.  
  Contiene:
  - Username
  - Email
  - Password

- **LoginDto**  
  Utilizado para el inicio de sesión.  
  Contiene:
  - Email
  - Password

- **AuthResponseDto**  
  Utilizado como respuesta tanto en login como en registro.  
  Contiene:
  - UserId
  - Username
  - Email
  - Token (JWT)

---

###  Servicio de Autenticación

Se implementa un servicio de autenticación encargado de orquestar el proceso de registro y login, separando la lógica de negocio de los controladores.

Este servicio es responsable de:

- Validar la existencia de usuarios
- Hashear contraseñas
- Verificar credenciales
- Generar tokens JWT

---

###  Registro de Usuario

El proceso de registro sigue los siguientes pasos:

1. El frontend envía los datos al endpoint de registro.
2. El backend valida que el email y el username no estén en uso.
3. Se genera el hash de la contraseña usando BCrypt.
4. Se crea la entidad User y se persiste en la base de datos.
5. Se genera un token JWT para el usuario recién creado.
6. Se retorna un AuthResponseDto al frontend.

Esto permite que el usuario quede **autenticado inmediatamente después del registro**.

---

###  Inicio de Sesión (Login)

El proceso de login funciona de la siguiente manera:

1. El frontend envía el email y la contraseña.
2. El backend busca el usuario por email.
3. Se compara la contraseña ingresada con el hash almacenado usando BCrypt.
4. Si las credenciales son válidas, se genera un nuevo JWT.
5. Se retorna un AuthResponseDto con el token.

---

###  Generación del JWT

Para la generación del token se implementa un **servicio JWT**, el cual:

- Lee la clave secreta desde appsettings.json
- Construye los claims del usuario (Id, Username, Email)
- Define una fecha de expiración

El JWT es completamente **stateless**, lo que evita el uso de sesiones en servidor. O sea no es necesario usar cookies y eso por que estamos en Web APi en MVC seria distinto

---

###  Consumo del Token en el Frontend

Una vez recibido el token:

1. Angular lo almacena en localStorage.
2. El token se envía en cada request protegida mediante el header:

```http
Authorization: Bearer <token>
