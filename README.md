🏁 Summary

✅ Implemented full CRUD with DDD
✅ Business rules for quantity-based discounts
✅ SQLite integration
✅ Angular frontend with reactive forms
✅ Auto-calculation of totals and discounts
✅ Support for edit and cancel operations

## 🏗️ Architecture

The backend follows **DDD (Domain-Driven Design)** principles:
- **Domain layer:** Core entities (`Sale`, `SaleItem`) and business rules.  
- **Application layer:** Use cases (`CreateSaleUseCase`, `UpdateSaleUseCase`, etc.).  
- **Infrastructure layer:** Repository implementation and data persistence using **Entity Framework Core + SQLite**.  
- **API layer:** Controllers exposing endpoints for full CRUD operations.

## 🧱 Tech Stack

| Layer | Technology |
|-------|-------------|
| **Backend** | .NET 8 (C#), ASP.NET Core Web API |
| **Architecture** | Domain-Driven Design (DDD) |
| **Database** | SQLite (via EF Core) |
| **Frontend** | Angular 17 |
| **UI Library** | HTML5 + CSS + Reactive Forms |
| **Formatting** | CurrencyPipe, DatePipe |
| **Version Control** | Git & GitHub |
| **IDE** | VS Code |

## ⚙️ Project StructureDeveloperStore/
├── backend/
│ ├── DeveloperStore.Api/ # API (Controllers & Startup)
│ ├── DeveloperStore.Application/ # Use cases & DTOs
│ ├── DeveloperStore.Domain/ # Entities, Interfaces & Rules
│ ├── DeveloperStore.Infrastructure/ # Repositories, EF Core + SQLite
│ ├── DeveloperStore.Tests/ # Unit Test, Integration Test
│
├── frontend/
│ └── developerstore-ui/ # Angular project (Forms + CRUD UI)
│
└── README.md

### 🖥️ Backend (.NET API)
    cd backend/DeveloperStore.Api

    dotnet restore
    dotnet run
    
    👉 https://localhost:5132 or http://localhost:5132/swagger

    cd backend/DeveloperStore.Tests

    dotnet test


### 🖥️ Frontend (Angular)
    cd frontend/developerstore-ui

    npm install
    ng serve
    
    👉 http://localhost:4200

📄 Endpoints

| Method   | Endpoint          | Description     |
| -------- | ----------------- | --------------- |
| `GET`    | `/api/sales`      | Get all sales   |
| `GET`    | `/api/sales/{id}` | Get sale by ID  |
| `POST`   | `/api/sales`      | Create new sale |
| `PUT`    | `/api/sales/{id}` | Update sale     |
| `DELETE` | `/api/sales/{id}` | Cancel sale     |

🧩 Entities

Sale

| Property    | Type           | Description                    |
| ----------- | -------------- | ------------------------------ |
| Id          | Guid           | Unique identifier              |
| SaleNumber  | string         | Sale number                    |
| Customer    | string         | Customer name                  |
| Branch      | string         | Branch name                    |
| Date        | DateTime       | Sale date and time             |
| Items       | List<SaleItem> | Sale items                     |
| TotalAmount | decimal        | Total value of the sale        |
| IsCancelled | bool           | Indicates if sale is cancelled |

SaleItem

| Property  | Type    | Description               |
| --------- | ------- | ------------------------- |
| Id        | Guid    | Unique identifier         |
| Product   | string  | Product name              |
| Quantity  | int     | Quantity sold             |
| UnitPrice | decimal | Price per unit            |
| Discount  | decimal | Discount applied          |
| Total     | decimal | Total amount for the item |

🧮 Frontend Features

- Reactive form for creating and editing sales
- Automatic total calculation and currency formatting (BRL)
- Real-time discount calculation
- Sale cancelation
- Inline item editing
- Validation and field highlighting
- Cancel edit button to reset form

🧰 Database

- Database: SQLite
- ORM: Entity Framework Core
- Migration & Seeding handled automatically by DeveloperStoreDbContext
- The database file is generated locally as developerstore.db in the bin directory

🧾 Example JSON (Create Sale)

{
  "saleNumber": "1122",
  "customer": "John Doe",
  "branch": "SP",
  "date": "2025-10-28T13:49:00",
  "items": [
    {
      "product": "Notebook Dell XPS",
      "quantity": 12,
      "unitPrice": 134.5,
      "discount": 0.2,
      "total": 1291.2
    }
  ],
  "totalAmount": 1291.2
}


