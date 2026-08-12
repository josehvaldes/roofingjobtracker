
CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE SCHEMA IF NOT EXISTS jobs;

-- Leave schema/table/index creation to EF Core migrations.
-- This file only handles database bootstrap prerequisites that exist outside the EF model.
