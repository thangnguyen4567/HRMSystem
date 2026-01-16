# HRM System - Quản Lý Nhân Viên

Hệ thống quản lý nhân viên được xây dựng bằng ASP.NET Core 8.0 với Entity Framework Core và PostgreSQL.

## Tính năng

- ✅ Đăng ký và đăng nhập người dùng
- ✅ JWT Authentication
- ✅ CRUD operations cho nhân viên
- ✅ Quản lý trạng thái hoạt động của nhân viên
- ✅ API documentation với Swagger

## Công nghệ sử dụng

- **ASP.NET Core 8.0** - Framework chính
- **Entity Framework Core** - ORM
- **PostgreSQL** - Cơ sở dữ liệu
- **ASP.NET Identity** - Quản lý người dùng và xác thực
- **JWT Bearer Authentication** - Xác thực API
- **Swagger/OpenAPI** - Tài liệu API
- **Docker** - Container hóa

## Cài đặt và Chạy

### 1. Yêu cầu hệ thống

#### Chạy với Docker (Khuyến nghị)

- Docker Desktop hoặc Docker Engine
- Docker Compose

#### Chạy thủ công (Development)

- .NET 8.0 SDK
- PostgreSQL 12+
- Visual Studio 2022 hoặc VS Code

### 2. Chạy với Docker (Khuyến nghị)

```bash
# Clone repository
git clone <repository-url>
cd hrm-system

# Chạy ứng dụng với Docker Compose
docker-compose up -d

# Truy cập: http://localhost:8080
```

### 3. Chạy thủ công (Development)

#### Thiết lập Database

1. Cài đặt PostgreSQL và tạo database:

```sql
CREATE DATABASE hrmsystem;
```

2. Cập nhật connection string trong `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=hrmsystem;Username=your_username;Password=your_password"
  }
}
```

3. Cập nhật JWT key trong `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyHere_MakeItLongAndSecure123456789",
    "Issuer": "HRMSystem",
    "Audience": "HRMSystemUsers"
  }
}
```

#### Chạy ứng dụng

1. Khôi phục packages:

```bash
dotnet restore
```

2. Tạo và áp dụng migration:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

3. Chạy ứng dụng:

```bash
dotnet run
```

Ứng dụng sẽ chạy tại `https://localhost:5001` và `http://localhost:5000`.

## Sử dụng hệ thống

### 🚀 Chạy ứng dụng

1. **Build và chạy:**

```bash
dotnet build
dotnet run
```

2. **Truy cập:**
   - Mở trình duyệt và truy cập: `https://localhost:5001`
   - Ứng dụng sẽ tự động migrate database và tạo tài khoản admin

### 👤 Tài khoản mặc định

- **Email:** admin@hrsystem.com
- **Mật khẩu:** Admin123!
- **Vai trò:** Administrator

### 🖥️ Giao diện Web

Hệ thống cung cấp giao diện web hoàn chỉnh:

1. **Trang đăng nhập** (`index.html`): Đăng nhập vào hệ thống
2. **Dashboard** (`dashboard.html`): Quản lý nhân viên với các tính năng:
   - Xem danh sách nhân viên
   - Thêm nhân viên mới
   - Sửa thông tin nhân viên
   - Xóa nhân viên
   - Làm mới dữ liệu

### 📡 API Endpoints

### Đăng ký tài khoản

```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "admin@example.com",
  "password": "Password123!",
  "confirmPassword": "Password123!",
  "firstName": "Admin",
  "lastName": "User"
}
```

### Đăng nhập

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@example.com",
  "password": "Password123!"
}
```

Sử dụng token JWT trong header Authorization cho các API khác:

```
Authorization: Bearer YOUR_JWT_TOKEN
```

## 🐳 Chạy với Docker

### Yêu cầu hệ thống

- Docker Desktop hoặc Docker Engine
- Docker Compose

### Chạy với Docker Compose

1. **Clone repository và chuyển vào thư mục:**

```bash
git clone <repository-url>
cd hrm-system
```

2. **Chạy ứng dụng:**

```bash
docker-compose up -d
```

3. **Truy cập ứng dụng:**

   - Web UI: `http://localhost:8080`
   - API Documentation: `http://localhost:8080/swagger`

4. **Dừng ứng dụng:**

```bash
docker-compose down
```

### Tài khoản mặc định

- **Email:** admin@hrsystem.com
- **Mật khẩu:** Admin123!

### Docker Commands hữu ích

```bash
# Xem logs
docker-compose logs -f

# Restart services
docker-compose restart

# Rebuild và chạy lại
docker-compose up --build

# Dọn dẹp
docker-compose down -v  # Xóa volumes
docker system prune     # Dọn dẹp Docker
```

### Chạy riêng lẻ với Docker

```bash
# Build image
docker build -t hrmsystem .

# Chạy PostgreSQL
docker run -d --name postgres -e POSTGRES_DB=HRMSystem -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 postgres:15-alpine

# Chạy ứng dụng
docker run -d --name hrm-app -p 8080:8080 --link postgres hrmsystem
```

### API Nhân viên

#### Lấy danh sách nhân viên

```http
GET /api/employees
Authorization: Bearer YOUR_JWT_TOKEN
```

#### Tạo nhân viên mới

```http
POST /api/employees
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json

{
  "firstName": "Nguyễn",
  "lastName": "Văn A",
  "email": "nguyenvana@example.com",
  "phone": "0123456789",
  "position": "Developer",
  "salary": 15000000,
  "hireDate": "2024-01-15T00:00:00Z"
}
```

#### Cập nhật nhân viên

```http
PUT /api/employees/{id}
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json

{
  "firstName": "Nguyễn",
  "lastName": "Văn B",
  "email": "nguyenvanb@example.com",
  "phone": "0123456789",
  "position": "Senior Developer",
  "salary": 20000000,
  "hireDate": "2024-01-15T00:00:00Z",
  "isActive": true
}
```

#### Xóa nhân viên

```http
DELETE /api/employees/{id}
Authorization: Bearer YOUR_JWT_TOKEN
```

## Cấu trúc dự án

```
HRMSystem/
├── Controllers/
│   ├── AuthController.cs          # API xác thực
│   └── EmployeesController.cs     # API quản lý nhân viên
├── Data/
│   └── ApplicationDbContext.cs    # Database context
├── DTOs/
│   ├── AuthDto.cs                 # DTO cho authentication
│   └── EmployeeDto.cs             # DTO cho employee
├── Models/
│   ├── Employee.cs                # Model nhân viên
│   └── User.cs                    # Model người dùng
├── appsettings.json               # Cấu hình ứng dụng
├── Program.cs                     # Entry point
└── HRMSystem.csproj               # Project file
```

## Lưu ý bảo mật

- Thay đổi JWT key trong production
- Sử dụng HTTPS trong production
- Cấu hình CORS theo domain cụ thể
- Validate input dữ liệu đầy đủ
- Sử dụng rate limiting cho API

## Phát triển thêm

- Thêm role-based authorization
- Upload ảnh đại diện nhân viên
- Export dữ liệu ra Excel
- Dashboard thống kê
- Email notification
- Audit logging
