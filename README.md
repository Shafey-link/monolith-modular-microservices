# Microservices

Three applications, three processes, three databases. The order service does not call payment or inventory code. It calls them over HTTP.

| Service | URL | Database |
| --- | --- | --- |
| Inventory | http://localhost:5001 | `inventory.db` |
| Payments | http://localhost:5002 | `payments.db` |
| Orders | http://localhost:5003 | `orders.db` |

```powershell
./run-all.ps1
```

The seeded product is Coffee, price 4.50, stock 20. It lives only in the inventory service.

```powershell
curl -X POST http://localhost:5003/orders -H "Content-Type: application/json" -d "{\"productId\":\"11111111-1111-1111-1111-111111111111\",\"quantity\":2}"
```

If inventory or payments cannot be reached, the order call fails. That network boundary is the difference from the monolith and modular branches.
