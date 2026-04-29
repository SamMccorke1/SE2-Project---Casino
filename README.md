# Chuds2Chads Casino

Software Engineering II Group Project

## Setup

### Install .NET 10 SDK

Windows:

```powershell
winget install Microsoft.DotNet.SDK.10 --source winget
dotnet --list-sdks
```

### Install EF Core tooling

```powershell
dotnet tool install --global dotnet-ef
dotnet ef --version
```

### Clone and run

```powershell
git clone <YOUR_REPO_URL_HERE>
cd <YOUR_REPO_FOLDER>
cd Chuds2Chads
dotnet restore
dotnet build
dotnet ef database update
dotnet run
```

Open the local URL shown in the terminal, for example:

`http://localhost:5138`

## Database

Make sure `appsettings.json` contains a valid SQLite connection string, for example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=casino.db"
  }
}
```

