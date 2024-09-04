# Capa de dominio
La capa de dominio encapsula **reglas de negocio**. 
El dominio **no se ve afectado por cambios externos**, por que **no tiene referencias hacia otras capas**.
Contiene la lógica y tipos del dominio, como entidades, enums, excepciones e interfaces. No tiene dependencias externas.

Modelos
Las clases que representen Entidades, clases utilizadas durante la persisitencia de datos.

## Unit Test
Esta capa puede tener su propio proyecto de pruebas para validar **servicios de dominio**.