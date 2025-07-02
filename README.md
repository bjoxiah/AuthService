# AuthService

AuthService is a .NET 6.0-based application designed to manage user accounts and authentication services. It includes an API, domain models, repository, service layer, and unit tests.

## Features

- User account creation and validation
- Username availability checks
- Account updates
- Global exception handling
- Unit tests for validators, services, repositories, and controllers

## Project Structure

- **AuthService.Domain**: Contains domain models like `Account`.
- **AuthService.Repository**: Handles database interactions using Entity Framework Core.
- **AuthService.Service**: Contains business logic and service implementations.
- **AuthService.API**: Exposes RESTful endpoints for account management.
- **AuthService.Test**: Includes unit tests for various components.

## Requirements

- .NET 6.0 SDK
- SQLite database

## Setup

1. Clone the repository:
   ```sh
   git clone <repository-url>
