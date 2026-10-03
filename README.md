# Monolith

One application, one process, one database. `OrderService` calls `InventoryService` and `PaymentService` directly. One `SaveChangesAsync` commits the order, the stock change, and the payment together.

```powershell
dotnet run --project ECommerce
```

The API listens on http://localhost:5000. The seeded product is Coffee, price 4.50, stock 20.

```powershell
curl -X POST http://localhost:5000/orders -H "Content-Type: application/json" -d "{\"productId\":\"11111111-1111-1111-1111-111111111111\",\"quantity\":2}"
```
