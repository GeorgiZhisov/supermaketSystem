# 🛒 SupermarketManager System

A full-featured **Supermarket Management System** built with **ASP.NET Core 8 MVC**, developed as a final project for the "ASP.NET MVC" course at SoftUni Buditel.

---

## 📋 Project Overview

SupermarketManager enables supermarket managers to handle daily operations from a single web interface:

- Manage products with barcodes, pricing, stock levels, and supplier info
- Organise products into categories
- Track suppliers and their contact details
- Monitor employees and their positions
- Submit and approve restock requests for low-inventory items
- View an admin dashboard with real-time inventory statistics

---

## ⚙️ Technologies Used

| Technology | Version |
|---|---|
| ASP.NET Core MVC | 8.0 |
| Entity Framework Core | 8.0 |
| Microsoft SQL Server | SQLEXPRESS |
| ASP.NET Core Identity | 8.0 |
| Bootstrap | 5.3 |
| Bootstrap Icons | 1.11 |
| NUnit | 4.1 |
| Moq | 4.20 |

---

## 🗂️ Solution Structure

```
SupermarketManagerSystem/
├── SupermarketManagerSystem.Data/          # EF Core DbContext, Models, Migrations
│   ├── Models/
│   │   ├── ApplicationUser.cs
│   │   ├── Product.cs
│   │   ├── Category.cs
│   │   ├── Supplier.cs
│   │   ├── Employee.cs
│   │   ├── StockTransaction.cs
│   │   └── RestockRequest.cs
│   ├── ApplicationDbContext.cs
│   └── Migrations/
│
├── SupermarketManagerSystem.Services/      # Business Logic Layer
│   ├── Contracts/
│   │   ├── IProductService.cs
│   │   ├── ICategoryService.cs
│   │   ├── ISupplierService.cs
│   │   ├── IEmployeeService.cs
│   │   ├── IRestockRequestService.cs
│   │   └── IAdminService.cs
│   ├── Implementations/
│   │   ├── ProductService.cs
│   │   ├── CategoryService.cs
│   │   ├── SupplierService.cs
│   │   ├── EmployeeService.cs
│   │   ├── RestockRequestService.cs
│   │   └── AdminService.cs
│   └── Models/                             # Service-layer DTOs
│
├── SupermarketManagerSystem.Web/           # ASP.NET Core MVC Web App
│   ├── Areas/Admin/                        # Admin area with Dashboard
│   ├── Controllers/
│   │   ├── HomeController.cs
│   │   ├── ProductsController.cs
│   │   ├── CategoriesController.cs
│   │   ├── SuppliersController.cs
│   │   ├── EmployeesController.cs
│   │   ├── RestockRequestsController.cs
│   │   ├── ErrorController.cs
│   │   └── Api/ProductsApiController.cs   # Web API (AJAX)
│   ├── ViewModels/
│   ├── Views/
│   └── Program.cs
│
└── SupermarketManagerSystem.Tests/         # NUnit Unit Tests
    └── ProductServiceTests.cs
```

---

## 🧩 Entity Models

| Model | Description |
|---|---|
| `Product` | SKU with price, barcode, stock quantity, category & supplier |
| `Category` | Product grouping (Fruits, Dairy, Bakery...) |
| `Supplier` | Company supplying goods with contact info |
| `Employee` | Staff member with position and hire date |
| `StockTransaction` | Record of stock coming in/going out |
| `RestockRequest` | Request to reorder a low-stock product |
| `ApplicationUser` | Extended Identity user with FirstName/LastName |

---

## 🎮 Controllers

| Controller | Purpose |
|---|---|
| `HomeController` | Landing page and About |
| `ProductsController` | Full CRUD for products |
| `CategoriesController` | CRUD for categories (Admin) |
| `SuppliersController` | View/manage suppliers |
| `EmployeesController` | Employee management (Admin only) |
| `RestockRequestsController` | Submit & approve restock requests |
| `ErrorController` | Custom error pages (400, 401, 404, 500) |
| `Admin/DashboardController` | Admin area dashboard |
| `Api/ProductsApiController` | REST API for AJAX requests |

---

## 📄 Views (10+)

- `Home/Index` — Hero page with live AJAX low-stock widget
- `Home/About` — Project info
- `Products/All` — Searchable, filterable product list
- `Products/Details` — Full product info with restock alert
- `Products/Add` — Add new product form
- `Products/Edit` — Edit existing product
- `Products/Delete` — Deactivate product confirmation
- `Categories/All` — Category card grid
- `Suppliers/All` — Supplier table
- `Employees/All` — Employee table (Admin)
- `RestockRequests/All` — Restock request list with status actions
- `Admin/Dashboard/Index` — Stats cards + low-stock + recent requests
- `Error/NotFound` — 404 page
- `Error/Unauthorized` — 401 page
- `Error/BadRequest` — 400 page
- `Error/InternalServerError` — 500 page

---

## 🔐 Roles & Access

| Role | Access |
|---|---|
| Anonymous | Products list, Categories, Home |
| Logged-in | + Suppliers, Restock Requests, Add/Edit products |
| Administrator | + Employees, Admin Dashboard, Delete/Approve actions |

Default admin credentials (seeded on first run):
- **Email:** `admin@supermarket.bg`
- **Password:** `Admin123!`

---

## 🗄️ Database Setup

### Connection String
Edit `SupermarketManagerSystem.Web/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=DESKTOP-6E804HH\\SQLEXPRESS;Database=SupermarketManagerSystem;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### Apply Migrations
In **Package Manager Console** (with Web project selected as startup):
```powershell
Update-Database
```

Or via CLI:
```bash
dotnet ef database update --project SupermarketManagerSystem.Data --startup-project SupermarketManagerSystem.Web
```

This will:
- Create all tables
- Seed categories (8 default categories)
- Seed suppliers (3 default suppliers)

The admin user and role are seeded automatically at **first application startup**.

---

## ✅ Requirements Coverage

| Requirement | Status |
|---|---|
| 10+ views | ✅ 16 views |
| 4+ entity models | ✅ 7 models |
| 4+ controllers | ✅ 9 controllers |
| 1+ API controller | ✅ ProductsApiController |
| Services layer | ✅ 6 service interfaces + implementations |
| Identity + Roles | ✅ Administrator role + custom user properties |
| EF Core + SQL Server | ✅ |
| Migrations + Seeded data | ✅ |
| Bootstrap responsive UI | ✅ Bootstrap 5 |
| Custom error pages | ✅ 400, 401, 404, 500 |
| TempData messages | ✅ Success/Error alerts |
| AJAX + API | ✅ Low-stock widget on homepage |
| Validation | ✅ Client + server-side DataAnnotations |
| CSRF protection | ✅ AntiForgeryToken on all POST forms |
| DI (Dependency Injection) | ✅ All services registered in Program.cs |
| Unit tests (NUnit + Moq) | ✅ 10 tests for ProductService |
| Admin area | ✅ Areas/Admin/Dashboard |
| README.md | ✅ |

---

## 🔧 What Can Be Extended

- Full CRUD for Suppliers and Employees
- StockTransaction history page
- Product image upload
- Paging and sorting on list pages
- Reporting/export to Excel
- Azure or Replit deployment
