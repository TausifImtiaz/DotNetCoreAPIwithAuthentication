# DotNetCoreAPI with Authentication

## Overview

The **DotNetCoreAPI with Authentication** project is a .NET Core 3.1 API that demonstrates how to implement JWT Bearer token authentication and manage Master-Details CRUD operations. This project provides a secure and scalable API solution for handling data with robust authentication and authorization mechanisms.

## Features

- **JWT Bearer Token Authentication:** Secures the API using JSON Web Tokens (JWT) for authentication and authorization.
- **Master-Details CRUD Operations:** Implements Create, Read, Update, and Delete operations with a Master-Details approach for managing related data.
- **.NET Core 3.1 Framework:** Utilizes the .NET Core framework for building high-performance and cross-platform web APIs.

## Technologies Used

- .NET Core 3.1
- JWT Bearer Token Authentication
- ASP.NET Core
- Entity Framework Core
- SQL Server or other supported databases
- Swagger for API documentation

## Project Structure

- **Controllers:** API endpoints for handling HTTP requests and performing CRUD operations.
- **Models:** Defines the data structures used in the application.
- **Data Access Layer:** Includes Entity Framework Core context and repository patterns for data operations.
- **Authentication:** Configures JWT authentication and authorization.
- **Migrations:** Entity Framework Core migrations for database schema management.

## Getting Started

### Prerequisites

- .NET Core SDK 3.1
- SQL Server or another database supported by Entity Framework Core
- Visual Studio 2019 or later (or any other IDE of your choice)

### Cloning the Repository

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/TausifImtiaz/DotNetCoreAPIwithAuthentication.git
   ```

### Installation

1. **Open the Solution:**
   Open the `.sln` file in Visual Studio or your preferred IDE.

2. **Restore NuGet Packages:**
   Restore all required NuGet packages by building the solution or using the NuGet Package Manager.

3. **Configure the Database:**
   - Open `appsettings.json` and update the connection string to point to your database instance.
   - Apply Entity Framework Core migrations to create the database schema:
     ```bash
     dotnet ef migrations add InitialCreate
     dotnet ef database update
     ```

4. **Configure JWT Authentication:**
   - Open `Startup.cs` and ensure JWT authentication is properly configured with your secret key.

5. **Run the Application:**
   Press `F5` or use the command line to run the application:
   ```bash
   dotnet run
   ```

### Testing the API

1. **Access Swagger Documentation:**
   - Once the application is running, navigate to `http://localhost:5000/swagger` (or the URL specified in `launchSettings.json`) to view and test the API endpoints interactively.

2. **Authenticate Requests:**
   - Use a tool like Postman or Swagger UI to authenticate and test the API. Include the JWT token in the `Authorization` header as `Bearer [YourToken]`.

## Usage

1. **CRUD Operations:**
   - **Create:** POST requests to endpoints for adding new Master or Detail records.
   - **Read:** GET requests to retrieve Master and Detail records.
   - **Update:** PUT requests to modify existing Master or Detail records.
   - **Delete:** DELETE requests to remove Master or Detail records.

2. **Authentication:**
   - Obtain a JWT token by logging in through the provided authentication endpoints.
   - Include the token in the `Authorization` header for accessing protected endpoints.

## Contributing

Contributions are welcome! To contribute:
- Fork the repository.
- Create a feature branch.
- Commit your changes.
- Push to the branch.
- Open a pull request with a description of your changes.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgements

- .NET Core documentation
- JWT authentication resources
- Entity Framework Core resources

## Contact

For any questions or support, please contact [Tausif Imtiaz](mailto:tausifimtiaz@gmail.com).
