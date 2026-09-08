# 🏃 Runrs

A run club management web application that lets users create and manage running clubs, track their runs, and connect with other runners.

<!-- Optional: badges — remove any you don't want, or add your own from shields.io -->
![C#](https://img.shields.io/badge/C%23-239120?style=flat&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/ASP.NET%20Core%20MVC-512BD4?style=flat&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=flat&logo=microsoft-sql-server&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-7952B3?style=flat&logo=bootstrap&logoColor=white)

<!--
Demo video — drag your screen recording into this README on GitHub.com,
then replace the link below with the user-attachments URL it generates.
-->
<div align="center">
  <video src="REPLACE-WITH-YOUR-GITHUB-VIDEO-URL" width="700" controls>
  </video>
</div>

<!--
Screenshot — upload an image to /assets and reference it here,
or drag-and-drop directly into the README editor on GitHub.com.
-->
<p align="center">
  <img src="./assets/dashboard.png" alt="Runrs dashboard" width="700"/>
</p>

---

## 📖 About

Runrs is a web application built for managing running clubs. Members can join or create clubs, log their runs, and interact with other runners in the same club. The app was built as a class assignment, with a focus on applying solid backend architecture patterns alongside a fully custom front-end.

## ✨ Features

- Create and manage running clubs
- Log and track individual runs
- Connect and interact with other runners
- Secure user authentication and password hashing
- Rich text content editing for club posts/announcements
- Payment integration for club memberships or events

<!-- Add/remove features above to match what's actually implemented -->

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Language | C# |
| Framework | ASP.NET Core MVC |
| Database | SQL Server |
| ORM | Entity Framework Core |
| Architecture | Repository Pattern, Unit of Work |
| Auth | BCrypt (password hashing) |
| Payments | Stripe Checkout |
| Rich Text Editor | TinyMCE |
| Front-end | HTML/CSS, Bootstrap (customised) |

## 🏗️ Architecture

The application follows the **Repository Pattern** combined with **Unit of Work** to separate data access logic from business logic, making the codebase easier to test and maintain. Entity Framework Core handles the ORM layer against a SQL Server database.

<!-- Optional: add a simple diagram here if you have one -->

## 🚀 Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (specify version, e.g. .NET 8)
- SQL Server (LocalDB or full instance)
- A Stripe account (for payment features, test/sandbox keys are fine)

### Installation

1. Clone the repository
   ```bash
   git clone https://github.com/your-username/runrs.git
   cd runrs
   ```

2. Restore dependencies
   ```bash
   dotnet restore
   ```

3. Configure your connection string and secrets in `appsettings.json` (or `appsettings.Development.json`):
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=RunrsDb;Trusted_Connection=True;"
     },
     "Stripe": {
       "SecretKey": "your-stripe-secret-key",
       "PublishableKey": "your-stripe-publishable-key"
     }
   }
   ```

4. Apply database migrations
   ```bash
   dotnet ef database update
   ```

5. Run the application
   ```bash
   dotnet run
   ```

6. Open your browser at `https://localhost:5001` (or whatever port is shown in the console)

## 📂 Project Structure

```
Runrs/
├── Controllers/
├── Models/
├── Views/
├── Repositories/
├── Data/
├── wwwroot/
└── appsettings.json
```

<!-- Adjust the tree above to match your actual folder layout -->

## 📄 Documentation

- 📄 [Project Documentation (PDF)](Documentation/RUNRS_Documentation.pdf)


<!-- Update file paths above to match your actual /docs filenames -->

## 👤 Author

**Byron**
Third-year BIT student, Whitireia/WelTec (W&W)

## 📜 License

<!-- Add a license if relevant, e.g. MIT, or remove this section -->
This project was developed as part of a class assignment at WelTec.
