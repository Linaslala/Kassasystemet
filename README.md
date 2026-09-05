# Kassasystemet (Cash Register System)

A layered C# console application that manages products, members, campaigns and receipts for a small store. Built as the final examination project in object-oriented programming.

> The user interface is in Swedish — the application is a "klubb livs" (member shop) where you log in as a cashier.

## Tech stack & skills shown

- **C# / .NET 10**
- **Layered architecture** — `BusinessLogic`, `ConsoleAppUI` and data access are separated
- **SOLID principles** — interfaces for every responsibility (`IMemberModel`, `ISaveReceiptToFile`, etc.)
- **File-based persistence** — data is saved to plain-text files with separate read/save managers
- **Console UX** — arrow-key navigation, centered text, validated input and a receipt printer

## Features

- **Products** — create, list, update and delete products
- **Members** — create, list, update, delete and search members
- **Campaigns** — create, list, update, delete and search campaigns with percent-off campaigns
- **Purchases** — create a purchase and resume an ongoing one
- **Sales reports** — list receipts and find a specific receipt
- **Seeding** — the app seeds demo data automatically on first start

## Project structure

```
Kassasystemet/
├── BusinessLogic/          # Domain logic, split by feature
│   ├── CampaignLogic/      #   campaigns (models, data managers, interfaces)
│   ├── MemberLogic/        #   members
│   ├── ProductLogic/       #   products
│   ├── ReceiptLogic/       #   receipts and cart items
│   └── Seed/               #   demo-data seeder
├── ConsoleAppUI/           # Console user interface
│   ├── Menues/             # main, member, product, campaign, purchase, sales-report menus
│   ├── MenueOptionCalls/   # one class per menu action
│   └── HelpMethods/        # centered output, arrow navigation, validated input, receipt printer
└── TextFiles/              # generated storage files (.txt), ignored in git
```

## Getting started

```bash
git clone https://github.com/Linaslala/Kassasystemet.git
cd Kassasystemet
dotnet run --project Kassasystemet
```

Requires the .NET 10 SDK.

## Possible future improvements

- Replace file storage with a database (e.g. EF Core / SQLite)
- Add authentication for cashier logins
- Add unit tests for the business logic
- Export receipts to PDF