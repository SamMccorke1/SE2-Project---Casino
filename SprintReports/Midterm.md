# 🎰 Chuds2Chads Casino  
Software Engineering II Group Project

## Team Members
- Samuel McCorkel  
- Matthew Williams  
- Lucas Litzenberger  
- Caleb Blevins  
- Reo Day  

---

# 1. Product Vision

Chuds2Chads Casino is a web-based casino platform designed to provide users with an engaging multiplayer gaming experience using virtual currency. The platform allows users to create accounts, log in securely, participate in casino games, earn and spend in-game currency, and customize their avatars through unlockable cosmetic items. The long-term vision is to create an expandable system that supports multiple casino games, persistent player progression, and social/multiplayer interaction in a clean and scalable environment.

---

# 2. Project Goals

The major goals of this project are:

- Build a functional casino-style web application using Blazor Server
- Support secure account creation and login with ASP.NET Core Identity
- Store persistent player data using SQLite and Entity Framework Core
- Allow players to participate in casino games using virtual currency
- Support multiplayer-ready game room architecture
- Track player balances and transactions over time
- Support future expansion for features such as avatar customization and loot crates
- Follow Scrum methodology across multiple sprints
- Maintain organized backlog management, version control, testing, and documentation throughout development

---

# 3. Release Plan

## Planned Release Progression
### Release 1
- Project shell and framework setup
- Core Blazor application structure
- Initial architecture and repository setup

### Release 2
- User authentication and login system
- Database integration with ASP.NET Core Identity
- SQLite configuration and migrations

### Release 3
- Wallet and virtual currency tracking
- Core database tables for users, transactions, rooms, and sessions
- Dashboard/account support

### Release 4
- Initial gameplay structure
- Multiplayer-ready room/session design
- Expansion planning for cosmetic and loot crate features

### Future Planned Features
- Blackjack gameplay
- Poker gameplay
- Roulette
- Slots
- Horse racing
- Avatar customization
- Cosmetic unlockables / loot crates
- Leaderboards

---

# 4. Sprint Reports

## Sprint 1
### Focus
- Project planning
- Initial architecture
- Technology stack selection
- Repository and framework setup

### Work Completed
- Established project framework
- Created initial product backlog
- Defined technical direction using Blazor Server and SQLite
- Began planning system architecture

### Challenges
- Scope needed to be reduced and organized
- Initial effort went into translating a large game idea into a realistic sprint-based plan

### Outcome
- Team established the project direction and technical foundation

---

## Sprint 2
### Focus
- Database setup
- Identity and account system planning
- Schema design

### Work Completed
- Began implementing the SQLite database
- Added Entity Framework Core support
- Added ASP.NET Core Identity support
- Designed tables for users, wallets, transactions, rooms, and sessions
- Worked on login/database integration

### Challenges
- Matching the correct package versions to the project target framework
- Ensuring database structure could support both current and future features

### Outcome
- Functional database migration process established
- User/account architecture connected to database design

---

## Sprint 3
### Focus
- Login system integration
- Wallet creation and persistence
- Dashboard/account-related functionality

### Work Completed
- Implemented and tested login functionality
- Connected authentication flow to database-backed user accounts
- Added wallet creation logic for persistent virtual currency
- Verified migrations and account storage in SQLite
- Tested schema stability after feature additions

### Challenges
- Making authentication, Identity, and database schema work together cleanly
- Ensuring account-related changes did not break the rest of the app

### Outcome
- Users can be persisted in the database
- Wallet/account architecture is in place for future casino features

---

## Sprint 4
### Focus
- Preparing the application for gameplay expansion
- Ensuring the database can support future features

### Work Completed
- Continued maintaining and refining the schema
- Tested functionality after adding feature support
- Planned special features such as loot crates and avatar customization
- Supported account/login structure while expanding database design
- Helped keep the database stable as new systems were introduced

### Challenges
- Designing schema for both current features and future special systems
- Balancing immediate functionality with long-term extensibility

### Outcome
- Database and authentication systems are stable enough to support continued development of casino features

---

# 5. Trello / Planner Board

An exported copy of the Trello/Planner board should be attached in the GitHub repository and submitted separately as required. The board includes:

- Product backlog items
- Sprint backlog items
- Item status
- Sprint assignment
- Completed and incomplete tasks
- Priority and ownership

> Attach exported board files/screenshots in a `/docs` or `/planning` folder in the repository.

---

# 6. Source Code

All application source code is maintained in this repository. The codebase includes:

- Blazor Server UI components
- ASP.NET Core Identity authentication
- EF Core database context and models
- SQLite migrations
- Account/login/dashboard functionality
- Room/game/session data models

---

# 7. Coding Standards

The team followed these coding standards:

- Consistent C# naming conventions
- PascalCase for classes, methods, and public properties
- Clear separation of concerns between UI, data, and authentication logic
- Consistent file and folder naming
- Entity models placed in logical data/entity folders
- Readable indentation and formatting
- Avoidance of unnecessary duplicate logic

---

# 8. Documentation Standards

The team maintained documentation by:

- Keeping README information updated
- Documenting setup requirements for teammates
- Tracking project activities through sprint reports
- Maintaining backlog artifacts and board exports
- Using clear file/folder structure for project organization

---

# 9. Development Environment (Tech Stack)

- .NET 10.0
- Blazor Server (Interactive Server Components)
- ASP.NET Core Identity
- Entity Framework Core 10
- SQLite
- Visual Studio / VS Code
- GitHub

---

# 10. Deployment Environment

The project is currently run in a local development environment.

## Current Deployment Context
- Localhost via `dotnet run`
- SQLite file-based local database
- Browser-based web interface

