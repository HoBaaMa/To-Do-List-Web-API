# To-Do List Web API

A simple ASP.NET Core Web API for managing to-do items. This project demonstrates CRUD operations, filtering by date, and supports JSON Patch
for partial updates.

---

## 📋 Table of Contents

- [Features](#features)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Setup](#setup)
- [API Endpoints](#api-endpoints)
  - [Example To-Do Item JSON](#example-to-do-item-json)
  - [JSON Patch Example](#json-patch-example)
- [Model](#model)
- [Error Handling](#error-handling)
- [📦 Required NuGet Packages](#-required-nuget-packages)
  - [🚀 Quick Install All](#-quick-install-all)
- [License](#license)

---

## Features

- Create, read, update, and delete to-do items
- Filter to-do items by creation date
- Partial updates using JSON Patch
- Entity Framework Core with SQL Server
- OpenAPI/Swagger documentation

## Technologies Used

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Swashbuckle (Swagger)
- Newtonsoft.Json (for JSON Patch support)

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Setup

1. **Clone the repository:**
- git clone https://github.com/HoBaaMa/To-Do-List-Web-AP.git
2. **Configure the database connection:**
- Update the `DefaultConnection` string in `appsettings.json` with your SQL Server details.
3. **Apply migrations:**
- dotnet ef database update
4. **Run the application:**
- dotnet run
5. **Access Swagger UI:**
- Navigate to `https://localhost:{port}/swagger` in your browser.

---

## API Endpoints

| Method | Endpoint                  | Description                        |
|--------|---------------------------|------------------------------------|
| GET    | `/api/todo`               | Get all to-do items                |
| GET    | `/api/todo/{id}`          | Get a to-do item by ID             |
| GET    | `/api/todo/date/{date}`   | Get to-do items by creation date   |
| POST   | `/api/todo`               | Create a new to-do item            |
| PUT    | `/api/todo/{id}`          | Update an existing to-do item      |
| PATCH  | `/api/todo/{id}`          | Partially update a to-do item      |
| DELETE | `/api/todo/{id}`          | Delete a to-do item                |

### Example To-Do Item JSON
{
  "id": 1,
  "title": "Sample To-Do Item",
  "isCompleted": false,
  "createdAt": "2023-10-01T12:00:00Z",
  "completedAt": "2023-10-10T12:00:00Z"
}
# Response
{
  "id": 1,
  "title": "Sample To-Do Item",
  "isCompleted": false,
  "createdAt": "2023-10-01T12:00:00Z",
  "completedAt": "2023-10-10T12:00:00Z"
}
- `id`, `createdAt`, and `completedAt` are managed by the API/database.

---

## JSON Patch Example

To mark a to-do item as completed:

**Request:**
PATCH /api/todo/1
Content-Type: application/json-patch+json

[
  {
    "op": "replace",
    "path": "/isCompleted",
    "value": true
  },
  {
    "op": "set",
    "path": "/completedAt",
    "value": "2023-10-01T12:00:00Z"
  }
]
# Response{
  "id": 1,
  "title": "Sample To-Do Item",
  "isCompleted": true,
  "createdAt": "2023-10-01T12:00:00Z",
  "completedAt": "2023-10-01T12:00:00Z"
}

---

## Model
<code>```csharp</code>
    public class TodoItem
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime? CompletedAt { get; set; }
    }
<code>```</code>
---

## Error Handling

- Returns `400 Bad Request` for invalid input or out-of-range IDs.
- Returns `404 Not Found` if the item does not exist.

---

## 📦 Required NuGet Packages

To run and develop this project, make sure you have the following NuGet packages installed:

| Package Name                                      | Description                                 | NuGet Command                                               |
|---------------------------------------------------|---------------------------------------------|-------------------------------------------------------------|
| `Microsoft.EntityFrameworkCore`                   | 🗄️ Entity Framework Core ORM                | `dotnet add package Microsoft.EntityFrameworkCore`          |
| `Microsoft.EntityFrameworkCore.SqlServer`         | 🛢️ SQL Server provider for EF Core          | `dotnet add package Microsoft.EntityFrameworkCore.SqlServer`|
| `Microsoft.EntityFrameworkCore.Tools`             | 🛠️ EF Core CLI tools (for migrations, etc.) | `dotnet add package Microsoft.EntityFrameworkCore.Tools`    |
| `Microsoft.AspNetCore.JsonPatch`                  | 🩹 JSON Patch support for ASP.NET Core       | `dotnet add package Microsoft.AspNetCore.JsonPatch`         |
| `Microsoft.AspNetCore.Mvc.NewtonsoftJson`         | 🧩 Newtonsoft.Json integration               | `dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson`|
| `Swashbuckle.AspNetCore`                          | 📖 Swagger/OpenAPI for API documentation     | `dotnet add package Swashbuckle.AspNetCore`                 |

---

### 🚀 Quick Install All

You can install all required packages with these commands:

---

## License

This project is licensed under the MIT License.

