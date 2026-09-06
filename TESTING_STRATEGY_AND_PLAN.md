# Comprehensive Testing Strategy & Implementation Plan

**Solution:** `CIOT_ModularHub` (DDD Modular Monolith)  
**Target Framework:** .NET 10  
**Test Framework:** TUnit + TUnit.Aspire + ArchUnitNET + CodeCoverage  
**Target Solution:** `c:\Users\E0108166\Downloads\DDD Muslim\IOTHub_DDD`  
**Branch:** `feature/tests-architecture-unit-integration`

---

## 1. Executive Summary

This plan outlines the design and step-by-step implementation of an enterprise-grade testing strategy for the **CIOT Modular IoT Hub** backend. The solution follows a **DDD Light / Modular Monolith** architecture consisting of **14 Bounded Contexts**, an Aspire AppHost orchestrator, and an ASP.NET Core Web API host.

To mirror the gold-standard architecture from the reference project while honoring the 14 bounded context modules in `IOTHub_DDD`, we are creating three dedicated test projects:

```
tests/
├── Directory.Build.props
├── coverage.api.logic.runsettings
├── Architecture/
│   └── CIOT.Architecture.Tests/
├── Unit/
│   └── CIOT.Unit.Tests/
└── Integration/
    └── CIOT.Integration.Tests/
```

---

## 2. Test Suites Overview

| Test Project | Purpose & Scope | Key Frameworks & Libraries |
| :--- | :--- | :--- |
| **`CIOT.Architecture.Tests`** | Enforces DDD layer boundaries, module isolation across all 14 contexts, CQRS naming & design rules, validation conventions, and DbContext separation. | `ArchUnitNET`, `ArchUnitNET.TUnit`, `TUnit` |
| **`CIOT.Unit.Tests`** | Tests domain logic, entities, value objects, MediatR command/query handlers, validation rules, and error flows without I/O or external services. | `TUnit`, `TUnit.Assertions`, `TUnit.Mocks`, `EFCore.InMemory` |
| **`CIOT.Integration.Tests`** | Executes end-to-end API workflows against the Aspire AppHost containerized environment (PostgreSQL / TimescaleDB) for all 14 bounded context endpoints. | `TUnit.Aspire`, `TUnit.Assertions`, `Aspire.Hosting.Testing` |

---

## 3. The 14 Bounded Contexts in Scope

Each bounded context consists of 4 projects (`Domain`, `Application`, `Infrastructure`, `Endpoints`) and will be covered across Architecture, Unit, and Integration testing:

1. **Admin** (`CIOT.Modules.Admin`) — User administration, system settings, RBAC.
2. **Asset** (`CIOT.Modules.Asset`) — Asset management, firmware assignments, device grouping.
3. **Audit** (`CIOT.Modules.Audit`) — Security audit logs, activity tracking, event auditing.
4. **Catalog** (`CIOT.Modules.Catalog`) — Device types, hardware specs, firmware catalog.
5. **CustomerOutlet** (`CIOT.Modules.CustomerOutlet`) — Customer outlets, customer clusters, hierarchy.
6. **Devices** (`CIOT.Modules.Devices`) — Device registrations, status, heartbeats, connectivity.
7. **Identity** (`CIOT.Modules.Identity`) — Authentication, JWT token handling, credentials, roles.
8. **Integration** (`CIOT.Modules.Integration`) — External connectors, ERP/CRM webhooks, sync pipelines.
9. **LocalAdapter** (`CIOT.Modules.LocalAdapter`) — Edge gateway adapter protocols, message translation.
10. **Mobile** (`CIOT.Modules.Mobile`) — Mobile companion API, push notification triggers.
11. **Org** (`CIOT.Modules.Org`) — Master organization structure (Countries, Business Units, Sales Orgs, Sales Territories).
12. **Provisioning** (`CIOT.Modules.Provisioning`) — Zero-touch device onboarding, provisioning workflows.
13. **Report** (`CIOT.Modules.Report`) — Operational dashboards, SLA tracking, aggregation queries.
14. **Telemetry** (`CIOT.Modules.Telemetry`) — Time-series sensor intake, metrics aggregation, TimescaleDB queries.

---

## 4. Detailed Test Architecture

### 4.1. Architecture Tests (`CIOT.Architecture.Tests`)

- **Layer Dependency Rules:**
  - `Domain` layer must **not** depend on `Application`, `Infrastructure`, `Endpoints`, or `Host`.
  - `Application` layer must **not** depend on `Infrastructure`, `Endpoints`, or `Host`.
  - `Infrastructure` and `Endpoints` must only depend on `Application`, `Domain`, and `Common`.
- **Bounded Context Isolation:**
  - Modules must not directly reference or access other modules' internal `Infrastructure` or `Domain` entities directly. Communication between modules is mediated via CQRS commands/queries or contracts in `Common`.
- **CQRS & Pattern Conventions:**
  - All Commands must implement `ICommand<T>` / `ICommand`.
  - All Queries must implement `IQuery<T>`.
  - All Handlers must implement `IRequestHandler<TRequest, TResponse>`.
  - All Validators must inherit from `FluentValidation.AbstractValidator<T>` and be named `*Validator`.
- **Persistence & Entity Conventions:**
  - Each Module has its own dedicated `DbContext` located in its `Infrastructure` project.
  - Entities must encapsulate setters or expose domain methods.

---

### 4.2. Unit Tests (`CIOT.Unit.Tests`)

- **Domain Model Tests:**
  - Invariant validation, entity creation, business methods, domain event generation.
