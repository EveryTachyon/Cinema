# TechnoCinema

TechnoCinema is an ASP.NET Core MVC cinema website for discovering films, browsing showtimes, managing cinema content, and selecting seats for a booking. The current user interface is branded as **FrierenCinema**.

The project uses .NET 8, Entity Framework Core, and MySQL. Checked-in database migrations are applied automatically when the application starts.

## Features

### For visitors

- View the cinema homepage and promotional banners.
- Browse movies, search by title, filter by genre, and sort by rating.
- View movie details, trailer information, and matching showtimes.
- Search showtimes by film name and filter them by cinema.
- Browse paginated showtime results.
- Open the seat-selection page for a showtime.

### For cinema staff

- Create and edit movies.
- Create and edit showtimes.
- Review recently edited showtimes.
- Create and edit homepage banners.
- Inspect database status.

### User pages

- Register, log in, and log out.
- Store passwords using ASP.NET Core's `PasswordHasher<User>`.

## Technology

- ASP.NET Core MVC on .NET 8
- Entity Framework Core `8.0.22`
- Pomelo Entity Framework Core MySQL `8.0.2`
- MySqlConnector `2.5.0`
- Bootstrap and jQuery for the shared UI
- xUnit and EF Core InMemory for automated tests

## Requirements

- .NET 8 SDK
- MySQL Server running locally
- Visual Studio 2022 or another .NET-compatible editor

The default local database expects MySQL at `localhost` with database `technocinema`, user `root`, and password `mysql`. These are development defaults only. Replace them locally and never commit real credentials.

## Run the Website

From the repository root:

```bash
dotnet restore
dotnet build TechnoCinema.sln
dotnet run --project TechnoCinema/TechnoCinema.csproj
```

The configured launch profiles use:

- HTTP: `http://localhost:5146`
- HTTPS: `https://localhost:7121`

When the application starts, `Program.cs` runs `Database.Migrate()`. MySQL must be available, and the configured user must have permission to create or update the schema.

## Database Setup

The connection string is in `TechnoCinema/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "MySql": "Server=localhost;Database=technocinema;User=root;Password=mysql;SslMode=None"
  }
}
```

For a local override, use .NET user secrets:

```bash
dotnet user-secrets set "ConnectionStrings:MySql" "Server=localhost;Database=technocinema;User=YOUR_USER;Password=YOUR_PASSWORD;SslMode=None" --project TechnoCinema/TechnoCinema.csproj
```

The EF Core context manages banners, locations, movies, rooms, seats, showtimes, tickets, and users. Migrations are stored in `TechnoCinema/Data/Migrations`.

## Entity Framework Migrations

Visual Studio Package Manager Console:

```powershell
Add-Migration MigrationName -Project TechnoCinema
Update-Database -Project TechnoCinema
Remove-Migration -Project TechnoCinema
```

.NET CLI from the repository root:

```bash
dotnet ef migrations add MigrationName --project TechnoCinema/TechnoCinema.csproj
dotnet ef database update --project TechnoCinema/TechnoCinema.csproj
dotnet ef migrations remove --project TechnoCinema/TechnoCinema.csproj
```

Install the EF CLI tool once if needed:

```bash
dotnet tool install --global dotnet-ef
```

After changing a model, create and review a migration before applying it. Pending migrations are also applied during application startup.

## Website Areas

The application uses conventional MVC routing: `{controller}/{action}/{id?}`.

| Area | Route | Purpose |
| --- | --- | --- |
| Home | `/Home/Index` | Homepage banners |
| Movies | `/MovieList/Index` | Search, genre filters, and rating sorting |
| Movies | `/Movies/Index` | Movie catalogue |
| Movies | `/Movies/Details/{id}` | Movie information and showtimes |
| Showtimes | `/Seansiajad/Index` | Search, cinema filters, and pagination |
| Showtimes | `/Seansiajad/Create` | Add a showtime |
| Showtimes | `/Seansiajad/Edit/{id}` | Edit a showtime |
| Booking | `/Buy/Index/{id}` | Seat-selection page |
| Users | `/User/Register` | Create an account |
| Users | `/User/Login` | Sign in |
| Users | `/User/Logout` | Sign out |
| Diagnostics | `/Db/DbStatus` | Check database connectivity and showtime count |

The shared navigation bar and footer are defined in `TechnoCinema/Views/Shared/_Layout.cshtml`. `Seansiajad` is the existing controller name for showtimes, and `OstaPiletid` is the existing action name for buying tickets.

## Tests

Run all tests with:

```bash
dotnet test TechnoCinema.sln
```

Tests are stored in `TechnoCinema.Tests`. They use xUnit and an in-memory EF Core database, so controller tests do not require MySQL.

## Project Structure

```text
Cinema/
|-- TechnoCinema.sln
|-- TechnoCinema/
|   |-- Controllers/       MVC request handlers
|   |-- Data/              EF Core context and migrations
|   |-- Models/            Domain entities
|   |-- Views/              Razor pages
|   |-- wwwroot/            CSS, JavaScript, images, and libraries
|   |-- Program.cs          Startup, database, and routing
|   `-- appsettings.json    Application settings
|-- TechnoCinema.Tests/    xUnit controller tests
`-- README.md
```

## Development Notes

- Update the top navigation in `TechnoCinema/Views/Shared/_Layout.cshtml`.
- Add global styles in `TechnoCinema/wwwroot/css/site.css`.
- Add client-side behavior in `TechnoCinema/wwwroot/js/site.js`.
- Keep model changes and EF migrations together.
- Use a separate database for local development and automated tests.

## Known Limitations

- The user controller stores login state in session, but session services and middleware still need to be registered in `Program.cs`.
- The seat-selection page exists, while the complete ticket confirmation and payment workflow is still in progress.
- Staff routes do not currently have authorization attributes or role checks.
- Some shared navigation links point to pages that are not implemented yet, including the user index and privacy page.

## License

No license has been specified for this repository yet.
