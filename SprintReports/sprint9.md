# Sprint 9 Report

## Sprint Metadata
- **Project:** Chuds2Chads Casino
- **Scrum Master:** Reo Day
- **Sprint Duration:** 4/13/2026-4/17/2026
- **Class Sessions / Meetings:** 4/14/2026 and 4/16/2026
- **GitHub Repository:** https://github.com/SamMccorke1/SE2-Project---Casino.git
- **Trello Board:** https://trello.com/b/oZdqoLQ2

## Sprint Goal
Strengthen the project’s integrated gameplay experience by expanding multiplayer and leaderboard support, improving automated testing and CI validation, refreshing key UI areas, and stabilizing Blackjack, Poker, avatar/store features, and wallet-backed game logic.

## PBIs Chosen for the Sprint
- Multiplayer service expansion and lobby/gameflow improvements
- Leaderboard implementation and dashboard support
- Wallet-backed settlement and automated integration testing
- CI workflow refinement and documentation updates
- Avatar customization and cosmetic store improvements
- Blackjack and Poker test stability and code cleanup

### Selection Rationale
- PBIs were selected based on delivery impact and integration priority late in the semester.
- Multiplayer, leaderboard, and wallet-backed testing were prioritized because they directly improved core gameplay completeness and confidence.
- Feature-stabilization work was selected to reduce regression risk before final wrap-up.
- The team also used recent sprint velocity and current integration needs to decide what could be completed within the sprint.

## Sprint Planning Activity Summary
- The team reviewed remaining high-value features and stabilization needs.
- Work was assigned according to specialization: multiplayer and leaderboard work to Reo, wallet and metrics testing to Caleb, avatar/store refinement to Lucas, test stabilization and code cleanup to Matthew, and multiplayer Blackjack/Poker flow improvements to Sam.
- The sprint emphasized system reliability, automated validation, and feature readiness rather than broad new feature expansion.

## Tasks Planned
- Implement and integrate multiplayer and leaderboard services.
- Add wallet-backed settlement and integration testing across games.
- Improve CI workflows and test documentation.
- Refresh dashboard and Roulette UI where needed for the new features.
- Continue improving avatar customization and store logic.
- Fix flaky test behavior and stabilize Blackjack/Poker support.

## Task Details
### Task 1
- **Task Owner:** Reo Day
- **Task Title:** Multiplayer, Leaderboard, CI, and Documentation Integration
- **Task Description:** Improved implementation of multiplayer and leaderboard services and integrated tests, refreshed the dashboard and Roulette UI, wired startup services for leaderboard support, added multiplayer and leaderboard documentation, and improved CI/testing workflows.
- **Task Status:** Complete
- **Challenges Faced:** Keeping UI refresh work aligned with service-layer changes while maintaining working multiplayer behavior.
- **Lessons Learned:** Feature completion is much stronger when documentation, tests, and CI improvements are delivered alongside the code.
- **Planned Time:** 8 hours
- **Actual Time:** 10 hours

### Task 2
- **Task Owner:** Lucas Litzenberger
- **Task Title:** Avatar Customization and Cosmetic Store Refinement
- **Task Description:** Assisted with managing, organizing, fixing, and cleaning the codebase while continuing to develop avatar customization, a cosmetic store to spend chips on accessories, and the related supporting logic.
- **Task Status:** Complete
- **Challenges Faced:** Balancing cleanup work with continued iteration on customization features and chip-spending flows.
- **Lessons Learned:** Cosmetic systems rely on both UI polish and clean supporting data/model logic.
- **Planned Time:** 6 hours
- **Actual Time:** 7 hours

### Task 3
- **Task Owner:** Matthew Williams
- **Task Title:** Test Stabilization and Blackjack Testability Improvements
- **Task Description:** Reviewed directories and documentation, cleaned code, fixed the New Round Test that had a random chance to fail, and updated Blackjack so `Deck()` implements the `IDeck` interface to support fixed-deck testing without changing default behavior.
- **Task Status:** Complete
- **Challenges Faced:** Eliminating nondeterministic test behavior without affecting normal gameplay.
- **Lessons Learned:** Testability improvements can strengthen confidence without changing production behavior when interfaces are introduced carefully.
- **Planned Time:** 5 hours
- **Actual Time:** 6 hours

### Task 4
- **Task Owner:** Caleb Blevins
- **Task Title:** Wallet Settlement, Integration Testing, and Metrics Coverage
- **Task Description:** Implemented and adjusted wallet-backed settlement and integration testing across casino games, added Blackjack and Poker game-logic coverage, introduced settlement services, and added performance metric tests for core game operations.
- **Task Status:** Complete
- **Challenges Faced:** Coordinating wallet-backed behavior consistently across multiple game types and test scenarios.
- **Lessons Learned:** Shared settlement patterns and test infrastructure make multi-game reliability much easier to validate.
- **Planned Time:** 8 hours
- **Actual Time:** 9 hours

