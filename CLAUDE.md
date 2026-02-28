# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

GarageVelo — a .NET MAUI mobile application for renting bicycle garages. Users scan QR codes to find garages, view them on a map, subscribe (1€/day, 20€/month, 200€/year), authenticate, and pay. Design inspired by FlowBird (purple/indigo theme).

## Tech Stack

- **.NET 10 MAUI** (Android + iOS + macOS + Windows)
- **CommunityToolkit.Mvvm 8.4** — MVVM with source generators
- **CommunityToolkit.Maui 14.0** — UI helpers, converters
- **Microsoft.Maui.Controls.Maps 10.0** — Map control
- **ZXing.Net.Maui.Controls 0.4** — QR code scanning
- **Shell navigation** with TabBar (Scanner, Carte, Profil)

## Build & Run Commands

```bash
# Restore packages
dotnet restore

# Build (all platforms)
dotnet build GarageVelo/GarageVelo.csproj

# Build for Android only
dotnet build GarageVelo/GarageVelo.csproj -f net10.0-android

# Run on Android emulator
dotnet build GarageVelo/GarageVelo.csproj -t:Run -f net10.0-android
```

## Architecture

```
GarageVelo/
├── GarageVelo.slnx                 ← Solution file
└── GarageVelo/
    ├── GarageVelo.csproj
    ├── MauiProgram.cs              ← DI, plugins
    ├── App.xaml/.cs                ← Theme, session check
    ├── AppShell.xaml/.cs           ← Shell TabBar + routes
    ├── Models/                     ← Garage, User, Subscription, Payment, SubscriptionPlan
    ├── Services/                   ← Interfaces (IAuthService, IGarageService, etc.)
    │   ├── Mock/                   ← In-memory mock implementations
    │   └── Api/                    ← HttpClient stubs (ready for real API)
    ├── ViewModels/                 ← MVVM ViewModels with CommunityToolkit
    ├── Views/                      ← XAML pages
    ├── Converters/                 ← InverseBoolConverter
    └── Resources/Styles/           ← Colors.xaml (FlowBird palette), Styles.xaml
```

## Key Conventions

- **MVVM pattern**: ViewModels use `[ObservableProperty]` and `[RelayCommand]` from CommunityToolkit.Mvvm
- **DI**: All services and ViewModels registered in `MauiProgram.cs`
- **Mock services**: Swap `Mock/` → `Api/` implementations in DI for real backend
- **Navigation**: Shell routes — `//login`, `//main`, `register`, `garageDetail`, `subscription`
- **Demo account**: `demo@garagevelo.fr` / `password123`
- **Theme**: Purple primary (#5F016F), blue secondary (#010BEC), lavender accent (#9896F0)
- **QR payload format**: `{"id":"GV-0001","pos":7,"lock":"814623","size":"Medium"}`

## Repository

- Remote: https://github.com/DenAlexandre/GarageVelo.git
- Branch: main
