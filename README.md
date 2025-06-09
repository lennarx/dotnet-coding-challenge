# Unily .NET Coding Challenge

This is the solution for Unily’s backend technical challenge.  
It consists of a simple API to manage users in memory, built with .NET Core 3.1.

## ✔️ Implemented Features

- Full CRUD operations for users
- Pagination support for listing users
- Input validation using FluentValidation
- Structured error handling
- Basic logging
- Unit and integration tests included

## ▶️ How to Run the Project

1. Open the solution in Visual Studio 2019 or later.
2. Set `dotnet.challenge.api` as the startup project.
3. Run the solution (`F5` or `Ctrl+F5`).
4. Swagger UI will be available at: `https://localhost:{port}/swagger`

## 📘 API Endpoints

| Method | Route               | Description                       |
|--------|---------------------|-----------------------------------|
| POST   | `/api/User`         | Create a new user                 |
| GET    | `/api/User`         | Retrieve paginated list of users |
| GET    | `/api/User/{id}`    | Get a user by ID                  |
| PUT    | `/api/User/{id}`    | Update an existing user by ID     |
| DELETE | `/api/User/{id}`    | Delete a user by ID               |

## 🧪 Running Tests

To execute tests:

```bash
dotnet test
