# 🗃️ BuddyMatch - 2-Table Relational Database Architecture

## 📊 Database Schema Diagram

```
┌─────────────────────────────────────┐         ┌─────────────────────────────────────┐
│             USERS TABLE             │         │         USER_PROFILES TABLE         │
│         (Core Information)          │         │       (Study Information)           │
├─────────────────────────────────────┤         ├─────────────────────────────────────┤
│ 🔑 id (PK)          │ INTEGER       │◄────────┤ 🔑 id (PK)          │ INTEGER       │
│ 👤 name             │ VARCHAR(100)  │         │ 🔗 user_id (FK)     │ INTEGER       │
│ 📧 email            │ VARCHAR(100)  │         │ 🎓 program          │ VARCHAR(150)  │
│ 🔒 password         │ VARCHAR(100)  │         │ 💡 interests        │ TEXT          │
│ 📅 created_at       │ TIMESTAMP     │         │ ⏰ availability     │ VARCHAR(100)  │
└─────────────────────────────────────┘         │ 📅 updated_at       │ TIMESTAMP     │
                                                └─────────────────────────────────────┘
```

## 🔗 Relationship Details

### **One-to-One (1:1) Relationship**
```
    USERS                           USER_PROFILES
      │                                   │
      │ ◄──── user_id (Foreign Key) ──────┤
      │                                   │
   One User                         One Profile
```

### **Key Constraints:**
- `users.id` = **Primary Key** (unique identifier)
- `user_profiles.user_id` = **Foreign Key** → references `users.id`
- `user_profiles.user_id` = **Unique** (ensures 1:1 relationship)
- **CASCADE DELETE**: Deleting a user automatically deletes their profile

## 📋 Project Requirements Satisfied

| Requirement | Implementation | ✅ Status |
|-------------|----------------|-----------|
| **Primary Keys** | `users.id` & `user_profiles.id` | ✅ |
| **Relation Cardinality** | One User → One Profile (1:1) | ✅ |
| **Update Customer Address** | Update `user_profiles` table | ✅ |
| **Update Product Price** | Update `program`/`interests` fields | ✅ |

## 🔄 Data Flow Example

### **When fetching a user:**
```sql
SELECT u.id, u.name, u.email, u.password, u.created_at,
       p.program, p.interests, p.availability, p.updated_at
FROM users u
LEFT JOIN user_profiles p ON u.id = p.user_id
WHERE u.email = 'markus.s@cbs.dk'
```

### **When updating profile info:**
```sql
-- Update core user info
UPDATE users 
SET name = 'New Name', email = 'new.email@cbs.dk' 
WHERE id = 2;

-- Update study-related info  
UPDATE user_profiles 
SET program = 'New Program', 
    interests = 'New Interests',
    updated_at = CURRENT_TIMESTAMP
WHERE user_id = 2;
```

## 🎯 Benefits of This Architecture

### **✅ Advantages:**
- **Normalized Data**: No redundancy, clean separation of concerns
- **Referential Integrity**: Foreign keys ensure data consistency
- **Scalability**: Easy to add more profile types or user categories
- **Maintainability**: Clear separation between core user data and profile data
- **Performance**: Indexed foreign keys for fast joins

### **🔧 Frontend Compatibility:**
- **Zero Changes Required**: Frontend still receives the same JSON structure
- **Backward Compatible**: All existing API endpoints work unchanged
- **Transparent**: Users don't notice the database restructuring

## 📈 Sample Data Flow

```
User Login Request
        ↓
API joins users + user_profiles tables
        ↓
Returns combined JSON:
{
  "id": 2,
  "name": "Markus Sørensen",
  "email": "markus.s@cbs.dk",
  "program": "Master - Finance and Investments",
  "interests": "Excel, Corporate Finance",
  "availability": "Tue & Thu 14:00–17:00"
}
```

## 🏗️ Migration Summary

**Before (Single Table):**
```
users: [id, name, email, password, program, interests, availability, created_at]
```

**After (Relational):**
```
users: [id, name, email, password, created_at]
user_profiles: [id, user_id, program, interests, availability, updated_at]
```

This architecture now meets all academic database design principles while maintaining full application functionality! 🚀

---

## 📝 **Updated Project Documentation Section**

### **2.3. Database:**

The database is based on relational design principles with a fully normalized structure consisting of a **two-table architecture** comprising `users` and `user_profiles` tables, ensuring efficient querying, preventing redundancy, and maintaining data integrity through primary and foreign keys. This normalized approach implements a **one-to-one relationship** that separates core user authentication data from study-related academic information, connected through a foreign key constraint that ensures referential integrity.

Our **one-to-one relational design** features the `users` table storing essential authentication data (id, name, email, password, created_at) as the primary entity, while the `user_profiles` table contains academic study information (program, interests, availability, updated_at) with a `user_id` foreign key establishing the relationship. This design satisfies academic database requirements by implementing clear **primary keys** in both tables, demonstrating **relation cardinality** through the one-to-one constraint, and enabling **update operations** equivalent to modifying customer addresses (profile updates) and product prices (program/interest modifications).

Matching relies on structured **JOIN queries** across these normalized tables with indexed foreign keys to support fast lookups based on shared academic programs and interests. The relational design enables sophisticated matching criteria while maintaining optimal query performance through proper indexing on the `user_id` foreign key and frequently queried fields like `program`.

To support development and testing, we manually inserted ten realistic user profiles into the PostgreSQL database, with data automatically distributed across both tables during our migration from the single-table structure. Each profile includes core user attributes in the `users` table (name, email, password) and corresponding study-related entries in the `user_profiles` table (academic program, interests, study availability). This **normalized two-table dataset** was essential for testing full CRUD operations across the relational structure, validating matching logic through JOIN operations, and ensuring proper foreign key constraints. The records allowed us to simulate real-world user behavior, test frontend rendering via Angular components, and verify backend C# logic responsible for filtering and matching profiles across the relational tables.

**SQL MATCHING WITH RELATIONAL JOINS:**

```sql
-- ADVANCED MATCHING WITH TWO-TABLE JOIN
SELECT u.id, u.name, u.email, u.created_at,
       p.program, p.interests, p.availability, p.updated_at
FROM users u
LEFT JOIN user_profiles p ON u.id = p.user_id
WHERE u.id != @userId
  AND (
    p.program = @userProgram
    OR p.interests ILIKE '%' || @userInterest || '%'
  );
```

This implementation demonstrates proper relational database design with **primary-foreign key relationships**, **normalized data structure**, and **efficient JOIN operations** that meet academic database design standards while maintaining full application functionality.
