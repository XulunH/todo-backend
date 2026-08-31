# todo-backend

REST API for a to-do list, built with .NET 8 and Entity Framework Core on SQLite. It
implements the contract in [docs/api-spec.md](docs/api-spec.md), which is the source of
truth for request and response shapes.

The iOS client that consumes it is in [todo-frontend](https://github.com/XulunH/todo-frontend).

## Requirements

- .NET 8 SDK
- The EF Core CLI, for creating the database on a fresh clone:

```
dotnet tool install --global dotnet-ef --version 8.0.11
```

## Running

From the repo root:

```
dotnet ef database update --project src/TodoApi
dotnet run --project src/TodoApi
```

The API listens on `http://localhost:5248`, and Swagger UI is at
`http://localhost:5248/swagger` when running in Development.

The database is a SQLite file at `src/TodoApi/todo.db`. It is gitignored, so the migration
step above is needed the first time — after that, `dotnet run` on its own is enough.

## Endpoints

```
GET     /tasks          filterable and sortable, see below
GET     /tasks/{id}
POST    /tasks
PUT     /tasks/{id}
DELETE  /tasks/{id}
```

`sort_by` accepts `+dueDate`, `-dueDate`, `+createdDate` or `-createdDate`, where `+` is
ascending and `-` is descending. Both parameters are optional and can be combined.

## Layout

```
src/TodoApi/
  Controllers/   routing and status codes
  Services/      business logic and querying
  Data/          EF Core DbContext
  Models/        the TaskItem entity
  Dtos/          request and response shapes
  Json/          UTC date converter
  Migrations/
```

Controllers stay thin and hand off to `ITaskService`, which is the only place that touches
the database. That keeps the HTTP concerns and the data access separable, and makes the
service straightforward to test without spinning up the web host.

## Notes on some decisions

A few things that aren't obvious from reading the code:

- **DTOs rather than exposing the entity.** `id` and `createdDate` belong to the server.
  The create DTO omits them entirely. The update DTO does include them, because the spec's
  PUT body has them, but the service ignores those fields and keeps the stored values.

- **`DueDate` is nullable on the DTOs.** `[Required]` does nothing on a non-nullable
  `DateTime`, so an omitted `dueDate` would quietly become `0001-01-01`. Making it
  `DateTime?` gives it a real absent state that validation can reject.

- **Descriptions are trimmed before validation.** `[Required]` happily accepts a string of
  spaces, so the property setter trims first.

- **One error shape.** The spec asks for `{"message": "string"}` on failures, but ASP.NET
  returns `ProblemDetails` for validation errors by default. `InvalidModelStateResponseFactory`
  in `Program.cs` rewrites those, and an exception handler covers unhandled 500s.

- **Dates are always UTC.** SQLite has no notion of `DateTimeKind`, so values read back
  come out as `Unspecified` and serialize without a `Z` — meaning the same task could
  serialize differently depending on whether it had just been written or read from disk. A
  value converter re-applies UTC on read, and a JSON converter writes a fixed
  `yyyy-MM-ddTHH:mm:ss.fffZ` so clients always see one format.

- **PUT with a mismatched id returns 400.** If the body carries an `id` that disagrees with
  the URL, the request is rejected rather than silently preferring one, so a client bug
  surfaces instead of hiding. Omitting the field is still fine, since the URL already
  identifies the task.

## Known gaps

There are no automated tests. The service layer is the natural place to start, since the
filtering and sorting logic is where the real behaviour lives.

HTTPS redirection is disabled in Development so the iOS simulator can talk to the API over
plain HTTP; it stays on everywhere else.
