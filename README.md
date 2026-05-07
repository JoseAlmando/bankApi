# BankApi

REST API bancaria construida con **.NET 10**, **Clean Architecture**, **EF Core**, **ASP.NET Core Identity** y **JWT Bearer**.

## Stack

| Capa           | Tecnología                                    |
|----------------|-----------------------------------------------|
| Framework      | .NET 10 (ASP.NET Core)                        |
| ORM            | EF Core 10 + SQL Server                       |
| Auth           | ASP.NET Core Identity + JWT Bearer            |
| Documentación  | Scalar UI (`/scalar/v1`)                      |
| Arquitectura   | Clean Architecture (Domain → Application → Infrastructure → Api) |

## Estructura de proyectos

```
BankApi/
├── BankApi/                   # Api (BankApi.Api.csproj)
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   └── AccountsController.cs
│   ├── Middlewares/
│   │   └── ExceptionHandlingMiddleware.cs
│   └── Program.cs
│
├── BankApi.Domain/            # Domain (BankApi.Domain.csproj)
│   ├── BaseEntity.cs
│   ├── Account.cs
│   ├── Transaction.cs
│   └── ETransactionType.cs
│
├── BankApi.ViewModel/         # Application (BankApi.Application.csproj)
│   ├── Interfaces/
│   ├── DTOs/
│   └── UseCases/
│       ├── Accounts/          # CreateAccount, GetAccountById, GetMyAccounts
│       └── Transactions/      # Deposit, Withdraw, GetTransactionsByAccount
│
├── BankApi.Infraestructure/   # Infrastructure (BankApi.Infrastructure.csproj)
│   ├── Context/BankDbContext.cs
│   ├── Auth/JwtTokenGenerator.cs
│   ├── Repositories/
│   ├── Services/
│   │   ├── AuthService.cs
│   │   └── CurrentUserService.cs
│   ├── Migrations/
│   └── DependencyInjection.cs
│
└── BankApi.Common/            # Shared cross-cutting utilities
```

## Reglas de negocio

- Monto debe ser `> 0` en depósitos y retiros.
- No se permite retiro si `Balance < Monto`.
- Solo el dueño de la cuenta puede operar/consultar su información.
- Todas las fechas son **UTC**.
- Auditoría automática: `CreatedAt` y `UpdatedAt` se gestionan en `SaveChangesAsync`.

## Endpoints

### Auth
| Método | Ruta                   | Auth     | Descripción           |
|--------|------------------------|----------|-----------------------|
| POST   | `/api/auth/register`   | Anónimo  | Registro de usuario   |
| POST   | `/api/auth/login`      | Anónimo  | Login → `{ token, expiresAt }` |

### Cuentas
| Método | Ruta                          | Auth       | Descripción                        |
|--------|-------------------------------|------------|------------------------------------|
| POST   | `/api/accounts`               | Bearer JWT | Crear cuenta                       |
| GET    | `/api/accounts`               | Bearer JWT | Listar cuentas del usuario         |
| GET    | `/api/accounts/{id}`          | Bearer JWT | Obtener cuenta por ID              |
| POST   | `/api/accounts/{id}/deposit`  | Bearer JWT | Depositar monto                    |
| POST   | `/api/accounts/{id}/withdraw` | Bearer JWT | Retirar monto                      |
| GET    | `/api/accounts/{id}/transactions` | Bearer JWT | Historial de transacciones     |

## Configuración

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=<host>,1433;Database=BankDb;User Id=sa;Password=<password>;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "Secret": "<min-32-char-secret>",
    "Issuer": "BankApi",
    "Audience": "BankApi",
    "ExpirationMinutes": 60
  }
}
```

### Levantar con Docker (SQL Server)

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=TestPassword2025!" \
  -p 1433:1433 --name bank_test -d mcr.microsoft.com/mssql/server:2022-latest
```

### Migraciones EF Core

```bash
# Crear migración
dotnet ef migrations add <Name> \
  -p BankApi.Infraestructure/BankApi.Infrastructure.csproj \
  -s BankApi/BankApi.Api.csproj

# Aplicar migración
dotnet ef database update \
  -p BankApi.Infraestructure/BankApi.Infrastructure.csproj \
  -s BankApi/BankApi.Api.csproj
```

### Ejecutar la API

```bash
dotnet run --project BankApi/BankApi.Api.csproj
```

Swagger/Scalar disponible en: `https://localhost:<port>/scalar/v1`

## Principios de arquitectura

1. **Domain** no depende de nada externo.
2. **Application** solo depende de Domain.
3. **Infrastructure** implementa los contratos de Application.
4. **Api** solo referencia Infrastructure en el registro DI (`Program.cs`).
5. `IdentityUser` (`AppUser`) vive únicamente en Infrastructure — Domain usa `OwnerId: string`.
