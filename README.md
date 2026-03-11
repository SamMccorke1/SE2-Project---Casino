# 🎰 Chuds2Chads Casino  
Software Engineering II Group Project


# Setup Instructions

## Install .NET 10 SDK
###  (Windows)
```powershell
winget install Microsoft.DotNet.SDK.10 --source winget
Verify
dotnet--list-sdks
Install Ef Tool
dotnet tool install --global dotnet-ef
Verify 
dotnet ef --version
Clone the Repository
git clone <YOUR_REPO_URL_HERE>
cd <YOUR_REPO_FOLDER>
cd Chuds2Chads
Restore and Build
dotnet restore
dotnet build
Database Setup
Make sure appsettings.json contains:
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=casino.db"
  }
}
Then Run
dotnet ef database update
If the migrations do not exist
dotnet ef database update
Run the app
dotnet run
Open the local URL shown in the termial is such as:
http://localhost:5138

