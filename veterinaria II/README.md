# Sistema de Control Clinico Veterinario v2

Proyecto de Programacion 2. Aplicacion de escritorio en C# con formularios en Avalonia UI y base de datos SQLite.

## Requisitos

- .NET SDK (el proyecto usa net10.0)
- Funciona en macOS, Windows y Linux

## Como ejecutarlo

    git clone https://github.com/TU_USUARIO/VeterinariaV2.git
    cd VeterinariaV2
    dotnet run

La base de datos `veterinaria.db` y sus tablas se crean automaticamente al iniciar.
El script de las tablas tambien esta en `Docs/script_tablas.sql`.

## Usuario inicial

- Usuario: `admin`
- Contrasena: `admin123` (cambiarla desde el formulario de Usuarios)

Las contrasenas se guardan con hash (PBKDF2 con sal).

## Roles y permisos

| Modulo | Administrador | Recepcionista | Veterinario |
|---|---|---|---|
| Usuarios | Si | No | No |
| Propietarios | Si | Si | No |
| Mascotas | Si | Si | Si |
| Citas | Si (completo) | Si (completo) | Solo sus citas, lectura |
| Consultas | Si | No | Solo sus consultas |
| Vacunacion | Si | No | Si |

## Estructura del proyecto

- `Modelos/` clases de datos (Usuario, Propietario, Mascota, Cita, Consulta, Vacunacion)
- `Datos/` conexion SQLite, inicializador de tablas y repositorios con los metodos CRUD
- `Vistas/` formularios (login, usuarios, propietarios, mascotas, citas, consultas, vacunacion)
- `Docs/` script SQL de las tablas

## Reglas de negocio

- Una mascota pertenece a un propietario; no se elimina un propietario con mascotas.
- Un veterinario no puede tener dos citas activas a la misma fecha y hora.
- Al registrar una consulta, la cita pasa a Atendida; al eliminarla, vuelve a Pendiente.

## Autor

[Tu nombre] - Ingenieria en Sistemas
