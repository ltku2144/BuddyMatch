#!/bin/bash

# Test script to verify the fixed user creation
echo "Testing user creation with profile..."

# Create a new test user
curl -X POST "http://localhost:5072/api/User" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test User",
    "email": "test.user@cbs.dk",
    "passwordHash": "TestPassword123",
    "userProfile": {
      "program": "Test Program",
      "interests": "Testing, APIs",
      "availability": "Mon-Fri 09:00-17:00"
    }
  }'

echo -e "\n\nUser creation completed. Check database to verify both user and profile were created."