### Task 5
- **Task Owner:** Samuel McCorkel
- **Task Title:** Multiplayer Blackjack and Poker Flow Improvements
- **Task Description:** Assisted in managing, organizing, fixing, and cleaning the codebase while continuing to improve multiplayer Blackjack and Poker functionality, gameplay flow, and overall behavior.
- **Task Status:** Complete
- **Challenges Faced:** Maintaining multiplayer game flow while multiple connected systems were changing in the same sprint.
- **Lessons Learned:** Multiplayer card games need both logic stability and careful flow control to feel reliable.
- **Planned Time:** 6 hours
- **Actual Time:** 7 hours

## Roadblocks
- Integration work required coordination across UI, services, tests, and CI at the same time.
- Flaky test behavior had to be fixed before the test suite could be trusted consistently.
- Multiple connected gameplay systems increased the risk of regressions during merge and cleanup work.

## Sprint Review
### Was the sprint goal achieved? Why / Why not?
Yes. Sprint 9 successfully expanded multiplayer and leaderboard support, strengthened wallet-backed testing and settlement flows, improved CI and documentation, and stabilized key game systems for final delivery.

### Acceptance Testing by Product Owner: Feedback
- The expanded automated coverage and improved multiplayer/leaderboard support increased confidence in the integrated product.
- The project became easier to validate because tests, services, and documentation were improved together.

### Adjustments Planned (Product)
- Continue final polish on multiplayer flow and late-stage feature stability.
- Keep using the leaderboard, dashboard, and wallet-backed systems as shared sources of truth.
- Preserve test stability while finishing the remaining final-deliverable work.

### Revised Definition of Done (DoD)
A feature is done when:
- it is implemented and integrated into the shared application,
- automated tests or validation are added or updated where appropriate,
- CI workflows continue to pass,
- documentation is updated if the feature changes system behavior,
- and the feature does not break existing gameplay flow.

### Sprint Review Summary
Sprint 9 was a high-value stabilization and expansion sprint that improved the project’s multiplayer, leaderboard, testing, and operational readiness all at once.

## Sprint Retrospective
### Adjustments Planned (Process)
- Continue addressing test flakiness immediately instead of deferring it.
- Keep pairing feature work with documentation and CI validation.
- Maintain coordinated cleanup during merge-heavy stages to reduce late regressions.

### Sprint Retrospective Summary
The team used Sprint 9 effectively to mature the project instead of only adding surface-level features. The sprint showed strong value in combining feature delivery with testing, cleanup, and workflow improvements.

### Team Velocity
- **Sprint 7:** Strong
- **Sprint 8:** Strong and improving
- **Sprint 9:** High, with major integrated delivery across features, tests, and workflows

## Backlog Grooming Activity Summary
### PBIs Added / Removed / Reprioritized
- Reprioritized multiplayer and leaderboard work because those features now have enough supporting infrastructure to be completed.
- Added more explicit test coverage and CI-related tasks as part of feature completion.
- Continued prioritizing stability, wallet-backed validation, and user-facing polish over speculative feature expansion.

### Definition of Ready (DoR)
A backlog item is ready when:
- it has a clear user or system outcome,
- dependencies are known,
- integration and testing expectations are understood,
- ownership is assigned,
- and it is scoped tightly enough for one sprint.

## Two Daily Scrum Meeting Summaries
### Daily Scrum 1 - 4/14/2026
- **Reo Day:** Working on multiplayer and leaderboard service improvements, automated test/workflows implementation, dashboard integration, and CI/testing improvements; absent from class but communicated progress.
- **Lucas Litzenberger:** Continuing avatar customization, chip-spending store features, and code cleanup; present in class.
- **Matthew Williams:** Cleaning directories and documentation while investigating flaky Blackjack-related test behavior; absent from class but communicated progress.
- **Caleb Blevins:** Working on wallet settlement, integration tests, and metric testing; absent from class but communicated progress.
- **Samuel McCorkel:** Continuing multiplayer Blackjack and Poker cleanup and gameplay-flow improvements; present in class.

### Daily Scrum 2 - 4/16/2026
- **Reo Day:** Completed major multiplayer and leaderboard integration work, documentation, and workflow improvements.
- **Lucas Litzenberger:** Continued customization/store cleanup and feature refinement.
- **Matthew Williams:** Completed Blackjack testing improvement and stabilized tests.
- **Caleb Blevins:** Completed wallet-backed settlement services, integration tests, and performance metric coverage.
- **Samuel McCorkel:** Continued polishing multiplayer Blackjack and Poker game flow and supporting cleanup.

## Attendance
4/14/2026 
Present: Lucas Litzenberger, Samuel McCorkel
Absent: Matthew Williams, Reo Day, Caleb Blevins

4/16/2026
Present: Lucas Litzenberger, Samuel McCorkel, Matthew Williams, Reo Day, Caleb Blevins
