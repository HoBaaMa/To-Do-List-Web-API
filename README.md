# 📝 To-Do List Web API

A robust and professional ASP.NET Core Web API for managing to-do items with comprehensive CRUD operations, advanced filtering capabilities, and JSON Patch support for partial updates. Built with modern .NET 8 features, Entity Framework Core, structured logging with Serilog, and custom validation attributes to ensure data integrity.

---

## 📋 Table of Contents

- [✨ Features](#-features)
- [🧰 Technologies Used](#-technologies-used)
- [🚀 Getting Started](#-getting-started)
  - [🔧 Prerequisites](#-prerequisites)
  - [⚙️ Setup](#-setup)
- [🌐 API Endpoints](#-api-endpoints)
- [📄 Example JSON & Request/Response](#-example-json--requestresponse)
- [🩹 JSON Patch Examples](#-json-patch-examples)
- [🧱 Models](#-models)
- [❗ Error Handling](#-error-handling)
- [📦 Required NuGet Packages](#-required-nuget-packages)
- [🚀 Quick Install Command](#-quick-install-command)
- [📖 Project Architecture](#-project-architecture)
- [🛡️ Validation & Business Rules](#️-validation--business-rules)
- [📊 Logging & Monitoring](#-logging--monitoring)
- [🪪 License](#-license)

---

## ✨ Features

- **Full CRUD Operations**: Create, read, update, and delete to-do items with comprehensive validation
- **Advanced Filtering**: Filter to-do items by creation date with intelligent date-based queries
- **JSON Patch Support**: Partial updates using standardized JSON Patch operations
- **Data Validation**: Custom validation attributes ensuring business rule compliance
- **Structured Logging**: Comprehensive logging with Serilog for monitoring and debugging
- **Entity Framework Core**: Modern ORM with SQL Server integration and database migrations
- **Repository Pattern**: Clean architecture with separation of concerns
- **Swagger Integration**: Auto-generated API documentation with interactive testing interface
- **Error Handling**: Robust error handling with appropriate HTTP status codes and logging

---

## 🧰 Technologies Used

- **.NET 8** - Latest LTS version of .NET
- **ASP.NET Core** - Modern web framework for building APIs
- **Entity Framework Core 9.0.7** - Object-relational mapping (ORM)
- **SQL Server** - Relational database management system
- **Swagger/OpenAPI** - API documentation and testing interface
- **Serilog** - Structured logging framework with multiple sinks
- **Newtonsoft.Json** - JSON serialization and JSON Patch support

---

## 🚀 Getting Started

### 🔧 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download) or later
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express/LocalDB acceptable for development)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/) (optional but recommended)

### ⚙️ Setup

1. **Clone the repository:**
   ```bash
   git clone https://github.com/HoBaaMa/To-Do-List-Web-API.git
   cd To-Do-List-Web-API
   ```

2. **Navigate to the project directory:**
   ```bash
   cd "To-Do List Web API"
   ```

3. **Configure the database connection:**
   
   Update the `DefaultConnection` string in `appsettings.json` with your SQL Server details:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=.;Initial Catalog=ToDoList;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True"
     }
   }
   ```

4. **Install dependencies:**
   ```bash
   dotnet restore
   ```

5. **Apply database migrations:**
   ```bash
   dotnet ef database update
   ```

6. **Run the application:**
   ```bash
   dotnet run
   ```

7. **Access Swagger UI:**
   
   Navigate to `https://localhost:7XXX/swagger` in your browser (port number will be displayed in console).

---

## 🌐 API Endpoints

| 🔠 Method | 🌐 Endpoint                 | 📝 Description                           |
|----------|-----------------------------|------------------------------------------|
| 🟢 GET   | `/api/todo`                 | Retrieve all to-do items                 |
| 🔍 GET   | `/api/todo/{id}`            | Retrieve a specific to-do item by ID     |
| 📅 GET   | `/api/todo/date/{date}`     | Retrieve to-do items by creation date    |
| ➕ POST  | `/api/todo`                 | Create a new to-do item                  |
| ♻️ PUT   | `/api/todo/{id}`            | Update an existing to-do item completely |
| 🩹 PATCH | `/api/todo/{id}`            | Partially update a to-do item            |
| ❌ DELETE| `/api/todo/{id}`            | Delete a to-do item                      |

---

## 📄 Example JSON & Request/Response

### Create To-Do Item (POST)

**Request:**
```json
{
  "title": "Complete project documentation",
  "description": "Write comprehensive README and API documentation",
  "isCompleted": false
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "title": "Complete project documentation",
  "description": "Write comprehensive README and API documentation",
  "isCompleted": false,
  "createdAt": "2024-01-15T10:30:00Z",
  "completedAt": null
}
```

### Update To-Do Item (PUT)

**Request:**
```json
{
  "id": 1,
  "title": "Complete project documentation",
  "description": "Write comprehensive README and API documentation",
  "isCompleted": true,
  "createdAt": "2024-01-15T10:30:00Z",
  "completedAt": "2024-01-15T14:45:00Z"
}
```

---

## 🩹 JSON Patch Examples

### Mark Item as Completed
```json
[
  {
    "op": "replace",
    "path": "/isCompleted",
    "value": true
  },
  {
    "op": "replace",
    "path": "/completedAt",
    "value": "2024-01-15T14:45:00Z"
  }
]
```

### Update Title and Description
```json
[
  {
    "op": "replace",
    "path": "/title",
    "value": "Updated task title"
  },
  {
    "op": "replace",
    "path": "/description",
    "value": "Updated task description with more details"
  }
]
```

### Add Description to Existing Item
```json
[
  {
    "op": "add",
    "path": "/description",
    "value": "This is a newly added description"
  }
]
```

---

## 🧱 Models

### TodoItem
```csharp
[CompletedStateValidation]
[CompletedCreationDatesValidation]
public class TodoItem
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string Title { get; set; }
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    public bool? IsCompleted { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? CompletedAt { get; set; }
}
```

### Model Constraints
- **Title**: Required, maximum 200 characters (database), 100 characters (model validation)
- **Description**: Optional, maximum 1000 characters (database), 500 characters (model validation)
- **IsCompleted**: Optional boolean, defaults to false
- **CreatedAt**: Automatically set to current timestamp on creation
- **CompletedAt**: Set when item is marked as completed

---

## ❗ Error Handling

The API provides comprehensive error handling with appropriate HTTP status codes:

### Error Response Format
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "detail": "Title cannot be null or empty."
}
```

### Common Error Scenarios

| Status Code | Scenario | Example Message |
|-------------|----------|-----------------|
| `400 Bad Request` | Invalid input data | "Title cannot be null or empty." |
| `400 Bad Request` | Invalid ID range | "ID must be greater than zero." |
| `400 Bad Request` | Validation failures | "CompletedAt must be set when IsCompleted is true." |
| `404 Not Found` | Item not found | "Todo Item with ID: 5 not found." |
| `404 Not Found` | No items for date | "No tasks found for the date: 1/15/2024" |
| `500 Internal Server Error` | Unexpected errors | "An internal server error occurred." |

---

## 📦 Required NuGet Packages

| 📦 Package Name | Version | 📝 Description |
|----------------|---------|----------------|
| `Microsoft.EntityFrameworkCore` | 9.0.7 | Entity Framework Core ORM framework |
| `Microsoft.EntityFrameworkCore.SqlServer` | 9.0.7 | SQL Server database provider for EF Core |
| `Microsoft.EntityFrameworkCore.Tools` | 9.0.7 | EF Core command-line tools for migrations |
| `Microsoft.AspNetCore.JsonPatch` | 9.0.7 | JSON Patch support for partial updates |
| `Microsoft.AspNetCore.Mvc.NewtonsoftJson` | 8.0.18 | Newtonsoft.Json integration for ASP.NET Core |
| `Swashbuckle.AspNetCore` | 6.6.2 | Swagger/OpenAPI documentation generator |
| `Serilog.AspNetCore` | 9.0.0 | Serilog integration for ASP.NET Core |
| `Serilog.Settings.Configuration` | 9.0.0 | Configuration support for Serilog |
| `Serilog.Sinks.Console` | 6.0.0 | Console output sink for Serilog |
| `Serilog.Sinks.File` | 7.0.0 | File output sink for Serilog |

---

## 🚀 Quick Install Command

Install all required packages at once:

```bash
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.7
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.7
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.7
dotnet add package Microsoft.AspNetCore.JsonPatch --version 9.0.7
dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson --version 8.0.18
dotnet add package Swashbuckle.AspNetCore --version 6.6.2
dotnet add package Serilog.AspNetCore --version 9.0.0
dotnet add package Serilog.Settings.Configuration --version 9.0.0
dotnet add package Serilog.Sinks.Console --version 6.0.0
dotnet add package Serilog.Sinks.File --version 7.0.0
```

---

## 📖 Project Architecture

### Repository Pattern
The project implements the Repository pattern for clean separation of concerns:

- **Controllers**: Handle HTTP requests and responses
- **Repositories**: Encapsulate data access logic
- **Models**: Define data structures and validation rules
- **Data**: Entity Framework DbContext and migrations
- **Attributes**: Custom validation attributes for business rules

### Folder Structure
```
To-Do List Web API/
├── Attributes/          # Custom validation attributes
├── Controllers/         # API controllers
├── Data/               # Entity Framework DbContext
├── Migrations/         # Database migrations
├── Models/             # Data models and DTOs
├── Repositories/       # Repository interfaces and implementations
└── Program.cs          # Application entry point
```

---

## 🛡️ Validation & Business Rules

### Custom Validation Attributes

1. **CompletedStateValidationAttribute**
   - Ensures `CompletedAt` is set when `IsCompleted` is true
   - Ensures `CompletedAt` is null when `IsCompleted` is false

2. **CompletedCreationDatesValidationAttribute**
   - Validates that `CreatedAt` is earlier than `CompletedAt`
   - Prevents logical inconsistencies in task completion dates

### Data Validation
- **Title**: Required field with maximum length constraints
- **Description**: Optional with length limitations
- **ID Validation**: Ensures positive integer values
- **Date Validation**: Automatic timestamp management and validation

---

## 📊 Logging & Monitoring

### Serilog Configuration

The application uses Serilog for structured logging with the following configuration:

- **Console Output**: Formatted logs for development
- **File Output**: Rolling daily log files in `logs/` directory
- **Structured Logging**: Logs include contextual information for better debugging

### Log Levels
- **Information**: API calls, successful operations
- **Warning**: Validation failures, not found scenarios
- **Error**: Unexpected exceptions and system errors

### Log Examples
```
2024-01-15 10:30:00 [INF] [To-Do-List-WebAPI/Server-125.08.13.1] API call received to display all the tasks.
2024-01-15 10:30:01 [INF] [To-Do-List-WebAPI/Server-125.08.13.1] 5 tasks retrieved successfully.
```

---

## 🪪 License

This project is licensed under the [MIT License](LICENSE.txt).
