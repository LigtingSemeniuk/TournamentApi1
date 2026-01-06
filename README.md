# TournamentApi – GraphQL Tournament Management API

## Project description
TournamentApi is a backend API based on GraphQL, implemented using ASP.NET Core and the HotChocolate library.
The application is a simple tool for managing tournaments in a knockout (cup) system.

The project fulfills the requirements of the assignment:
- user registration and login,
- JWT-based authentication,
- tournament, participant, bracket, and match management,
- retrieving information about the logged-in user's matches without providing userId in queries.

## Domain model
The application is based on the UML class diagram provided in the assignment and includes the following entities:
- User
- Tournament
- Bracket
- Match

Relationships:
- Users participate in tournaments
- Each tournament has one bracket
- A bracket consists of multiple matches
- Each match has two players and an optional winner

## Authentication and authorization
Authentication is implemented using JSON Web Tokens (JWT).
- The user identifier is stored in the JWT claim "sub"
- userId is never passed directly in GraphQL queries or mutations
- User identity is resolved exclusively from the JWT token
- Protected operations use HotChocolate authorization mechanisms

## Technology stack
- ASP.NET Core
- GraphQL (HotChocolate)
- Entity Framework Core
- SQLite
- JWT Authentication

## Project structure
TournamentApi
├── Data
│   └── AppDbContext.cs
├── Domain
│   ├── User.cs
│   ├── Tournament.cs
│   ├── Bracket.cs
│   └── Match.cs
├── GraphQL
│   ├── Query.cs
│   ├── Mutation.cs
│   ├── Inputs
│   └── Types
├── Security
│   ├── JwtOptions.cs
│   ├── JwtTokenService.cs
│   └── PasswordHasher.cs
├── Services
│   ├── AuthService.cs
│   ├── TournamentService.cs
│   └── MatchService.cs
├── Program.cs
└── appsettings.json

## Running the project

### Requirements
- .NET 8 SDK
- Visual Studio or Rider
- SQLite

### Database initialization
Before running the application, apply Entity Framework Core migrations.

Using Package Manager Console:
```
Add-Migration InitialCreate
Update-Database
```

### Running the application
Run the project using Visual Studio (F5).  
After startup, the GraphQL endpoint will be available at:
```
https://localhost:7039/graphql
```

The Banana Cake Pop GraphQL IDE is available at this address.

## Usage instructions

### User registration
```
mutation {
  register(input:{
    firstName:"Ihor",
    lastName:"S",
    email:"ihor@test.com",
    password:"Pass123!"
  }){
    userId
    token
  }
}
```

### User login
```
mutation {
  login(input:{
    email:"ihor@test.com",
    password:"Pass123!"
  }){
    userId
    token
  }
}
```

### Authorization header
For authenticated operations, add the following HTTP header in the GraphQL client:
```
Authorization: Bearer <JWT_TOKEN>
```

### Creating a tournament
```
mutation {
  createTournament(input:{
    name:"Cup 1",
    startDate:"2026-01-06T00:00:00Z"
  }){
    id
    name
    status
  }
}
```

### Adding the logged-in user as a participant
```
mutation {
  addParticipant(input:{ tournamentId: 1 }){
    id
    participants { id email }
  }
}
```

### Starting a tournament
```
mutation {
  start(input:{ tournamentId: 1 }){
    id
    status
    bracket {
      matches {
        id
        round
        player1 { email }
        player2 { email }
        winner { email }
      }
    }
  }
}
```

### Retrieving matches of the logged-in user
```
query {
  myMatches {
    id
    round
    player1 { email }
    player2 { email }
    winner { email }
  }
}
```

## Assignment compliance
- GraphQL API implemented using HotChocolate
- JWT-based authentication
- No userId passed in user-level queries
- Operations aligned with the provided UML class diagram
- Tournament management in a knockout system

## Author
This project was developed as part of an advanced backend programming course assignment.
