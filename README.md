# Modular monolith

One application, one process, one database. Orders, Payments, and Inventory are separate modules. `OrderService` still calls `InventoryService` and `PaymentService` in-process. No HTTP call leaves the application.

```powershell
dotnet run --project ECommerce
```

The API listens on http://localhost:5000. The seeded product is Coffee, price 4.50, stock 20.

```powershell
curl -X POST http://localhost:5000/orders -H "Content-Type: application/json" -d "{\"productId\":\"11111111-1111-1111-1111-111111111111\",\"quantity\":2}"
```

Stock for that product is available at `GET /products/11111111-1111-1111-1111-111111111111`.
