Midterm 2 Repository Documentation
Progress Since Midterm 1
Product Vision

Chuds2Chads Casino is a web-based multiplayer casino platform that allows users to create accounts, manage a virtual wallet, join game rooms, and play casino-style games using virtual currency. Since Midterm 1, the project has shifted from core infrastructure into implemented gameplay systems, multiplayer features, wallet-backed settlement, and automated testing.

Project Goals Since Midterm 1

The major goals for this phase of the project were to:

Expand the application from core account/database functionality into playable game features
Implement casino game systems including roulette, slots, horse race, blackjack, and poker support
Strengthen multiplayer room and friend-based interaction
Add leaderboard functionality
Improve wallet-backed gameplay support
Introduce automated testing and CI validation
Keep the application integrated and runnable from one machine
Release Plan Since Midterm 1
Release 2 / Sprints 5–7 Focus
Roulette gameplay
Slots gameplay
Horse race gameplay
Blackjack gameplay support
Poker gameplay support
Multiplayer room functionality
Leaderboard support
Wallet-backed settlement/integration
Automated testing and GitHub Actions support
Source Code

Since Midterm 1, source code additions and improvements include:

Roulette logic and UI
Slots logic and UI
Horse race logic and UI
Blackjack gameplay logic
Poker gameplay support
Leaderboard service
Multiplayer/friends support
Wallet-backed settlement services
Expanded test suite
GitHub Actions automation support
Development Environment (Tech Stack)
.NET 10
Blazor Server
ASP.NET Core Identity
Entity Framework Core
SQLite
GitHub
GitHub Actions
Visual Studio / VS Code
Deployment Environment

Current deployment remains local development:

run with dotnet run
SQLite local database
browser-based web interface on localhost
Version Management

Since Midterm 1, version management has continued through:

Git branching
GitHub pull requests
feature integration through shared repo workflows
push/pull coordination across teammates
GitHub Actions validation before merge
Test Plan, Tests Performed, Test Types, and Analysis
Test Plan

Since Midterm 1, testing focused on:

validating game result logic
validating wallet-backed settlement flows
validating multiplayer and leaderboard behavior
validating integrated gameplay systems
validating performance of key game operations
verifying CI test execution through GitHub Actions
Tests Performed

The project now includes tests for:

Wallet service behavior
Roulette service behavior
Slots service behavior
Horse race service behavior
Blackjack game logic
Poker hand evaluation
Leaderboard service
Multiplayer service
Wallet-integrated roulette flow
Wallet-integrated slots flow
Wallet-integrated horse race flow
Blackjack settlement flow
Poker buy-in / cash-out settlement flow
Performance metric tests
Test Types
Unit testing
Integration testing
Service testing
Functional testing
Performance / metric smoke testing
Automated CI test execution
Analysis

Current testing results:

88 automated tests passing
0 failed
0 skipped
build passes
application runs successfully
GitHub Actions checks pass

This confirms that the system is stable across gameplay logic, wallet integration, multiplayer services, and performance smoke checks.

Test Automation and Automated Test Execution Configuration

Since Midterm 1, the project now includes automated testing and CI execution.

Automation Support
local execution through:
dotnet build
dotnet test
automated execution through GitHub Actions on branch / PR workflows
Current Automation Coverage
build verification
automated test discovery
automated test execution
CI validation before merge
Change Management / Bug Tracking Process

Since Midterm 1, changes and bug fixes have been handled through:

GitHub branches
pull requests
sprint backlog items
Trello / Planner tracking
test-driven validation of fixes
Process
Identify issue or feature need
Add to sprint/backlog
Assign owner
Implement change
Run tests / retest
Merge once validated
Definition of Ready (Revised)

A backlog item is Ready when:

it is clearly described
acceptance criteria are understood
dependencies are identified
ownership is clear
it is small enough for a sprint
the team understands what completion should look like
Definition of Done (Revised)

A backlog item is Done when:

code is implemented
application builds successfully
functionality works as intended
required tests are completed
no existing functionality is broken
changes are pushed to the repo
work is integrated into the shared codebase
acceptance criteria are satisfied
Architectural Design

Since Midterm 1, the architecture expanded from core app/database support into gameplay and service integration.

