# BookMyShow Web API

## Overview
The **BookMyShow Web API** is a .NET-based RESTful API designed to manage movie ticket bookings. This API provides functionalities for users, movies, theaters, seats, and bookings, enabling seamless ticket reservation.

## Features
- User authentication and authorization
- Movie listings and showtimes
- Theater and seat management
- Booking and payment processing
- Admin controls for managing content

## Technologies Used
- **Framework:** .NET Core Web API
- **Database:** SQL Server with Entity Framework Core
- **Authentication:** JWT
- **Tools:** Swagger for API documentation

## Project Structure
```
BookMyShow-Web-API/
│── BookMyShow/
│   ├── BookMyShow.csproj            # Project file
│   ├── Program.cs                   # Main entry point
│   ├── appsettings.json             # Configuration settings
│   ├── Controllers/                 # API Controllers
│   ├── Data/                        # Database context and configurations
│   │   ├── Entities/                # Database entity definitions
│   │   ├── BookMyShowDbContext.cs   # Database context
│   │   ├── DataExtensions.cs        # Data extension methods
│   ├── Exceptions/                  # Custom exception handling
│   │   ├── CustomExceptions.cs      # Exception definitions
│   ├── Migrations/                  # Entity Framework migrations
│   ├── Models/                      # Database models and DTOs
│   │   ├── CommonDTOs/              # Common Data Transfer Objects
│   │   ├── MovieDTOs/               # DTOs for Movies
│   │   ├── ReviewDTOs/              # DTOs for Reviews
│   │   ├── TheatreDTOs/             # DTOs for Theatres
│   │   ├── TicketDTOs/              # DTOs for Tickets
│   │   ├── UserDTOs/                # DTOs for Users
│   ├── Repository/                  # Repository pattern implementations
│   │   ├── Implementations/         # Concrete repository implementations
│   │   ├── IRepository/             # Interface definitions for repositories
│   ├── BookMyShow.http              # HTTP request files for testing
```

## Installation

1. **Clone the repository**
   ```sh
   git clone https://github.com/your-username/BookMyShow-Web-API.git
   ```

2. **Navigate to the project directory**
   ```sh
   cd BookMyShow-Web-API
   ```

3. **Restore dependencies**
   ```sh
   dotnet restore
   ```

4. **Update database**
   ```sh
   dotnet ef database update
   ```

5. **Run the API**
   ```sh
   dotnet run
   ```

## API Endpoints

### Authentication
| Method | Endpoint                       | Description                     |
|--------|--------------------------------|---------------------------------|
| POST   | /api/auth/signup               | Register a user                 |
| POST   | /api/auth/login                | User authentication             |

### Movies
| Method | Endpoint                       | Description                     |
|--------|--------------------------------|---------------------------------|
| GET    | /api/movies                    | Get movie listings              |
| GET    | /api/movies/{id}               | Get movie details by ID         |
| POST   | /api/movies                    | Add a new movie (admin)         |
| PUT    | /api/movies/{id}               | Update movie details (admin)    |
| DELETE | /api/movies/{id}               | Delete a movie (admin)          |

### Reviews
| Method | Endpoint                       | Description                     |
|--------|--------------------------------|---------------------------------|
| POST   | /api/reviews                   | Add a movie review              |
| GET    | /api/reviews/{movieId}         | Get reviews for a movie         |

### Theatres
| Method | Endpoint                       | Description                     |
|--------|--------------------------------|---------------------------------|
| GET    | /api/theatres                  | Get list of theaters            |
| GET    | /api/theatres/{id}             | Get theater details by ID       |
| POST   | /api/theatres                  | Add a new theater (admin)       |
| PUT    | /api/theatres/{id}             | Update theater details (admin)  |
| DELETE | /api/theatres/{id}             | Delete a theater (admin)        |

### Theatre Owners
| Method | Endpoint                       | Description                     |
|--------|--------------------------------|---------------------------------|
| POST   | /api/theatreowners             | Register a theater owner        |
| GET    | /api/theatreowners/{id}        | Get theater owner details       |

### Tickets
| Method | Endpoint                       | Description                     |
|--------|--------------------------------|---------------------------------|
| GET    | /api/tickets                   | Get all ticket bookings         |
| GET    | /api/tickets/{id}              | Get ticket details by ID        |
| POST   | /api/tickets                   | Book a ticket                   |
| DELETE | /api/tickets/{id}              | Cancel a ticket                 |

### Users
| Method | Endpoint                       | Description                     |
|--------|--------------------------------|---------------------------------|
| GET    | /api/users/{id}                | Get user profile by ID          |
| PUT    | /api/users/{id}                | Update user profile             |
| DELETE | /api/users/{id}                | Delete user account             |

## Configuration
Ensure the database connection string and JWT secret are correctly configured in `appsettings.json`.

## Contribution
Contributions are welcome! Feel free to fork the repository and submit pull requests.

## License
This project is open-source and available under the [MIT License](LICENSE).

