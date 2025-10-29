# 🏨 Hotel Management System (HMS)

A comprehensive web-based hotel management system built with ASP.NET Core MVC that manages hotel operations including room booking, customer check-in/out, payments, and staff management.

## 🎯 Features

### 👤 User Module
- **User Registration & Login** - Secure authentication using ASP.NET Identity
- **View Available Rooms** - Browse and filter rooms by type, price, and availability
- **Book a Room** - Easy booking process with date selection
- **Booking History** - View all your bookings in one place
- **Cancel Booking** - Cancel bookings (with policy restrictions)
- **Profile Management** - Update personal information (name, email, phone, ID proof)

### 👨‍💼 Admin Module
- **Dashboard** - Overview with statistics (total rooms, booked rooms, available rooms, revenue)
- **Room Management** - Add, edit, delete, and manage room availability
- **Customer Management** - View all registered customers
- **Booking Management** - View and manage all bookings
- **Staff Management** - Add, edit, and remove staff members
- **Reports** - Generate daily and monthly booking reports
- **Invoice Generation** - Download PDF invoices for bookings

### 🛏️ Room Module
- Room types: Single, Double, Deluxe, Suite
- Price per night configuration
- Status management: Available, Booked, Cleaning
- Room descriptions and image support

### 📅 Booking Module
- Date selection for check-in and check-out
- Automatic cost calculation based on duration
- Booking confirmation system
- Automatic room status updates

### 💳 Payment Module
- Mock payment processing (automatic confirmation)
- Payment status tracking
- PDF invoice generation using QuestPDF

### 👨‍🍳 Staff Management
- Staff list with roles and shift timings
- Full CRUD operations
- Admin-only access

## 🛠️ Tech Stack

- **Frontend**: Razor Pages / MVC Views, Bootstrap 5, Chart.js
- **Backend**: ASP.NET Core MVC
- **Database**: SQL Server (LocalDB)
- **ORM**: Entity Framework Core
- **Authentication**: ASP.NET Identity
- **PDF Generation**: QuestPDF
- **Icons**: Bootstrap Icons

## 📋 Prerequisites

Before running this project, ensure you have the following installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- SQL Server LocalDB (comes with Visual Studio) or SQL Server Express

## 🚀 Installation & Setup

### Step 1: Clone the Repository

```bash
git clone https://github.com/your-username/HotelManagementSystem.git
cd HotelManagementSystem
```

### Step 2: Restore NuGet Packages

```bash
dotnet restore
```

### Step 3: Update Database Connection

Open `appsettings.json` and verify the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HotelManagementSystemDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### Step 4: Create and Seed Database

The database will be automatically created and seeded when you run the application for the first time. The seed data includes:

- Admin user: `admin@hotel.com` / `Admin@123`
- Sample rooms (Single, Double, Deluxe, Suite)
- Sample staff members

### Step 5: Run the Application

```bash
dotnet run
```

Or use Visual Studio:
- Press `F5` to run the application
- Navigate to `https://localhost:5001` or `http://localhost:5000`

## 👥 Default Credentials

### Admin Account
- **Email**: `admin@hotel.com`
- **Password**: `Admin@123`

### User Account
- Register a new account from the registration page

## 📁 Project Structure

```
HotelManagementSystem/
│
├── Controllers/
│   ├── HomeController.cs
│   ├── RoomController.cs
│   ├── BookingController.cs
│   ├── AdminController.cs
│   ├── StaffController.cs
│   ├── AccountController.cs
│   └── InvoiceController.cs
│
├── Models/
│   ├── ApplicationUser.cs
│   ├── Room.cs
│   ├── Booking.cs
│   ├── Payment.cs
│   ├── Staff.cs
│   └── Feedback.cs
│
├── Views/
│   ├── Home/
│   ├── Room/
│   ├── Booking/
│   ├── Admin/
│   ├── Staff/
│   ├── Account/
│   └── Shared/
│
├── Data/
│   ├── ApplicationDbContext.cs
│   └── SeedData.cs
│
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── images/
│
├── appsettings.json
└── Program.cs
```

## 🗄️ Database Schema

### Tables

- **Users** - User accounts (ASP.NET Identity)
- **Rooms** - Room information
- **Bookings** - Booking records
- **Payments** - Payment transactions
- **Staff** - Staff information
- **Feedback** - Customer feedback (optional)

## 🎨 UI Features

- **Responsive Design** - Works on desktop, tablet, and mobile devices
- **Modern UI** - Clean, professional design with Bootstrap 5
- **Interactive Dashboard** - Admin dashboard with statistics and charts
- **User-Friendly Forms** - Easy-to-use forms with validation
- **Real-time Updates** - Instant status updates and notifications

## 🔐 Security Features

- Password hashing using ASP.NET Identity
- Role-based authorization (Admin/User)
- CSRF protection
- Secure authentication cookies
- Input validation

## 📊 Admin Dashboard Features

- Total rooms count
- Available rooms count
- Booked rooms count
- Total revenue calculation
- Recent bookings table
- Monthly booking statistics (Chart.js)

## 🔄 Booking Flow

1. User browses available rooms
2. User selects a room and views details
3. User selects check-in and check-out dates
4. System calculates total cost
5. User proceeds to payment
6. Payment confirmation updates booking status
7. Room status automatically changes to "Booked"
8. User can download invoice PDF

## 📝 API Endpoints

### Public Routes
- `GET /` - Home page
- `GET /Room` - List all rooms
- `GET /Room/Details/{id}` - Room details
- `GET /Account/Login` - Login page
- `GET /Account/Register` - Registration page

### Authenticated Routes (User)
- `GET /Booking` - User's bookings
- `POST /Booking/Create` - Create booking
- `GET /Booking/Payment/{bookingId}` - Payment page
- `POST /Booking/ProcessPayment` - Process payment
- `GET /Booking/Details/{id}` - Booking details
- `POST /Booking/Cancel/{id}` - Cancel booking
- `GET /Account/Profile` - User profile
- `POST /Account/UpdateProfile` - Update profile

### Admin Routes
- `GET /Admin/Dashboard` - Admin dashboard
- `GET /Admin/Rooms` - Manage rooms
- `POST /Admin/CreateRoom` - Create room
- `POST /Admin/EditRoom/{id}` - Edit room
- `POST /Admin/DeleteRoom/{id}` - Delete room
- `GET /Admin/Customers` - View customers
- `GET /Admin/Bookings` - View all bookings
- `GET /Admin/Reports` - Generate reports
- `GET /Staff` - Manage staff
- `POST /Staff/Create` - Add staff
- `POST /Staff/Edit/{id}` - Edit staff
- `POST /Staff/Delete/{id}` - Delete staff

## 🐛 Troubleshooting

### Database Connection Issues
- Ensure SQL Server LocalDB is installed
- Check connection string in `appsettings.json`
- Try creating the database manually using Entity Framework migrations

### Migration Issues
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Package Restore Issues
```bash
dotnet nuget locals all --clear
dotnet restore
```

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is licensed under the MIT License.

## 👨‍💻 Author

Created as part of a hotel management system project.

## 🙏 Acknowledgments

- Bootstrap 5 for UI components
- QuestPDF for PDF generation
- Chart.js for data visualization
- ASP.NET Core team for the excellent framework

## 📞 Support

For support, please open an issue in the GitHub repository.

---

