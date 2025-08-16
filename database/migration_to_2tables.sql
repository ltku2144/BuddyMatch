-- ============================================================
-- MIGRATION SCRIPT: Convert Single Table to 2-Table Architecture
-- From: users (single table) -> users + user_profiles (relational)
-- ============================================================

BEGIN;

-- Step 1: Create the new user_profiles table
CREATE TABLE IF NOT EXISTS public.user_profiles (
    id integer NOT NULL,
    user_id integer NOT NULL,
    program character varying(150),
    interests text,
    availability character varying(100),
    updated_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);

-- Step 2: Create sequence for user_profiles
CREATE SEQUENCE IF NOT EXISTS public.user_profiles_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

ALTER SEQUENCE public.user_profiles_id_seq OWNER TO postgres;
ALTER SEQUENCE public.user_profiles_id_seq OWNED BY public.user_profiles.id;
ALTER TABLE ONLY public.user_profiles ALTER COLUMN id SET DEFAULT nextval('public.user_profiles_id_seq'::regclass);

-- Step 3: Migrate data from users table to user_profiles table
INSERT INTO public.user_profiles (user_id, program, interests, availability, updated_at)
SELECT id, program, interests, availability, created_at
FROM public.users
WHERE NOT EXISTS (
    SELECT 1 FROM public.user_profiles WHERE user_profiles.user_id = users.id
);

-- Step 4: Remove the profile columns from users table
ALTER TABLE public.users 
DROP COLUMN IF EXISTS program,
DROP COLUMN IF EXISTS interests,
DROP COLUMN IF EXISTS availability;

-- Step 5: Add constraints to user_profiles
ALTER TABLE ONLY public.user_profiles
    ADD CONSTRAINT user_profiles_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.user_profiles
    ADD CONSTRAINT user_profiles_user_id_key UNIQUE (user_id);

-- Step 6: Add foreign key relationship
ALTER TABLE ONLY public.user_profiles
    ADD CONSTRAINT user_profiles_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(id) ON DELETE CASCADE;

-- Step 7: Create indexes for performance
CREATE INDEX IF NOT EXISTS idx_user_profiles_user_id ON public.user_profiles(user_id);
CREATE INDEX IF NOT EXISTS idx_user_profiles_program ON public.user_profiles(program);
CREATE INDEX IF NOT EXISTS idx_users_email ON public.users(email);

-- Step 8: Update sequence value
SELECT pg_catalog.setval('public.user_profiles_id_seq', (SELECT MAX(id) FROM public.user_profiles), true);

COMMIT;

-- Verification queries:
-- SELECT COUNT(*) FROM public.users;
-- SELECT COUNT(*) FROM public.user_profiles;
-- SELECT u.name, u.email, p.program, p.interests FROM public.users u LEFT JOIN public.user_profiles p ON u.id = p.user_id LIMIT 5;
