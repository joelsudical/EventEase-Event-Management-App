# EventEase: Event Management App

EventEase is a Blazor Server event management app for browsing events, registering attendees, tracking sessions, and recording check-ins.

## Features

- Searchable, category-filtered event catalog with virtualized results
- Validated event registration and edit forms
- Circuit-scoped attendee session state
- Event check-in and virtualized attendance overview
- Responsive event, registration, and attendance views

## Run locally

Requires the .NET 8 SDK.

```powershell
dotnet run --urls http://localhost:5080
```

Open `http://localhost:5080` in a browser.

## Build and publish

```powershell
dotnet build
dotnet publish --configuration Release
```

Registration and attendance currently use in-memory storage for this learning project. Replace these services with durable storage before deploying for real users.