- **Application Handler Tests (CQRS):**
  - Command Handlers: Happy paths, domain errors, conflict handling, and failure results.
  - Query Handlers: Filter application, pagination, sorting, not-found behavior.
  - FluentValidation rule verification for valid/invalid requests.
- **Test Infrastructure & Shared Helpers:**
  - `TestDbContextFactory`: Factory to spin up isolated in-memory or SQLite in-process DbContexts for fast parallel execution.
  - `TestTimeProvider`: Deterministic clock mocking for time-sensitive tests.
  - MediatR mock pipeline behaviors for isolation.

---

### 4.3. Integration Tests (`CIOT.Integration.Tests`)

- **Aspire Test Fixture (`ApiAppFixture`):**
  - Inherits from `AspireFixture<Projects.CIOT_AppHost>`.
  - Boots the Aspire AppHost container environment (PostgreSQL / TimescaleDB container).
  - Configures `HttpClient` targeting the API service.
- **API Endpoint Tests (per Bounded Context):**
  - HTTP status codes, headers, and payload verification.
  - CRUD operations persistence verification in the real database.
  - Error responses formatted according to standard Result / ProblemDetails schema.
- **Platform & Health Contract Tests:**
  - `/health` and `/alive` Aspire ServiceDefaults health endpoints.
  - OpenAPI endpoint `/openapi/v1.json` schema validation.

---

## 5. Step-by-Step Execution Roadmap

```
[Phase 1: Tooling & Central Package Management]
       │
       ▼
[Phase 2: Architecture Test Project & Rules]
       │
       ▼
[Phase 3: Unit Test Project & Bounded Context Suites]
       │
       ▼
[Phase 4: Aspire Integration Test Project & E2E API Tests]
       │
       ▼
[Phase 5: Solution Verification & Test Run Execution]
```

### Phase 1: Project Setup & Central Package Management
1. Update [Directory.Packages.props](Directory.Packages.props) to add test dependencies:
   - `TUnit`, `TUnit.Assertions`, `TUnit.Core`, `TUnit.Engine`, `TUnit.Aspire`, `TUnit.Mocks` (`1.65.63`)
   - `TngTech.ArchUnitNET`, `TngTech.ArchUnitNET.TUnit` (`0.13.4`)
   - `Microsoft.Testing.Extensions.CodeCoverage` (`18.10.0`)
   - `Microsoft.EntityFrameworkCore.InMemory` (`10.0.11`)
2. Create `tests/Directory.Build.props` to configure test projects (test runner, coverage, warnings).
3. Create `tests/coverage.api.logic.runsettings` for code coverage profiling.
4. Update [CIOT_ModularHub.slnx](CIOT_ModularHub.slnx) to register the new test projects under `/tests/`.

### Phase 2: Implement `CIOT.Architecture.Tests`
1. Create `CIOT.Architecture.Tests.csproj` referencing the API host and all module projects.
2. Add `ArchitectureTestHelpers.cs` to load all module assemblies into the ArchUnit model.
3. Implement test suites:
   - `LayerDependencyArchitectureTests.cs`: Clean architecture boundary rules.
   - `BoundedContextIsolationTests.cs`: Cross-module reference boundaries.
   - `CqrsArchitectureTests.cs`: Command/Query/Handler conventions.
   - `ValidatorArchitectureTests.cs`: FluentValidation naming and inheritance rules.
   - `DbContextArchitectureTests.cs`: Module DbContext isolation and configuration.

### Phase 3: Implement `CIOT.Unit.Tests`
1. Create `CIOT.Unit.Tests.csproj`.
2. Create test infrastructure (`TestDbContextFactory`, `TestTimeProvider`, `ResultAssertionsHelper`).
3. Implement unit test suites organized by bounded context:
   - `Org/`: Countries, BusinessUnits, SalesOrganizations, SalesTerritories handlers & validators.
   - `Devices/`: Device registration, heartbeat processing, queries.
   - `Asset/`: Firmware assignment, asset lifecycle.
   - `CustomerOutlet/`: Customer clusters, outlet queries and commands.
   - `Identity/`: Token generation, credential checks.
   - `Telemetry/`: Telemetry intake, time-series validation.
   - `Provisioning/`: Device onboarding states.
   - `Catalog/`, `Admin/`, `Audit/`, `Integration/`, `LocalAdapter/`, `Mobile/`, `Report/`.

### Phase 4: Implement `CIOT.Integration.Tests`
1. Create `CIOT.Integration.Tests.csproj` referencing `CIOT.AppHost` and `CIOT.Api`.
2. Implement `ApiAppFixture.cs` using `TUnit.Aspire` to orchestrate test instances of the AppHost and database.
3. Implement integration test suites organized by bounded context:
   - `Infrastructure/ApiPlatformContractTests.cs` (Health, alive, OpenAPI)
   - `Org/` (E2E HTTP API tests for `/api/org/...`)
   - `Devices/` (E2E HTTP API tests for `/api/devices/...`)
   - `CustomerOutlet/` (E2E HTTP API tests for `/api/customer-outlet/...`)
   - `Asset/`, `Catalog/`, `Identity/`, `Provisioning/`, `Telemetry/`, etc.

### Phase 5: Verification & Quality Gates
1. Run full build `dotnet build CIOT_ModularHub.slnx`.
2. Execute all architecture tests with `dotnet test tests/Architecture/CIOT.Architecture.Tests`.
3. Execute all unit tests with `dotnet test tests/Unit/CIOT.Unit.Tests`.
4. Execute integration tests with `dotnet test tests/Integration/CIOT.Integration.Tests`.
5. Verify code coverage and commit changes to git branch `feature/tests-architecture-unit-integration`.
