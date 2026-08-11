# roofingjobtracker
a Roofing job tracker system for portfolio project



## 0 -- Domain + architecture sketch

local deployment via /docker/docker-compose.yml file

```text

┌───────────────────────────────────────────────────┐
│                  Docker Compose Network           │
│                                                   │
│    ┌────────────┐                                 │
│    │  FrontEnd  │                                 │
│    │ (Next.js)  │                                 │
│    └──────┬─────┘                                 │
│           │                                       │
│  ┌────────┴─────────────────────────┐             │
│  │  ┌─────┴─────┐  ┌──────────────┐ │             │
│  │  │ .Net Api  │=>│ HangfireJobs │ │             │
│  │  │           │  │              │ │             │
│  │  └─────┬─────┘  └──────────────┘ │             │
│  └────────┐─────────────────────────┘             │
│    ┌──────┴─────┐                                 │
│    │            │                                 │
│    │ PostgresDB │                                 │
│    └────────────┘                                 │
└───────────────────────────────────────────────────┘

```

## 1 -- Database Design with PostgreSQL
Postgres version: 18
GUID generator : gen_random_uuid()
Database extensions: pgcrypto for GUID generation
SQL script: /database/init.sql

```mermaid
erDiagram
    ORGANIZATION {
        UUID id PK
        VARCHAR name
        TIMESTAMPTZ created_at
        TIMESTAMPTZ updated_at
    }

    WORKER {
        UUID id PK
        VARCHAR name
        VARCHAR email UK
        VARCHAR phone_number
        TIMESTAMPTZ created_at
        TIMESTAMPTZ updated_at
    }

    CUSTOMER {
        UUID id PK
        VARCHAR name
        VARCHAR email UK
        VARCHAR phone_number
        TIMESTAMPTZ created_at
        TIMESTAMPTZ updated_at
    }

    JOBS {
        UUID id PK
        VARCHAR title
        TEXT description
        TEXT status
        TIMESTAMPTZ scheduled_date
        UUID assignee_id FK
        UUID customer_id FK
        UUID organization_id FK
        TIMESTAMPTZ created_at
        TIMESTAMPTZ updated_at
    }

    JOB_ADDRESSES {
        UUID id PK
        UUID job_id FK
        VARCHAR street
        VARCHAR city
        VARCHAR state
        VARCHAR zip_code
        DECIMAL latitude
        DECIMAL longitude
    }

    JOB_PHOTOS {
        UUID id PK
        UUID job_id FK
        TEXT url
        TIMESTAMPTZ captured_at
        TEXT caption
    }

    OUTBOX_MESSAGES {
        UUID id PK
        TEXT type
        JSONB content
        TIMESTAMPTZ occurred_on
        TIMESTAMPTZ processed_on
        TIMESTAMPTZ created_at
    }

    ORGANIZATION ||--o{ JOBS : "organization_id"
    CUSTOMER ||--o{ JOBS : "customer_id"
    WORKER ||--o{ JOBS : "assignee_id"
    JOBS ||--o| JOB_ADDRESSES : "job_id (1:1)"
    JOBS ||--o{ JOB_PHOTOS : "job_id (1:N)"
```

## 2 -- .NET Modular Monolith with DDD 
## 2.1 -- Monolith Foundation (Solution setup, Clean Architecture projects, DI)
## 2.2 -- Job aggregate + value objects + domain events
## 2.3 -- CQRS + MediatR + validation + repository
## 2.4 -- Outbox + Hangfire + integration events
## 2.5 -- Backend Unit Tests
## 3 -- TypeScript Deep Dive
## 4 -- Next.js Architecture and Patterns
## 4.1 -- FSD Architecture + Server/Client boundary 
## 4.2 -- Zustand store + React Design Patterns
## 4.3 -- Frontend Unit Tests
## 5 -- Testing & Bugfixing, instead of testing since testing was implemented in the previous steps
## 6 -- System Design + README + docker