
CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE SCHEMA IF NOT EXISTS tracker;

CREATE TABLE IF NOT EXISTS tracker.organization (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS tracker.worker (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    phone_number VARCHAR(20),
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS tracker.customer (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    phone_number VARCHAR(20),
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS tracker.jobs (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    title VARCHAR(255) NOT NULL,
    description TEXT,
    status TEXT NOT NULL,
    scheduled_date TIMESTAMPTZ NOT NULL,
    assignee_id UUID REFERENCES tracker.worker(id),
    customer_id UUID NOT NULL REFERENCES tracker.customer(id),
    organization_id UUID NOT NULL REFERENCES tracker.organization(id),
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    CONSTRAINT chk_tracker_status
        CHECK (status IN ('Draft', 'Scheduled', 'InProgress', 'Completed', 'Cancelled'))
);

CREATE TABLE IF NOT EXISTS tracker.job_addresses (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    job_id UUID NOT NULL UNIQUE REFERENCES tracker.jobs(id) ON DELETE CASCADE,
    street VARCHAR(255) NOT NULL,
    city VARCHAR(100) NOT NULL,
    state VARCHAR(100) NOT NULL,
    zip_code VARCHAR(20) NOT NULL,
    latitude DECIMAL(9, 6) NOT NULL,
    longitude DECIMAL(9, 6) NOT NULL
);

CREATE TABLE IF NOT EXISTS tracker.job_photos (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    job_id UUID NOT NULL REFERENCES tracker.jobs(id) ON DELETE CASCADE,
    url TEXT NOT NULL,
    captured_at TIMESTAMPTZ NOT NULL,
    caption TEXT
);

CREATE TABLE IF NOT EXISTS tracker.outbox_messages (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    type TEXT NOT NULL,
    content JSONB NOT NULL,
    occurred_on TIMESTAMPTZ NOT NULL,
    processed_on TIMESTAMPTZ,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- Multi-tenant + status filtering
CREATE INDEX IF NOT EXISTS idx_jobs_org_status
    ON tracker.jobs (organization_id, status);

-- Multi-tenant + date-range querying and stable cursor scans
CREATE INDEX IF NOT EXISTS idx_jobs_org_scheduled_date_id
    ON tracker.jobs (organization_id, scheduled_date, id);

-- Full-text search on title + description
CREATE INDEX IF NOT EXISTS idx_jobs_title_description_fts
    ON tracker.jobs USING GIN (to_tsvector('english', COALESCE(title, '') || ' ' || COALESCE(description, '')));

-- Supporting indexes for common FK/outbox access paths
CREATE INDEX IF NOT EXISTS idx_job_addresses_job_id
    ON tracker.job_addresses (job_id);

CREATE INDEX IF NOT EXISTS idx_job_photos_job_id
    ON tracker.job_photos (job_id);

CREATE INDEX IF NOT EXISTS idx_outbox_unprocessed
    ON tracker.outbox_messages (occurred_on)
    WHERE processed_on IS NULL;