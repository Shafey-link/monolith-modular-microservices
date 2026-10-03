# Monolith, Modular Monolith, and Microservices

The same e-commerce purchase, implemented three ways. A customer buys a product:

1. Create the order
2. Reserve inventory
3. Process payment
4. Confirm the order

Check out the branch you want to present. Each branch contains only that architecture.

| Branch | What runs | How the steps talk |
| --- | --- | --- |
| `monolith` | One application, one database | In-process method calls |
| `modular` | One application, three modules, one database | In-process calls across module boundaries |
| `microservices` | Three applications, three databases | HTTP calls |

```powershell
git checkout monolith
dotnet run --project ECommerce
```

```powershell
git checkout modular
dotnet run --project ECommerce
```

```powershell
git checkout microservices
./run-all.ps1
```

Every version accepts the same request. The seeded product is Coffee, price 4.50, stock 20.

```json
{ "productId": "11111111-1111-1111-1111-111111111111", "quantity": 2 }
```
