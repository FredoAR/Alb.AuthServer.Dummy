# Capa de Aplicación
La capa de aplicación maneja como la capa que contiene la lógica de negocio. Esta generalmente se expresa en forma de servicios. 
Esta capa por lo tanto se va a comunicar con la de persistencia de datos. 
Es la capa para definición de casos de uso y es el puente entre el dominio y la presentación.

Contiene la lógica de negocio y utiliza CQRS (Command Query Responsibility Segregation). Define interfaces que se implementan en capas externas. Depende del proyecto Domain.

La capa de aplicación contiene reglas de negocio de la aplicación (Application business rules).

Esta capa define interfaces que son implementadas por capas externas. 
Por ejemplo, si la aplicación necesita acceder a un servicio de notificación, se agregaría una nueva interfaz a la aplicación y 
se crearía una implementación dentro de la infraestructura.

Esta capa implementa CQRS y patrón mediador (Command Query Responsibility Segregation), con cada caso de uso comercial representado por un solo comando o consulta.

## Unit Test
Esta capa puede/debe tener su proyecto de prueba para validar serviciso de aplicación