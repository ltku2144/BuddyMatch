-- Fix missing profile for user_id 12 (Alejandro Toro)
-- This script adds a default profile for the user that was created before the fix

-- Check if profile already exists
SELECT 'Checking existing profile for user_id 12:' AS status;
SELECT * FROM public.user_profiles WHERE user_id = 12;

-- Insert missing profile if it doesn't exist
INSERT INTO public.user_profiles (user_id, program, interests, availability) 
VALUES (12, 'Computer Science', 'Programming, Databases', 'Mon-Fri 10:00-16:00') 
ON CONFLICT (user_id) DO UPDATE SET
    program = EXCLUDED.program,
    interests = EXCLUDED.interests,
    availability = EXCLUDED.availability,
    updated_at = CURRENT_TIMESTAMP;

-- Verify the fix
SELECT 'Profile after fix:' AS status;
SELECT * FROM public.user_profiles WHERE user_id = 12;

-- Show all users with their profiles to verify data consistency
SELECT 'All users with profiles:' AS status;
SELECT 
    u.id as user_id,
    u.name,
    u.email,
    p.program,
    p.interests,
    p.availability
FROM public.users u
LEFT JOIN public.user_profiles p ON u.id = p.user_id
ORDER BY u.id;
