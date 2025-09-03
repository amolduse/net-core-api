# Multi-Tenant .NET API with Clean Architecture

This project is a sample .NET API that demonstrates a multi-tenant architecture using a header-based tenant resolution strategy. It is built with a clean architecture approach and uses Dapper for data access, Honeycomb for observability, and AWS Secrets Manager for credential management.

## Project Overview

The solution is divided into four projects, following the principles of Clean Architecture:

-   `MultiTenantApi.Domain`: Contains the business entities.
-   `MultiTenantApi.Application`: Contains the application logic, interfaces, DTOs, and services.
-   `MultiTenantApi.Infrastructure`: Contains the implementations for data access (repositories), external services (AWS, Honeycomb), and other infrastructure concerns.
-   `MultiTenantApi.Api`: The main Web API project, which contains the controllers, middleware, and startup configuration.

### Key Features

-   **Multi-Tenancy**: The API resolves the tenant based on the `X-Tenant-Id` header.
-   **Clean Architecture**: The code is organized into layers for better separation of concerns and maintainability.
-   **Dapper**: Uses Dapper for high-performance data access.
-   **Honeycomb**: Integrated with Honeycomb for rich, structured logging and tracing.
-   **AWS Secrets Manager**: Fetches database credentials securely from AWS Secrets Manager.
-   **Shared and Tenant-Specific Databases**: Demonstrates how to query and combine data from both a shared database and tenant-specific databases.

## Prerequisites

-   .NET 8 SDK (or later)
-   Access to an AWS account with permissions to use Secrets Manager.
-   SQL Server instance(s) for the databases.

## Configuration

Before running the application, you need to configure the following:

### 1. `appsettings.json`

Open `MultiTenantApi.Api/appsettings.json` and fill in the placeholder values:

```json
{
  "Honeycomb": {
    "ApiKey": "YOUR_HONEYCOMB_API_KEY",
    "ServiceName": "MultiTenantApi"
  },
  "AWS": {
    "Region": "your-aws-region",
    "CredentialsSecretName": "your-aws-secret-name"
  },
  "ConnectionStrings": {
    "ConfigDb": "your-config-db-connection-string"
  },
  "Database": {
    "TenantDbServer": "your-tenant-db-server",
    "SharedDbServer": "your-shared-db-server"
  }
}
```

### 2. AWS Secrets Manager

Create a secret in AWS Secrets Manager with the name you specified in `appsettings.json`. The secret should be a JSON object with the following structure:

```json
{
  "Username": "your-db-username",
  "Password": "your-db-password"
}
```

### 3. Databases

You need to set up the following databases:

-   **CONFIGDB**: This database stores the mapping between tenant IDs and database names.
    -   Create a table named `Tenants`:
        ```sql
        CREATE TABLE Tenants (
            TenantId NVARCHAR(50) PRIMARY KEY,
            DatabaseName NVARCHAR(100) NOT NULL
        );
        ```
    -   Insert some sample data:
        ```sql
        INSERT INTO Tenants (TenantId, DatabaseName) VALUES ('tenant1', 'Tenant_1_DB');
        INSERT INTO Tenants (TenantId, DatabaseName) VALUES ('tenant2', 'Tenant_2_DB');
        ```

-   **SharedDB**: This database contains data that is shared across all tenants.
    -   Create a table named `Categories`:
        ```sql
        CREATE TABLE Categories (
            Id INT PRIMARY KEY,
            Name NVARCHAR(100) NOT NULL
        );
        ```
    -   Insert some sample data:
        ```sql
        INSERT INTO Categories (Id, Name) VALUES (1, 'Electronics'), (2, 'Books');
        ```

-   **Tenant Databases**: These are the databases for each tenant.
    -   For each tenant, create a database with the name you specified in the `Tenants` table of `CONFIGDB`.
    -   In each tenant database, create a table named `Products`:
        ```sql
        CREATE TABLE Products (
            Id INT PRIMARY KEY,
            Name NVARCHAR(100) NOT NULL,
            CategoryId INT
        );
        ```
    -   Insert some sample data into each tenant database. For example, in `Tenant_1_DB`:
        ```sql
        INSERT INTO Products (Id, Name, CategoryId) VALUES (1, 'Laptop', 1), (2, 'C# in Depth', 2);
        ```

## How to Run

1.  Navigate to the root directory of the solution in your terminal.
2.  Run the API:
    ```bash
    dotnet run --project MultiTenantApi.Api
    ```
3.  The API will be available at `https://localhost:port` or `http://localhost:port`.

## How to Test

1.  Open your browser and navigate to the Swagger UI at `https://localhost:port/swagger`.
2.  You will see the `/Products` endpoint.
3.  Click the "Authorize" button at the top right of the page.
4.  In the "Value" field for the `X-Tenant-Id` header, enter a tenant ID (e.g., `tenant1`).
5.  Click "Authorize" and then "Close".
6.  Expand the `/Products` endpoint and click "Try it out".
7.  Click "Execute".

You should see a JSON response containing the products for the specified tenant, with the category names joined from the shared database. You can try different tenant IDs to see the data change.

## Running the Tests

To run the unit tests, navigate to the root directory of the solution and run the following command:

```bash
dotnet test
```

## Renaming the Project

This project can be used as a boilerplate. To rename the project and all its namespaces, you can use the `rename-project.sh` script.

1.  Open a terminal in the root directory of the project.
2.  Run the script:
    ```bash
    ./rename-project.sh
    ```
3.  When prompted, enter the new name for your project (e.g., `MyAwesomeApi`).
4.  The script will handle renaming the solution, projects, directories, and namespaces.
5.  After the script finishes, it's a good idea to do a manual check and then rebuild the solution.
