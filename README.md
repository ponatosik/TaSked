# What is TaSked?

<img src="https://github.com/ponatosik/TaSked/blob/main/src/Presentation/App/App/Resources/AppIcon/taskedico.png?raw=true" align="right"
     alt="TaSked logo" width="128" height="128">

TaSked is a web API and mobile application designed to streamline homework and assignment management for student groups. It serves as platform where students can  manage their homework and track individually track progress.

*TaSked is built entirely on the .NET stack*

### Key features:

1. Task Management: Students within a group can add homework tasks. This ensures that all group members are aware of upcoming assigments.

2. Individual Progress Tracking: While tasks are shared within the group, each student can individually track their progress on these tasks.

3. Offline Capability: The app can function offline, syncing data when an internet connection is reestablished.

<img src="https://github.com/ponatosik/TaSked/assets/76880171/0d661a5c-dace-4b92-b119-9cb45653db6f" width="225" height="525">
<img src="https://github.com/ponatosik/TaSked/assets/76880171/a5d9d2b6-3e22-45c9-84f0-62fcb6a0ad67" width="225" height="525">
<img src="https://github.com/ponatosik/TaSked/assets/76880171/b4f6401d-0ab1-402f-8848-9aa79c154fc5" width="225" height="525">

# Project structure

## Domain
TaSked adopts a Domain-Driven Design (DDD) approach and utilizes shared domain models across both backend and frontend. This architecture ensures a consistent understanding of the problem domain throughout the entire application.

## TaSked API

The TaSked API is the backend that powers the mobile application.  It consist of the following layers:

### Application
- Houses the application services and use cases
- Contains Command and Query handlers (using MediatR)
- Defines DTOs (Data Transfer Objects)
- Specifies interfaces for infrastructure services

### Infrastructure
- Implements the infrastructure concerns
- Manages database context  (Entity Framework Core)
- Handles external service integrations (like Firebase Cloud messaging)

### Presentation
- The ASP.NET Core Web API project
- Contains API controllers
- Manages service configurations

#### Technology  Stack
1. ASP.NET Core: The foundation of our API, providing a high-performance, cross-platform framework for building modern web applications.

2. Entity Framework Core: An object-database mapper for .NET, used for data access and management. It supports our Domain-Driven Design approach with features like rich mapping, change tracking, and migrations.

3. MediatR: Implements the mediator pattern, helping to reduce dependencies between objects and improving the separation of concerns in the application layer.


## TaSked Mobile app

The mobile app depends on defined domain model and special TaSked api client library, defined in TaSked API project folder.

#### Technology  Stack

1. .NET MAUI (Multi-platform App UI): A cross-platform framework for creating native mobile and desktop apps with C# and XAML, allowing us to target Android, iOS, macOS, and Windows with a single codebase.

2. Reactive UI: A composable, cross-platform model-view-viewmodel framework, enhancing our ability to create reactive user interfaces.

3. Akavache: An asynchronous, persistent key-value store for .NET applications, used for local caching and offline capabilities.
