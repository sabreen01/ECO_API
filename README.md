# 🛒 eCommerce API

A professional **RESTful eCommerce API** built with **.NET 9** using **N-Tier Architecture**, featuring JWT authentication, pagination, filtering, and complete CRUD operations.

---

## 📋 Table of Contents

- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Architecture](#-architecture)
- [Getting Started](#-getting-started)
- [Database Schema](#-database-schema)
- [API Endpoints](#-api-endpoints)
- [Authentication](#-authentication)
- [Project Structure](#-project-structure)
- [Configuration](#-configuration)

---

## ✨ Features

- ✅ **N-Tier Architecture** (Domain, Infrastructure, Application, Presentation)
- ✅ **Generic Repository Pattern** with Unit of Work
- ✅ **JWT Authentication & Authorization** with Role-Based Access Control
- ✅ **Pagination, Filtering & Sorting** for efficient data retrieval
- ✅ **Soft Delete** mechanism for data integrity
- ✅ **EF Core Code-First** with Migrations
- ✅ **Eager Loading** support for related entities
- ✅ **DTOs** for clean separation between layers
- ✅ **Password Hashing** with BCrypt
- ✅ **Swagger UI** for API documentation
- ✅ **Business Logic Validation** in Service Layer
- ✅ **Transaction Management** with Unit of Work pattern

---

## 🛠 Tech Stack

- **Framework**: .NET 9
- **ORM**: Entity Framework Core 9
- **Database**: SQL Server 2022
- **Authentication**: JWT Bearer Tokens
- **Password Hashing**: BCrypt.Net
- **Container**: Docker (SQL Server)
- **IDE**: JetBrains Rider
- **API Documentation**: Swagger/OpenAPI

---

## 🏗 Architecture

```
┌─────────────────────────────────────────────────┐
│                 Presentation Layer              │
│              (ECommerce.API)                    │
│          Controllers, Middleware                │
└────────────────┬────────────────────────────────┘
                 │
┌────────────────▼────────────────────────────────┐
│              Application Layer                  │
│           (ECommerce.Application)               │
│      Services, DTOs, Business Logic             │
└────────────────┬────────────────────────────────┘
                 │
┌────────────────▼────────────────────────────────┐
│            Infrastructure Layer                 │
│         (ECommerce.Infrastructure)              │
│   DbContext, Repository, UnitOfWork             │
└────────────────┬────────────────────────────────┘
                 │
┌────────────────▼────────────────────────────────┐
│                Domain Layer                     │
│             (ECommerce.Domain)                  │
│          Entities, Enums, BaseEntity            │
└─────────────────────────────────────────────────┘
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [JetBrains Rider](https://www.jetbrains.com/rider/) or Visual Studio 2022

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/ecommerce-api.git
   cd ecommerce-api
   ```

2. **Start SQL Server with Docker**
   ```bash
   docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" \
     -p 1433:1433 --name ecommerce-sqlserver \
     -d mcr.microsoft.com/mssql/server:2022-latest
   ```

3. **Update Connection String** (if needed)
   
   Edit `src/ECommerce.API/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost,1433;Database=ECommerceDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True"
   }
   ```

4. **Apply Migrations**
   ```bash
   dotnet ef database update \
     --project src/ECommerce.Infrastructure \
     --startup-project src/ECommerce.API
   ```

5. **Run the API**
   ```bash
   dotnet run --project src/ECommerce.API
   ```

6. **Open Swagger UI**
   ```
   https://localhost:7xxx/swagger
   ```

---

## 💾 Database Schema

### Tables

- **Users** - Customer and Admin accounts
- **Categories** - Product categories with parent-child relationships
- **Products** - Product catalog with prices and stock
- **Orders** - Customer orders with status tracking
- **OrderItems** - Individual items within orders

### Entity Relationships

```
Users (1) ──────< Orders (N)
                     │
                     └──────< OrderItems (N) >────── Products (1)
                                                          │
Categories (1) ──────< Products (N)                      │
     │                                                    │
     └──────< SubCategories (N)                          │
```

### Soft Delete

All entities inherit from `BaseEntity` which includes:
- `Id` (int, Primary Key)
- `CreatedAt` (DateTime)
- `UpdatedAt` (DateTime?)
- `IsDeleted` (bool) - For soft delete functionality

---

## 📡 API Endpoints

### Authentication

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/api/auth/register` | Register new user | Public |
| POST | `/api/auth/login` | Login and get JWT token | Public |

### Products

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/products` | Get all products (paginated) | Public |
| GET | `/api/products/{id}` | Get product by ID | Public |
| POST | `/api/products` | Create new product | Admin |
| PUT | `/api/products/{id}` | Update product | Admin |
| DELETE | `/api/products/{id}` | Delete product (soft) | Admin |

**Query Parameters for GET /api/products:**
```
?pageNumber=1
&pageSize=10
&search=iphone
&categoryId=1
&minPrice=100
&maxPrice=1000
&isActive=true
&sortBy=price
&sortOrder=desc
```

### Categories

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/categories` | Get all categories | Public |
| GET | `/api/categories/{id}` | Get category by ID | Public |
| POST | `/api/categories` | Create new category | Admin |
| DELETE | `/api/categories/{id}` | Delete category (soft) | Admin |

### Orders

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/orders/{id}` | Get order by ID | Authenticated |
| POST | `/api/orders` | Create new order | Authenticated |
| PATCH | `/api/orders/{id}/status` | Update order status | Admin |

---

## 🔐 Authentication

### Registration

```bash
POST /api/auth/register
Content-Type: application/json

{
  "firstName": "Ahmed",
  "lastName": "Ali",
  "email": "ahmed@example.com",
  "password": "SecurePass@123",
  "phoneNumber": "+201234567890",
  "address": "123 Main St, Cairo"
}
```

**Response:**
```json
{
  "userId": 1,
  "email": "ahmed@example.com",
  "firstName": "Ahmed",
  "lastName": "Ali",
  "role": "Customer",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Login

```bash
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@ecommerce.com",
  "password": "Admin@123"
}
```

### Using the Token

Include the token in the Authorization header:

```bash
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Default Accounts

After seeding, you can use:

**Admin Account:**
- Email: `admin@ecommerce.com`
- Password: `Admin@123`

**Customer Account:**
- Email: `ahmed@test.com`
- Password: `Test@123`

---

## 📁 Project Structure

```
ECommerceAPI/
│
├── src/
│   ├── ECommerce.Domain/
│   │   ├── Entities/
│   │   │   ├── BaseEntity.cs
│   │   │   ├── Category.cs
│   │   │   ├── Order.cs
│   │   │   ├── OrderItem.cs
│   │   │   ├── Product.cs
│   │   │   └── User.cs
│   │   └── Enums/
│   │       └── OrderStatus.cs
│   │
│   ├── ECommerce.Infrastructure/
│   │   ├── Data/
│   │   │   ├── Configurations/
│   │   │   │   ├── CategoryConfiguration.cs
│   │   │   │   ├── OrderConfiguration.cs
│   │   │   │   ├── OrderItemConfiguration.cs
│   │   │   │   ├── ProductConfiguration.cs
│   │   │   │   └── UserConfiguration.cs
│   │   │   ├── Repositories/
│   │   │   │   ├── Repository.cs
│   │   │   │   └── UnitOfWork.cs
│   │   │   ├── AppDbContext.cs
│   │   │   └── AppDbContextSeed.cs
│   │   └── Interfaces/
│   │       ├── IRepository.cs
│   │       └── IUnitOfWork.cs
│   │
│   ├── ECommerce.Application/
│   │   ├── Common/
│   │   │   ├── PagedResult.cs
│   │   │   ├── PaginationParams.cs
│   │   │   └── ProductQueryParams.cs
│   │   ├── DTOs/
│   │   │   ├── Auth/
│   │   │   │   ├── AuthResponseDto.cs
│   │   │   │   ├── LoginDto.cs
│   │   │   │   └── RegisterDto.cs
│   │   │   ├── Category/
│   │   │   │   ├── CategoryDto.cs
│   │   │   │   └── CreateCategoryDto.cs
│   │   │   ├── Order/
│   │   │   │   ├── CreateOrderDto.cs
│   │   │   │   └── OrderDto.cs
│   │   │   └── Product/
│   │   │       ├── CreateProductDto.cs
│   │   │       ├── ProductDto.cs
│   │   │       └── UpdateProductDto.cs
│   │   └── Services/
│   │       ├── AuthService.cs
│   │       ├── CategoryService.cs
│   │       ├── OrderService.cs
│   │       └── ProductService.cs
│   │
│   └── ECommerce.API/
│       ├── Controllers/
│       │   ├── AuthController.cs
│       │   ├── CategoriesController.cs
│       │   ├── OrdersController.cs
│       │   └── ProductsController.cs
│       ├── appsettings.json
│       └── Program.cs
│
└── ECommerceAPI.sln
```

---

## ⚙️ Configuration

### JWT Settings

Edit `appsettings.json` to customize JWT configuration:

```json
{
  "JwtSettings": {
    "Secret": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "ECommerceAPI",
    "Audience": "ECommerceClient",
    "ExpiryInMinutes": 60
  }
}
```

### Connection String

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=ECommerceDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True"
  }
}
```

---

## 📝 Sample Requests

### Create Product (Admin Only)

```bash
POST /api/products
Authorization: Bearer {admin-token}
Content-Type: application/json

{
  "name": "iPhone 15 Pro Max",
  "description": "Latest Apple flagship",
  "price": 1299.99,
  "stockQuantity": 50,
  "imageUrl": "https://example.com/iphone15.jpg",
  "categoryId": 1
}
```

### Create Order (Authenticated)

```bash
POST /api/orders
Authorization: Bearer {user-token}
Content-Type: application/json

{
  "userId": 2,
  "shippingAddress": "123 Main St, Cairo, Egypt",
  "items": [
    {
      "productId": 1,
      "quantity": 2
    },
    {
      "productId": 5,
      "quantity": 1
    }
  ]
}
```

### Get Products with Filters

```bash
GET /api/products?pageNumber=1&pageSize=10&search=phone&categoryId=1&minPrice=500&maxPrice=1500&sortBy=price&sortOrder=desc
```

---


## 🎯 Design Patterns Used

1. **Repository Pattern** - Abstraction over data access
2. **Unit of Work Pattern** - Transaction management
3. **Dependency Injection** - Loose coupling
4. **DTO Pattern** - Data transfer between layers
5. **Factory Pattern** - Repository creation in UnitOfWork
6. **Strategy Pattern** - Different sorting/filtering strategies

---

## 🔒 Security Features

- ✅ Password hashing with BCrypt
- ✅ JWT token authentication
- ✅ Role-based authorization
- ✅ Input validation in services
- ✅ SQL injection prevention (EF Core parameterized queries)
- ✅ Soft delete for data recovery

---

## 📊 Business Logic Examples

### Order Creation Process

1. Validate user exists
2. Check product availability and stock
3. Verify product is active
4. Calculate the total amount
5. Update product stock (decrements)
6. Create an order with items
7. All operations in a single transaction (rollback on failure)

### Soft Delete

Instead of permanently deleting records:
- Set `IsDeleted = true`
- Update `UpdatedAt` timestamp
- Global query filter automatically excludes deleted records
- Data remains in the database for audit/recovery

---

## 🚧 Future Enhancements

- [ ] Global Exception Handling Middleware
- [ ] AutoMapper for DTO mapping
- [ ] FluentValidation for input validation
- [ ] Refresh Token mechanism
- [ ] Email notifications for orders
- [ ] File upload for product images
- [ ] Redis caching for performance
- [ ] Unit and Integration tests
- [ ] Docker Compose setup
- [ ] CI/CD pipeline
