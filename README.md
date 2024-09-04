# Introduction 
Este proyecto se construye como un API REST destinado a ser un Servidor básico de autenticación y autorización dummy, implementando autenticación por tokens.
El desarrollado es guíado por los principios de arquitectura limpia con la intención de practicar los conceptos clave así como la interacción con las capas
recomendadas y las que he conocido durante mí experiencia.

## Ficha técnica del proyecto
- .NET 6
- C# v10
- Arquitectura limpia
- Result Pattern
- SqlServer
- EntityFrameworkCore
- Microsoft.AspNetCore.Identity
- JWT
- Swagger
- AutoMapper
- Serilog
- Newtonsoft
- CORS

El proyecto requiere ajustar las cadenas de conexión a Base de Datos para ejecutar la migración de acuerdo al ambiente así como los datos de JWT.
El DbContext utiliza la estructura base de Identity más un par de propiedades que se agregan para extender la entidad. 

# Getting Started
TODO: Guide users through getting your code up and running on their own system. In this section you can talk about:
1.	Installation process
2.	Software dependencies
3.	Latest releases
4.	API references

# Build and Test
TODO: Describe and show how to build your code and run the tests. 

# Contribute
TODO: Explain how other users and developers can contribute to make your code better. 

If you want to learn more about creating good readme files then refer the following [guidelines](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops). You can also seek inspiration from the below readme files:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)
