# Mediator + Hangfire Example

This project demonstrates how to integrate **[MediatR](https://github.com/jbogard/MediatR)** (for CQRS / in-process messaging) with **[Hangfire](https://www.hangfire.io/)** (for background jobs) in an ASP.NET Core application.

It shows how to:
- ✅ Schedule MediatR commands to run **immediately**, **later**, or **recurring** with Hangfire.
- ✅ Serialize and deserialize MediatR requests safely using `Newtonsoft.Json`.
- ✅ Execute commands via a custom `ICommandExecutor` that dispatches to MediatR.
- ✅ Monitor and manage jobs via the **Hangfire Dashboard UI**.
- ✅ Run a background worker that generates random customer IDs and triggers long-running discount recalculation jobs.

---

## ⚡ Advantages

- **Decoupled architecture**  
  Commands are defined and handled via MediatR; Hangfire only schedules them.

- **Reliable execution**  
  Jobs are retried automatically if they fail (configurable retry policy).

- **Visibility**  
  The Hangfire Dashboard shows job history, retries, failures, and statistics.

- **Scalability**  
  Background job processing can be distributed across multiple servers.

- **Simulation of real scenarios**  
  Includes a sample `RecalculateCustomerDiscountCommand` that simulates a long-running database operation.

---

![img.png](img.png)