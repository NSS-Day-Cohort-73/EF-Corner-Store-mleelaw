#!/bin/bash
DB_NAME="CornerStore"
DOTNET_CMD="dotnet ef"

echo "Dropping database: $DB_NAME..."
$DOTNET_CMD database drop --force

echo "Removing migrations..."
rm -rf Migrations/

echo "Adding new migration..."
$DOTNET_CMD migrations add InitialCreate

echo "Applying migrations..."
$DOTNET_CMD database update

echo "Database reset complete!"