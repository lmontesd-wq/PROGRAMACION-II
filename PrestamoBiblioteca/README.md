# Prestamo de Biblioteca

Sistema en C# con SQLite para administrar lectores, libros y prestamos. Sigue la misma estructura del proyecto `GestionAcademica` del repositorio `programacion2-umg`.

## Requisitos

- .NET 6 SDK o posterior.
- Conexion a Internet durante la primera restauracion del paquete NuGet (`Microsoft.Data.Sqlite`).

## Ejecutar

```powershell
cd PrestamoBiblioteca
dotnet restore
dotnet run
```

La base de datos se crea en `%LOCALAPPDATA%\UMG\PrestamoBiblioteca\biblioteca.db`.
Se puede cambiar la ruta con la variable de entorno `PRESTAMO_BIBLIOTECA_DB`. Para reiniciar los datos, cierre el programa y elimine ese archivo.

## Estructura

```text
PrestamoBiblioteca/
├── Aplicacion/MenuBiblioteca.cs             -> Menu y captura de datos
├── BaseDatos/crear_tablas.sql               -> Tablas, restricciones, indices y triggers
├── Dominio/Lector.cs, Libro.cs, Prestamo.cs -> Entidades y validaciones
├── Persistencia/InicializadorBaseDatos.cs   -> Creacion de base y ejecucion del script
├── Persistencia/RepositorioBiblioteca.cs    -> CRUD con consultas parametrizadas
├── Program.cs
└── PrestamoBiblioteca.csproj
```

## Triggers

| Trigger | Que hace |
|---|---|
| `trg_prestamo_validar_insert` | Valida fechas, stock disponible y que el lector no tenga el mismo libro sin devolver |
| `trg_prestamo_descontar_stock` | Resta 1 al stock al registrar un prestamo |
| `trg_prestamo_validar_devolucion` | Valida la fecha de devolucion y evita devolver dos veces |
| `trg_prestamo_reponer_stock` | Suma 1 al stock al registrar la devolucion |
| `trg_prestamo_borrar_reponer` | Repone stock si se borra un prestamo activo (incluye borrado en cascada de un lector) |
| `trg_prestamo_bloquear_cambio_llaves` | Impide cambiar lector o libro de un prestamo ya creado |

Las fechas se guardan como texto `yyyy-MM-dd`. `FechaDevolucion` es `NULL` mientras el libro no se devuelva.
