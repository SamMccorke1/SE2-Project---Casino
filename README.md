# Chuds2Chads Casino (SE2 Group Project)

Blazor Server casino-style web application built for Software Engineering II.

This project includes:
- ASP.NET Core Identity (Email + Password login)
- Role support (Admin + User)
- SQLite database
- Entity Framework Core migrations
- Wallet + transaction tracking
- Multiplayer-ready game rooms and sessions

---

# 🧰 Tech Stack

- .NET 9.0
- Blazor Server (Interactive Server Components)
- SQLite
- Entity Framework Core
- ASP.NET Core Identity
- GitHub
- Visual Studio / VS Code

---

# ✅ Step 1 — Install .NET 9.0 SDK

## Option A (Recommended – Windows)

Open PowerShell and run:

```powershell
winget install Microsoft.DotNet.SDK.9 --source winget
```

Verify installation:

```powershell
dotnet --list-sdks
```

You should see something like:

```
9.0.xxx [C:\Program Files\dotnet\sdk]
```

---

## Option B (Manual Installer)

1. Go to:
   https://dotnet.microsoft.com/download

2. Download:
   **.NET SDK 9.0.x**

3. Install it.

4. Verify:

```powershell
dotnet --list-sdks
```

---

# 📦 Step 2 — Required NuGet Packages

Navigate to the folder containing `Chuds2Chads.csproj`:

```powershell
cd ChudstoChads
```

Then install required packages:

```powershell
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
```

---

# 🛠 Step 3 — Install EF Core CLI Tool (Required)

Run once on your machine:

```powershell
dotnet tool install --global dotnet-ef
```

Verify:

```powershell
dotnet ef --version
```

---

# 🚀 Step 4 — Clone and Run the Project

Clone the repository:

```powershell
git clone <YOUR_REPO_URL_HERE>
```

Go into the repo:

```powershell
cd <YOUR_REPO_FOLDER>
```

Go into the project folder (the one containing `Chuds2Chads.csproj`):

```powershell
cd ChudstoChads
```

Restore + Build:

```powershell
dotnet restore
dotnet build
```

Run the project:

```powershell
dotnet run
```

You will see a local address like:

```
http://localhost:5138
```

Open that in your browser.

---

# 🗄 Database Setup (SQLite + Migrations)

This project uses SQLite with EF Core migrations.

## Step 1 — Confirm Connection String

Open `appsettings.json` and confirm:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=casino.db"
  }
}
```

This creates a local SQLite file named:

```
casino.db
```

in the project folder.

---

## Step 2 — Create Database on Your Machine

From inside the project folder:

```powershell
dotnet ef database update
```

This will automatically:

- Create the SQLite database file
- Create all Identity tables
- Create Wallets table
- Create Transactions table
- Create GameRooms
- Create RoomPlayers
- Create GameSessions
- Apply all migrations

---

## Step 3 — If Migrations Do Not Exist

If there is no `Migrations` folder in the project:

```powershell
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

# 👥 Default Login (If Seeded)

If the project includes seed data, example admin login:

```
Email: admin@chuds2chads.local
Password: Admin1234!
```

(Only works if seed data is configured.)

---

# ❗ Common Issues

## .NET 9 not found
Run:

```powershell
dotnet --list-sdks
```

If 9.0 does not appear, reinstall the SDK.

---

## dotnet-ef not found
Install:

```powershell
dotnet tool install --global dotnet-ef
```

---

## Database file not appearing
Run:

```powershell
dotnet ef database update
```

Then check for `casino.db` in the project folder.

---

# ⚠ Important

- This is an educational project.
- No real money transactions occur.
- All currency is virtual and stored in SQLite.
- Multiplayer is handled via GameRooms and RoomPlayers tables.

---

# 📌 Summary

To run this project on a new machine:

1. Install .NET 9
2. Install EF tool
3. Clone repo
4. Run:
   ```powershell
   dotnet restore
   dotnet build
   dotnet ef database update
   dotnet run
   ```

You're ready to go.

