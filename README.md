# Task Management API (Backend)

This is the backend of the Task Management System, built using **ASP.NET Core**, **Entity Framework Core**, and **JWT Authentication**.

## Requirements
- .NET 6 or later
- EF Core CLI (`dotnet ef`)
- SQLite (or adjust `appsettings.json` for another DB)

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/rtaravind/tms.git
   cd src/tms.api

2. Restore Packages:
     ```bash
     dotnet restore
3. Apply database migrations:
   ```bash
   dotnet ef database update
4. Run the API:
   ```bash
   dotnet run
5.API runs at: https://localhost:7025


---

### **Frontend Repo (`tms-app`)**

And in the frontend repo:

```md
# Task Management App (Frontend)

This is the **React frontend** for the Task Management System.

## Requirements

- Node.js (v16 or later)
- npm or yarn

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/rtaravind/tms-app.git
   cd tms-app
2. Install dependencies:
   npm install
3.Start the development server:
   npm start
4. App runs at: http://localhost:3000

----

##** Note**
The API is hosted at https://localhost:7025. If you're deploying or using a different port/IP, make sure to reflect that in the frontend configuration.
### **CORS Setup**
By default, the API allows requests from `http://localhost:3000`.
If you're running the frontend from a different origin (e.g., different port or IP), update the CORS policy in `Program.cs`

     
   
