# CommandCentral_API_V2

# Todo

- User
  - [x] Create user
  - [x] Login User
  - [x] Get users
  - [x] Get user
  - [x] Edit user (*)
  - [x] Delete user
- Household
  - Register Household - POST
  - Edit Household
  - Delete Household
- Admin
  - See Households
  - Delete Households
  - See Users
  - Delete users
- GroceryList
  - Edit items - HUB
  - Add items - HUB
  - Delete items - HUB
  - (maybe create a grocerylist, maybe auto create per week)



# Migration commands
- `dotnet ef migrations add InitialAPIMigration -o ./SharedAPI/Persistence/Migrations/ -c ApiDbContext`
- `dotnet ef migrations add InitialMigration -o ./Identity/Migrations/ -c AuthDbContext`
- `dotnet ef migrations add InitialMigration -o ./SharedAPI/Persistence/Migrations`
