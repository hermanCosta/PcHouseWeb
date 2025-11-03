-- Migration Script: Add password_hash column to company table
-- Run this if you already have the database schema without the password field

ALTER TABLE company 
ADD COLUMN password_hash VARCHAR(255) NOT NULL DEFAULT '' AFTER shipping_address_id;

-- Update existing companies with a default password
-- Default password: "password123" hashed with SHA256
UPDATE company 
SET password_hash = 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f'
WHERE password_hash = '' OR password_hash IS NULL;

-- Remove default constraint after updating existing records
ALTER TABLE company 
MODIFY COLUMN password_hash VARCHAR(255) NOT NULL;

