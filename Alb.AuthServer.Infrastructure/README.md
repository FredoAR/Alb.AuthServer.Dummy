# Capa de infraestrucutura
La **capa Infraestructura** contiene clases para acceder a **recursos externos**. Estas clases deben **basarse en interfaces definidas 
dentro de la capa de aplicación**.
Implementa clases para acceder a recursos externos, como sistemas de archivos y servicios web, basándose en interfaces definidas en Application.

La uso para:
- Persistencia de datos con EntityFrameworkCore
- Uso de servicios externos


## Integración de Entity Framework Core y SqlServer
Se deben instalar los siguientes paquetes en la capa de infraestructura para manejar la integración con esta base de datos.
- dotnet add package Microsoft.EntityFrameworkCore
- dotnet add package Microsoft.EntityFrameworkCore.SqlServer
- dotnet add package Microsoft.EntityFrameworkCore.Design
- dotnet add package Microsoft.EntityFrameworkCore.Tools

En esta capa voy a crear:
- DbContext


## Integración de Identity
ASP.NET Core Identity en la API para la gestión de autenticación y autorización de usuarios.
- Microsoft.AspNetCore.Identity
    - Se utiliza cuando NO se usa EF. ste paquete proporciona las interfaces y clases básicas para la funcionalidad de Identity, 
    - incluyendo la gestión de usuarios, roles, autenticación, y autorización.

- Microsoft.AspNetCore.Identity.EntityFrameworkCore
    - Este paquete extiende Microsoft.AspNetCore.Identity para integrarse con Entity Framework Core, proporcionando implementaciones 
    - concretas para el almacenamiento de usuarios, roles, y otros datos relacionados con Identity en una base de datos relacional.

Paquetes a utilizar: 
- dotnet add package Microsoft.AspNetCore.Identity;
- dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore;


## Migraciones
Las migraciones se agregan, actualizan, eliminan desde el proyecto de **Infrastructura**.
Para ejecutar los comandos add/update/remove sobre las migraciones tengo que hacerlo con el comando completo como los que se muestran 
en el siguiente parrafo sino me lanza un error
quiza sea por el tema de la arquitectura o alguna configuración

Agregar migración
- dotnet ef migrations add InitAuthServerDbContext --project Alb.AuthServer.Infrastructure.csproj -s ../Alb.AuthServer/Alb.AuthServer.API.csproj -c AuthServerDbContext --verbose

Actualizar DB con la migración
- dotnet ef database update
- dotnet ef database update InitAuthServerDbContext --project Alb.AuthServer.Infrastructure.csproj -s ../Alb.AuthServer/Alb.AuthServer.API.csproj -c AuthServerDbContext --verbose

Remover migración
Elimia la ultima migración agregada
- dotnet ef migrations remove --project Alb.AuthServer.Infrastructure.csproj -s ../Alb.AuthServer/Alb.AuthServer.API.csproj -c AuthServerDbContext --verbose


Nota. Al hacer clic derecho sobre el paquete EntityFramework.Design -> Properties el campo Private Assets
contiene el valor "all" al borrar ese valor el mensaje de error obtenido al ejecutar comando de la 
migración cambio por otro que me permitio avanzar.


