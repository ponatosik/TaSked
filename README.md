# What is TaSked?

<img src="https://github.com/ponatosik/TaSked/blob/main/src/Presentation/App/App/Resources/AppIcon/taskedico.png?raw=true" align="right"
     alt="TaSked logo" width="128" height="128">

TaSked is a web API and mobile application designed to streamline homework and assignment management for student groups. It serves as platform where students can manage their homework and track individually track progress.


### Screenshots

<img src="https://github.com/user-attachments/assets/0ad1d252-4a1a-4027-9b3d-e94acd021223" width="252" height="588">
<img src="https://github.com/user-attachments/assets/44dc2762-0c58-452b-931e-4b1ae260eb99" width="252" height="588">
<img src="https://github.com/user-attachments/assets/2ad05302-6f74-453e-9b09-acae2174e384" width="252" height="588">


## Key features:

1. Task Management – Students within a group can add homework tasks, ensuring that all members stay informed about upcoming assignments.
2. Individual Progress Tracking – While tasks are shared within the group, each student can track their own progress independently.
3. Offline mode – The app works seamlessly offline, automatically syncing data when an internet connection is restored.
4. Releavnt Information Sharing – Students can store and share subject-related details, such as teacher contact information or group chat invitation links.


## Roadmap

5. Enhanced Student Interaction – Adding comments and assignment feedback to help students discuss time-consuming tasks and share insights on common challenges.
6. Extension Support – Integrating with learning platforms to automatically fetch assignments, keeping information up to date while preserving all core features of TaSked.


## Tech Stack

*TaSked is built entirely on the .NET stack*

 - **Server:** ASP.NET Core, MediatR, Entity Framework Core
 - **Client:** .NET MAUI, ReactiveUI
 - **Connected Services:** Auth0, Firebase Cloud Messaging


# Project structure

### TaSked API
The TaSked API is the backend that powers the mobile application, structured into three main layers:

1. **Application Layer** – Handles business logic, use cases, and request processing (using MediatR).
2. **Infrastructure Layer** – Manages database access (Entity Framework Core) and external integrations (e.g., Firebase Cloud Messaging).
3. **Presentation Layer** – The ASP.NET Core Web API, containing controllers and service configurations.

### TaSked Mobile App
The mobile app integrates with the API using a dedicated **API client project** and shares common data models.


# Authors

- [@ponatosik](https://github.com/ponatosik)
- [@VetsGo](https://github.com/VetsGo)
