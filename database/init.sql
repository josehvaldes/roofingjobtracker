CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE SCHEMA IF NOT EXISTS jobs;

CREATE TABLE IF NOT EXISTS jobs.organizations (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS jobs.workers (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    phone_number VARCHAR(20),
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS jobs.customers (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    phone_number VARCHAR(20),
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS jobs.jobs (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    title VARCHAR(255) NOT NULL,
    description TEXT,
    status TEXT NOT NULL,
    street VARCHAR(255) NOT NULL,
    city VARCHAR(100) NOT NULL,
    state VARCHAR(100) NOT NULL,
    zip_code VARCHAR(20) NOT NULL,
    latitude NUMERIC(9, 6) NOT NULL,
    longitude NUMERIC(9, 6) NOT NULL,
    scheduled_date TIMESTAMPTZ,
    assignee_id UUID REFERENCES jobs.workers(id),
    customer_id UUID NOT NULL REFERENCES jobs.customers(id),
    organization_id UUID NOT NULL REFERENCES jobs.organizations(id),
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    CONSTRAINT chk_jobs_status
        CHECK (status IN ('Draft', 'Scheduled', 'InProgress', 'Completed', 'Cancelled'))
);

CREATE TABLE IF NOT EXISTS jobs.job_photos (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    job_id UUID NOT NULL REFERENCES jobs.jobs(id) ON DELETE CASCADE,
    url TEXT NOT NULL,
    captured_at TIMESTAMPTZ NOT NULL,
    caption TEXT
);

CREATE TABLE IF NOT EXISTS jobs.outbox_messages (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    type TEXT NOT NULL,
    content JSONB NOT NULL,
    occurred_on TIMESTAMPTZ NOT NULL,
    processed_on TIMESTAMPTZ,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- Multi-tenant + status filtering
CREATE INDEX IF NOT EXISTS idx_jobs_jobs_org_status
    ON jobs.jobs (organization_id, status);

-- Multi-tenant filtering
CREATE INDEX IF NOT EXISTS idx_jobs_jobs_organization_id
    ON jobs.jobs (organization_id);

-- Multi-tenant + date-range querying and stable cursor scans
CREATE INDEX IF NOT EXISTS idx_jobs_jobs_org_scheduled_date_id
    ON jobs.jobs (organization_id, scheduled_date, id);

-- Full-text search on title + description
CREATE INDEX IF NOT EXISTS idx_jobs_jobs_title_description_fts
    ON jobs.jobs USING GIN (to_tsvector('english', COALESCE(title, '') || ' ' || COALESCE(description, '')));

-- Supporting indexes for common FK/outbox access paths
CREATE INDEX IF NOT EXISTS idx_jobs_job_photos_job_id
    ON jobs.job_photos (job_id);

CREATE INDEX IF NOT EXISTS idx_jobs_outbox_unprocessed
    ON jobs.outbox_messages (occurred_on)
    WHERE processed_on IS NULL;

