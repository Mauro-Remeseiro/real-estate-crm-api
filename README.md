# Real Estate CRM API

Backend API for a real estate CRM system built with **ASP.NET Core**, **Entity Framework Core**, **SQL Server**, **JWT Authentication** and **Docker**.

This project was developed as a backend portfolio project, focusing on real-world API development practices such as clean architecture, authentication, database persistence, migrations, protected endpoints and local development with Docker.

---

## Project Overview

Real Estate CRM API is a backend application designed to manage core operations of a real estate business.

The system currently supports:

- User registration and login
- JWT-based authentication
- Property management
- Client management
- Visit scheduling
- Agent-based ownership using authenticated users
- SQL Server persistence
- Swagger/OpenAPI documentation

The goal of this project is to demonstrate backend development skills using technologies commonly used in professional .NET environments.

---

## Tech Stack

- **C#**
- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **JWT Authentication**
- **Swagger / OpenAPI**
- **Docker Compose**
- **Git**
- **Clean Architecture principles**

---

## Architecture

The solution follows a clean and maintainable layered structure:

```text
real-estate-crm-api/
│
├── src/
│   ├── RealEstateCrmApi.Api/
│   ├── RealEstateCrmApi.Application/
│   ├── RealEstateCrmApi.Domain/
│   └── RealEstateCrmApi.Infrastructure/
│
├── tests/
│   ├── RealEstateCrmApi.Domain.UnitTests/
│   └── RealEstateCrmApi.Api.IntegrationTests/
│
├── docker-compose.yml
├── .env.example
└── README.md