Main architectural pieces now include:
Blazor Server UI
ASP.NET Core Identity authentication
EF Core + SQLite persistence
Wallet service and transaction tracking
Leaderboard service
Multiplayer service
Game logic for roulette, slots, horse race, blackjack, and poker
Settlement services for wallet-backed gameplay flows
Automated tests organized by services, games, and metrics
Detailed Design

Detailed additions since Midterm 1 include:

wallet-backed game settlement flow
leaderboard ranking based on wallet balance
multiplayer room/friend features
dedicated game logic classes for blackjack and poker
wallet-integrated test coverage across game systems
metric/performance smoke testing for core game operations
Database Design

The database continues to support:

users
wallets
transactions
game rooms
room players
game sessions

Since Midterm 1, the database design has been exercised more heavily through:

wallet-backed settlement flows
multiplayer room usage
leaderboard balance tracking
transaction history validation
UI/UX Design

Since Midterm 1, UI/UX implementation has expanded to include:

dashboard/game navigation
roulette screen
slots screen
horse race screen
blackjack game screen
poker-related screen support
leaderboard display
multiplayer/friend interaction support

The main goal has been moving from infrastructure into actual user-facing gameplay and interaction.

Implemented User Stories by Respective Owners

Since Midterm 1, implemented stories include:

Roulette gameplay
Slots gameplay
Horse race gameplay
Blackjack gameplay support
Poker gameplay support
Wallet-backed settlement support
Leaderboard ranking
Multiplayer/friend/room behavior
Automated testing expansion
Metric/performance testing
CI validation support

Each team member should explain the user stories they directly contributed to during the demo.

Project Execution and Tasks Performed Over the Four Sprints

Since Midterm 1, the team executed work in these areas:

implementing core casino games
integrating wallet-backed behavior into gameplay flows
expanding multiplayer and leaderboard support
writing automated tests
adding CI-based automated execution
refining service structure
integrating code so it runs from one machine
Lessons Learned Regarding Product Development
Feature completion is not enough unless it is integrated
Service boundaries make wallet/game testing easier
Gameplay systems need consistent settlement logic
Testing exposed architecture mismatches early
Building stable infrastructure first made later feature work easier
Lessons Learned Regarding Scrum
Sprint planning became more important as integration complexity increased
Backlog clarity improved ownership
Definition of Done needed to include testing and integration
Communication mattered more once multiple people touched connected systems
Incremental feature delivery worked better than large untested merges
Challenges, Issues, and Resolutions
Challenges
integrating multiple game systems into one repo
aligning wallet logic with different game types
branch naming / merge coordination
keeping all code runnable from one machine
introducing testing after multiple feature additions
Resolutions
added settlement services where needed
expanded automated integration tests
validated changes with GitHub Actions
improved test structure across services and games
used branch/PR workflow to stabilize integration
Open Issues / Concerns
Poker currently uses wallet-backed buy-in / cash-out settlement rather than full per-action wallet betting
some analyzer warnings remain
UI polish can still improve across some game screens
deployment is still local rather than cloud-hosted
DevOps First Way

The First Way focuses on improving flow from development into integration and delivery.

Since Midterm 1, the project supports the First Way by:

maintaining one integrated codebase
running the application from one machine
using Git branches and pull requests for integration
validating builds/tests before merge
integrating gameplay, wallet, multiplayer, and testing into one shared workflow
DevOps Second Way

The Second Way focuses on fast feedback.

This is one of the strongest areas of the project since Midterm 1 because the team added:

automated test coverage
wallet integration tests
service tests
game logic tests
performance/metric tests
GitHub Actions execution on PR/branch workflows

This gives immediate feedback when code breaks or behavior changes.

DevOps Third Way

The Third Way focuses on continuous learning and experimentation.

Since Midterm 1, the project supports this by:

expanding test coverage iteratively
learning from integration issues
improving architecture where wallet/game behavior did not align cleanly
using metric tests to make performance measurable
refining process through repeated sprint work
Current Progress Summary

Since Midterm 1, the project has moved from infrastructure-focused development into integrated gameplay, service expansion, automated testing, and CI validation.

Current measurable status
88 automated tests passing
0 failed
0 skipped
build succeeds
app runs successfully
GitHub Actions checks pass
