# Quick Start Guide

## First Time Setup

1. **Install Prerequisites**
   - .NET 8.0 SDK
   - Visual Studio 2022 or VS Code
   - SQL Server LocalDB

2. **Clone and Navigate**
   ```bash
   git clone <repository-url>
   cd HotelManagementSystem
   ```

3. **Restore Packages**
   ```bash
   dotnet restore
   ```

4. **Run the Application**
   ```bash
   dotnet run
   ```

5. **Access the Application**
   - Open browser: `https://localhost:5001` or `http://localhost:5000`
   - Database will be automatically created and seeded

## Default Login Credentials

### Admin
- Email: `admin@hotel.com`
- Password: `Admin@123`

### User
- Register a new account from the registration page

## Features Overview

### For Users
- Browse and search rooms
- Book rooms with date selection
- View booking history
- Cancel bookings
- Update profile
- Download invoices

### For Admins
- Dashboard with statistics
- Manage rooms (CRUD)
- View all bookings
- Manage customers
- Manage staff
- Generate reports
- View revenue statistics

## Common Tasks

### Creating a New Room
1. Login as admin
2. Go to Admin Dashboard → Manage Rooms
3. Click "Add New Room"
4. Fill in the details and submit

### Making a Booking
1. Register/Login as user
2. Browse rooms
3. Select a room
4. Click "Book Now"
5. Select check-in and check-out dates
6. Complete payment
7. Booking confirmed!

### Generating Reports
1. Login as admin
2. Go to Admin Dashboard → Reports
3. Select period (Daily/Monthly/All)
4. View booking statistics

## Troubleshooting

### Database Issues
- Ensure SQL Server LocalDB is running
- Check connection string in `appsettings.json`
- Try deleting the database and restarting the app

### Build Errors
- Run `dotnet clean` then `dotnet build`
- Ensure all NuGet packages are restored

## Support

For issues or questions, please check the main README.md file or open an issue in the repository.

