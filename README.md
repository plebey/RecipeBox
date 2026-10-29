# RecipeBox

RecipeBox is a REST API for managing recipes and ingredients, built with ASP.NET Core, Entity Framework Core and SQL Server.

The project includes authentication, authorization, recipe ownership, validation, logging, integration tests and Docker support.

## Features

- Recipe CRUD
- Ingredient CRUD
- User registration and login
- JWT Bearer authentication
- Password hashing
- `user` and `admin` roles
- Recipe ownership checks
- Pagination and filtering
- JSON Patch support
- Validation and centralized exception handling
- EF Core migrations
- Serilog logging
- OpenAPI + Scalar
- Integration tests
- Docker support

## Tech Stack

- C# / .NET
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Serilog
- Scalar / OpenAPI
- xUnit
- SQLite in-memory
- Docker / Docker Compose

## Architecture

```text
Controllers
    ↓
Services
    ↓
Repositories
    ↓
Entity Framework Core
    ↓
SQL Server
```

Main entities:

- `User`
- `Recipe`
- `Ingredient`
- `RecipeIngredient`

`RecipeIngredient` represents the many-to-many relationship between recipes and ingredients and stores the required ingredient amount.

## API Endpoints

### Authentication

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/users/register` | Register a new user |
| `POST` | `/api/users/login` | Login and receive JWT |

### Recipes

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/recipes` | Get recipes |
| `GET` | `/api/recipes/{id}` | Get recipe by ID |
| `POST` | `/api/recipes` | Create recipe |
| `PUT` | `/api/recipes/{id}` | Update recipe |
| `PATCH` | `/api/recipes/{id}` | Partially update recipe |
| `DELETE` | `/api/recipes/{id}` | Delete recipe |

### Ingredients

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/ingredients` | Get ingredients |
| `GET` | `/api/ingredients/{id}` | Get ingredient by ID |
| `POST` | `/api/ingredients` | Create ingredient |
| `PUT` | `/api/ingredients/{id}` | Update ingredient |
| `DELETE` | `/api/ingredients/{id}` | Delete ingredient |

Protected endpoints require a JWT Bearer token.

Users can modify only their own recipes. Administrators have access to privileged operations.

## Configuration

The application uses ASP.NET Core `IConfiguration`.

Local secrets such as the database connection string and JWT key should be stored using User Secrets or environment variables.

Required configuration:

```text
ConnectionStrings:DefaultConnection
Jwt:Key
Jwt:Issuer
Jwt:Audience
Jwt:ExpiresMinutes
```

## Run Locally

Restore dependencies:

```bash
dotnet restore
```

Apply migrations:

```bash
dotnet ef database update --project RecipeBox
```

Run the API:

```bash
dotnet run --project RecipeBox
```

In the `Development` environment, API documentation is available through Scalar.

## Docker

The project can also be started with Docker Compose:

```bash
docker compose up --build
```

Docker starts both the ASP.NET Core API and SQL Server and automatically applies EF Core migrations on application startup.

## Tests

Integration tests use:

- xUnit
- `Microsoft.AspNetCore.Mvc.Testing`
- SQLite in-memory

Run tests with:

```bash
dotnet test
```