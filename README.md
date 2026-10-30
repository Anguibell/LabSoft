# 🧪 LabSoft – Full Stack Clinical Laboratory Information Management System (LIMS)

![C#](https://img.shields.io/badge/C%23-239120?style=flat&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=flat&logo=dotnet&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?style=flat&logo=typescript&logoColor=white)
![React](https://img.shields.io/badge/React-20232A?style=flat&logo=react&logoColor=61DAFB)
![Tailwind CSS](https://img.shields.io/badge/Tailwind_CSS-38B2AC?style=flat&logo=tailwind-css&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=flat&logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat&logo=docker&logoColor=white)

## 📌 Project Overview
**LabSoft** is a full-stack web application and SaaS MVP designed for the comprehensive operational and administrative management of a clinical laboratory (**LIMS – Laboratory Information Management System**).

Engineered from scratch with **React and TypeScript** on the frontend, **C# and ASP.NET Core Web API** following **Clean Architecture** on the backend, and **PostgreSQL** as the relational database engine, LabSoft digitizes the entire clinical workflow—from patient intake and lab order creation to reference-range result validation and automated clinical PDF report generation.

🌐 **Live Frontend Demo (Vercel):** [https://lab-soft.vercel.app](https://lab-soft.vercel.app)

---

## 🏗️ System Architecture

### 1. Backend Architecture (Clean Architecture in C# / .NET)
The backend is structured into 4 decoupled layers to guarantee maintainability, testability, and strict separation of concerns:
- **`LabSoft.Domain`:** Core enterprise entities, clinical domain models, and business constants.
- **`LabSoft.Application`:** Data Transfer Objects (DTOs), service interfaces, application business logic, and input validation rules powered by **FluentValidation**.
- **`LabSoft.Infrastructure`:** Relational data persistence using **Entity Framework Core**, PostgreSQL repositories, database seeders, and technical integrations (including **QuestPDF** report generation).
- **`LabSoft.API`:** RESTful HTTP controllers, custom exception-handling middlewares, and **JWT Role-Based Authorization**.

### 2. Frontend Architecture (Modular React + TypeScript)
Built with **Vite**, **React**, and **TypeScript**, organized into feature-driven functional modules (`auth`, `dashboard`, `patients`, `catalog`, `panels`, `orders`, `results`, `settings`):
- **Server State & Caching:** Managed asynchronously via **TanStack Query (React Query)** and **Axios**.
- **Global UI & Session State:** Lightweight state management with **Zustand**.
- **Form Handling & Schema Validation:** Strongly-typed forms using **React Hook Form** and **Zod**.
- **UI/UX Design:** Clean, responsive clinical interface styled with **Tailwind CSS**, featuring reusable data tables, status badges, modal workflows, and multi-criteria search filters.

---

## 🚀 Core Functional Modules (MVP)

1. **📊 Executive & Operational Dashboard:** Real-time KPIs tracking received orders, attended patients, completed tests, pending queues, specialty breakdowns, and most-requested exams filterable by date range.
2. **👥 Patient Management:** Complete patient registry supporting national ID validation, birthdates, sex, contact history, specialized classification for newborns/minors without IDs, and primary holder-to-beneficiary relationships.
3. **🧬 Hierarchical Clinical Exam Catalog:** Management of laboratory tests organized by medical specialty, custom codes, pricing, measurement units, decimal precision, sex-specific reference ranges, and composite multi-parameter hierarchies (e.g., Complete Blood Count).
4. **🗂️ Exam Panels & Profiles:** Grouping of individual clinical tests into predefined diagnostic profiles (e.g., Clinical Chemistry Panel) to streamline order creation.
5. **📝 Lab Order Processing:** End-to-end order reception workflow supporting patient lookup/registration, individual test assignment, profile selection, and operational status tracking.
6. **🔬 Clinical Results & Reference Validation:** Entry and validation of numerical and qualitative test results, clinical observations, and automated flagging of out-of-range values against configured reference intervals.
7. **📄 Automated Clinical PDF Reports (`QuestPDF`):** Server-side generation of printable patient result reports featuring institutional branding, patient demographics, order metadata, parameter results, visual out-of-range indicators, lab signatures/seals, and pagination.
8. **🔐 Security, JWT Authentication & Roles:** Token-based authentication and protected API endpoints using ASP.NET Core Authorization with role-based access control.

---

## 🛠️ Tech Stack Summary

| Layer | Technologies & Libraries |
| :--- | :--- |
| **Backend** | C#, ASP.NET Core Web API, Clean Architecture, Entity Framework Core, FluentValidation, QuestPDF, JWT Auth |
| **Frontend** | React, TypeScript, Vite, Tailwind CSS, TanStack Query, Zustand, React Hook Form, Zod, React Router, Axios |
| **Database** | PostgreSQL (Relational Modeling, Code-First Migrations, Seeders) |
| **DevOps & Cloud** | Docker (`Dockerfile`), Render (Backend Web Service & Managed PostgreSQL), Vercel (Frontend Hosting) |

---

## 📂 Repository Structure

```text
LabSoft/
│
├── LabSoft.sln                     # .NET Solution file
├── Dockerfile                      # Multi-stage Docker build for Render deployment
│
├── src/                            # Backend (Clean Architecture)
│   ├── LabSoft.API/                # HTTP Controllers, Middlewares, Program.cs
│   ├── LabSoft.Application/        # DTOs, Interfaces, Application Services & FluentValidation
│   ├── LabSoft.Domain/             # Core Clinical Entities & Domain Models
│   └── LabSoft.Infrastructure/     # EF Core Persistence, PostgreSQL Repositories & Seeders
│
├── frontend/
│   └── labsoft-web/                # Frontend (React + TypeScript + Vite)
│       ├── public/
│       ├── src/
│       │   ├── app/
│       │   ├── components/         # Reusable UI components
│       │   ├── hooks/              # Custom React hooks
│       │   ├── layouts/            # Administrative dashboard layouts
│       │   ├── lib/                # Axios & TanStack Query configurations
│       │   ├── modules/            # Feature modules (auth, dashboard, patients, catalog, panels, orders, results, settings)
│       │   ├── routes/             # Protected routing definitions
│       │   ├── types/              # TypeScript interfaces & domain contracts
│       │   ├── utils/              # Formatting & helper functions
│       │   ├── App.tsx
│       │   └── main.tsx
│       ├── package.json
│       ├── tsconfig.json
│       └── vite.config.ts
│
└── README.md
```

---

## ⚙️ Local Development Setup

### Prerequisites
- **.NET SDK** (8.0+)
- **Node.js** (18+) & **npm**
- **PostgreSQL** (Local instance or cloud connection string)

### 1. Backend Setup (`src/`)
1. Clone the repository:
   ```bash
   git clone https://github.com/Anguibell/LabSoft.git
   cd LabSoft
   ```
2. Configure your PostgreSQL connection string and JWT settings in `src/LabSoft.API/appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=LabSoftDb;Username=postgres;Password=your_password"
     },
     "Jwt": {
       "Key": "your_super_secret_development_key_here",
       "Issuer": "LabSoftAPI",
       "Audience": "LabSoftWeb"
     }
   }
   ```
3. Apply Entity Framework Core migrations and run the API:
   ```bash
   dotnet ef database update --project src/LabSoft.Infrastructure --startup-project src/LabSoft.API
   dotnet run --project src/LabSoft.API
   ```

### 2. Frontend Setup (`frontend/labsoft-web/`)
1. Navigate to the frontend folder and install dependencies:
   ```bash
   cd frontend/labsoft-web
   npm install
   ```
2. Configure your `.env` file pointing to the local backend API URL:
   ```env
   VITE_API_URL=http://localhost:5000/api
   ```
3. Start the Vite development server:
   ```bash
   npm run dev
   ```

---

## 👩‍💻 Author
**Anguibell Romero** – *Systems Engineer & Full Stack Software Developer*  
- **LinkedIn:** [linkedin.com/in/anguibell-romero](https://www.linkedin.com/in/anguibell-romero)  
- **GitHub:** [github.com/Anguibell](https://github.com/Anguibell)
