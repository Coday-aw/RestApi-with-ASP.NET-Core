## E-Commerce API

A production-ready RESTful API built with ASP.NET Core, Entity Framework Core, and JWT authentication, following a clean Repository–Service architecture.

This project was developed as part of my education at Chas Academy and demonstrates best practices for building scalable, maintainable backend services.

## Features
### Product Management

Full CRUD operations for products

Pagination, filtering, and categorization

DTO-based architecture for clean API contracts

HybridCache integration for faster product retrieval

### Order Management

Create orders linked to the authenticated user

Orders contain order items with product, price, and quantity

Retrieve all orders for a specific user

Entity relationships handled via Entity Framework Core

Order → OrderItems → Product

## Authentication & Security

JWT-based authentication

Secure endpoints using [Authorize]

Extract authenticated UserId from claims

Protected access to user-specific resources

##  Architecture

The project follows a layered clean architecture to keep concerns separated and maintainable.

- Controllers
- Handle HTTP requests and responses
- Services
- Contain business logic
- Handle DTO mapping and validation
- Repositories
- Responsible for database access
- Encapsulate Entity Framework queries
- Entities
- Core domain models
- DTOs
- Request and response models used by the API
- Data
- Database configuration and EF Core migrations

## Tech Stack
- Technology	Purpose
- ASP.NET Core 8	API framework
- Entity Framework Core	ORM and database access
- SQL Server / SQLite	Database
- JWT Authentication	Authentication and authorization
- HybridCache	High-performance caching
- AutoMapper (optional)	DTO mapping