## Future Deployment Options
- Azure App Service
- Render
- Railway
- Other .NET-compatible hosting services

---

# 11. Version Management

Version management is handled using Git and GitHub.

## Process
- Source code stored in GitHub repository
- Team members pull latest updates and push changes regularly
- Commits are used to track development progress
- Files such as build artifacts and local database files should be excluded through `.gitignore`

---

# 12. Test Plan, Tests Performed, and Analysis

## Test Plan
Testing focused on validating:
- Login/account functionality
- Database migrations
- Database stability after schema changes
- Wallet creation and persistence
- Compatibility of new features with existing schema

## Tests Performed
- Build verification tests
- Migration tests
- Authentication/login tests
- Database persistence tests
- Schema compatibility tests
- Functional tests on wallet/account behavior

## Test Types
- Functional testing
- Manual integration testing
- Build verification
- Database validation testing

## Analysis
Testing confirmed:
- The database can be created and migrated successfully
- User accounts can be stored in SQLite
- Authentication and database integration work together
- Wallet logic can persist balances across sessions
- The database remains functional after feature planning/expansion

---

# 13. Test Automation

At this stage, testing has primarily been manual. Automated testing may be added in future iterations.

---

# 14. Change Management / Bug Tracking Process

The team tracks changes and bugs through:
- GitHub commits
- Trello/Planner backlog items
- Sprint discussions
- Bug fixes identified during testing and development

## Process
1. Identify issue or defect
2. Add item to backlog/board
3. Prioritize based on sprint goals
4. Assign ownership
5. Fix and retest
6. Mark as complete when verified

---

# 15. Definition of Ready

A backlog item is considered **Ready** when:
- It is clearly described
- It has business value
- It has acceptance criteria or a clear outcome
- Dependencies are identified
- The team understands the work well enough to start it
- It is small enough to fit into a sprint

---

# 16. Definition of Done

A backlog item is considered **Done** when:
- The code is implemented
- The application builds successfully
- Functionality works as intended
- Related testing has been performed
- The change does not break existing functionality
- The work is committed and pushed to the repository
- The item meets the agreed acceptance criteria

---

# 17. Architectural Design

The architecture consists of:

## Front End
- Blazor Server components and pages

## Authentication Layer
- ASP.NET Core Identity
- Email/password login
- Role support for Admin and User

## Data Layer
- EF Core DbContext
- SQLite persistence
- Entity models for users, wallets, rooms, sessions, and transactions

## System Design Goals
- Support persistent player accounts
- Support virtual currency
- Support multiplayer room architecture
- Support feature expansion for cosmetics and loot crates

---

# 18. Detailed Design

Detailed design work includes:

- `ApplicationUser` for Identity-based user management
- `AppDbContext` for EF Core configuration
- `Wallet` entity for persistent player balances
- `Transaction` entity for virtual currency history
- `GameRoom` entity for multiplayer lobby structure
- `RoomPlayer` entity for tracking players in a room
- `GameSession` entity for session-specific game state

UI pages/components include:
- Landing page
- Login page
- Dashboard
- Supporting account components

---

# 19. Database Design

The SQLite database currently includes tables for:

- AspNetUsers
- AspNetRoles
- Wallets
- Transactions
- GameRooms
- RoomPlayers
- GameSessions

## Design Goals
- Persistent user account storage
- Secure login integration
- Currency tracking across time
- Multiplayer-ready room/session design
- Extensibility for cosmetics, loot crates, and future gameplay systems

---

# 20. UI/UX Design

UI/UX goals include:
- A themed casino-style interface
- Simple and intuitive navigation
- Clear login/account flow
- Engaging landing page visuals
- Expandable design for future gameplay screens
- Support for player progression and customization features

---

# 21. Implemented User Stories by Respective Owners

Implemented and in-progress stories have included:
- User account creation / authentication support
- User login functionality
- Database-backed account persistence
- Wallet balance storage
- Multiplayer room/session data structure
- Initial landing/dashboard UI

Ownership should be documented by team member in the Trello/Planner export and sprint reports.

---

# 22. Project Execution and Tasks Performed Over Four Sprints

Across four sprints, the team completed work in these areas:

- Planning and backlog development
- Architecture and framework setup
- Database design and migration support
- Identity and login/account integration
- Wallet persistence
- Dashboard/account pages
- Planning for future casino features
- Multiplayer-ready schema design
- UI development and styling

---

# 23. Lessons Learned Regarding Product Development

- Large ideas need to be scoped down early
- Building a strong backend foundation first prevents rework later
- Authentication and database structure are central to many other features
- Planning future features early helps make the schema more scalable
- Building incrementally is more effective than trying to complete everything at once

---

# 24. Lessons Learned Regarding Scrum

- Scrum helped break down a large project into manageable work
- Sprint planning helped clarify priorities
- Backlog grooming helped identify unrealistic scope
- Incremental progress was more effective than waiting for “perfect” solutions
- Communication is essential when dependencies exist between front-end and back-end work

---

# 25. Challenges, Issues, and Resolutions

## Challenges
- Managing project scope
- Package/version compatibility
- Matching SDK, EF, and Identity versions
- Designing a schema that supports future features
- Connecting login/account systems to the database

## Resolutions
- Refined backlog and release plan
- Standardized package versions
- Used EF Core migrations for consistent DB setup
- Tested schema stability after changes
- Iteratively connected authentication to persistence

---

# 26. Open Issues / Concerns

- Additional gameplay systems still need to be implemented
- UI still needs more complete integration with backend systems
- Loot crate and cosmetic systems still require deeper implementation
- Multiplayer game logic beyond schema support remains to be built
- Deployment beyond localhost remains a future step

---

