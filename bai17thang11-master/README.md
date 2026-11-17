# TỔNG HỢP BÀI TẬP LỚN C#: BÀI 6.1 & BÀI 6.2
### KIẾN TRÚC 3-TIER, GIAO DỊCH SQLTRANSACTION (ACID) & DISCONNECTED MODEL

---

## THÔNG TIN SINH VIÊN
* **Họ và tên sinh viên:** Bùi Hoàng Gia Huy
* **Mã sinh viên:** 24810310254
* **Lớp / Khóa:** D19CNPM3
* **Môi trường phát triển:** .NET 8.0 Windows Forms, C#, Microsoft SQL Server 2022 (SQLEXPRESS), ADO.NET

---

## CẤU TRÚC THƯ MỤC NỘP BÀI TRÊN GITHUB

Toàn bộ dự án đã được phân chia rõ ràng thành **2 thư mục độc lập**, mỗi bài một thư mục chứa đầy đủ CSDL, mã nguồn 3-Tier, file Solution, thư mục ảnh chụp thực tế và file `README.md` báo cáo:

```
d:\bai17thang11/
├── Bai6_1_RetailSalesSystem/     # BÀI 6.1: HỆ THỐNG QUẢN LÝ BÁN HÀNG & SIÊU THỊ MINI
│   ├── Database/
│   │   └── ScriptDB.sql          # Script CSDL SalesDB và dữ liệu mẫu
│   ├── RetailSalesSystem/        # 3-Tier: DTO - DAL - BUS - GUI
│   ├── images/                   # 5 ảnh chụp kết quả màn hình chức năng
│   │   ├── 01_main_screen.png
│   │   ├── 02_crud_screen.png
│   │   ├── 03_transaction_screen.png
│   │   ├── 04_report_screen.png
│   │   └── 05_error_handling.png
│   ├── RetailSalesSystem.sln     # Solution mở Bài 6.1
│   └── README.md                 # Báo cáo chi tiết Bài 6.1
│
├── Bai6_2_HRMPayrollSystem/      # BÀI 6.2: HỆ THỐNG QUẢN LÝ NHÂN SỰ & TÍNH LƯƠNG
│   ├── Database/
│   │   └── ScriptDB.sql          # Script CSDL HRMDB và dữ liệu mẫu
│   ├── HRMPayrollSystem/         # 3-Tier: DTO - DAL - BUS - GUI
│   ├── images/                   # 5 ảnh chụp kết quả màn hình chức năng
│   │   ├── 01_main_screen.png
│   │   ├── 02_crud_screen.png
│   │   ├── 03_transaction_screen.png
│   │   ├── 04_report_screen.png
│   │   └── 05_error_handling.png
│   ├── HRMPayrollSystem.sln      # Solution mở Bài 6.2
│   └── README.md                 # Báo cáo chi tiết Bài 6.2
│
├── .gitignore                    # Bỏ qua các file rác build Visual Studio/.NET
└── README.md                     # Báo cáo tổng hợp
```

---

## TÓM TẮT ĐẶC TẢ VÀ TRUY CẬP TỪNG BÀI

### 1. [Bài 6.1: Hệ Thống Bán Hàng & Siêu Thị Mini (Retail Sales System)](Bai6_1_RetailSalesSystem/README.md)
* **Thư mục:** `Bai6_1_RetailSalesSystem/`
* **Cơ sở dữ liệu:** `SalesDB` (gồm 5 bảng `NhanVien`, `KhachHang`, `SanPham`, `HoaDon`, `ChiTietHoaDon`).
* **Nghiệp vụ cốt lõi:**
  * Thực thi giao dịch `SqlTransaction` xử lý tạo hóa đơn và trừ tồn kho tự động (đảm bảo tính toàn vẹn dữ liệu ACID). Nếu tồn kho không đủ $\rightarrow$ lập tức gọi `trans.Rollback()` và báo lỗi.
  * Tự động cộng điểm tích lũy: $\text{Điểm cộng} = \lfloor \text{Tổng tiền} / 100,000 \rfloor$.
  * Áp dụng kiến trúc 3-Tier và phòng chống 100% lỗ hổng SQL Injection bằng `SqlParameter` có ký tự đại diện `%`.
  * Báo cáo thống kê: `ExecuteScalar` tính tổng doanh thu trong ngày/tháng, `SqlDataReader` hiển thị Top 5 sản phẩm bán chạy nhất.
* **Chi tiết báo cáo & hình ảnh:** Xem tại [Bai6_1_RetailSalesSystem/README.md](Bai6_1_RetailSalesSystem/README.md).

---

### 2. [Bài 6.2: Hệ Thống Quản Lý Nhân Sự & Tính Lương (HRM & Payroll System)](Bai6_2_HRMPayrollSystem/README.md)
* **Thư mục:** `Bai6_2_HRMPayrollSystem/`
* **Cơ sở dữ liệu:** `HRMDB` (gồm 4 bảng `PhongBan`, `NhanVien`, `BangChamCong`, `BangLuong`).
* **Nghiệp vụ cốt lõi:**
  * Quản lý hồ sơ nhân sự 3-Tier CRUD, chuyển phòng ban, tìm kiếm theo tên/CCCD/phòng ban với Parameterized Query.
  * Mô hình ngắt kết nối (**Disconnected Model**): Sử dụng `SqlDataAdapter`, `DataTable`, `SqlCommandBuilder` và `adapter.Update()` để nạp và đồng bộ bảng chấm công hàng tháng trong 1 thao tác.
  * Công thức tính lương:
    $$\text{Thực Lĩnh} = \left(\frac{\text{Lương Cơ Bản} \times \text{Hệ Số Lương}}{22}\right) \times \text{Số Ngày Công} + (\text{Số Giờ OT} \times 50,000)$$
  * Giao dịch `SqlTransaction` chốt lương: Ghi nhận bảng lương cho toàn bộ nhân viên có trong tháng và khóa dữ liệu chấm công (`DaKhoa = 1`).
* **Chi tiết báo cáo & hình ảnh:** Xem tại [Bai6_2_HRMPayrollSystem/README.md](Bai6_2_HRMPayrollSystem/README.md).

---

## HƯỚNG DẪN KHỞI CHẠY TỪ DÒNG LỆNH

```powershell
# Chạy Bài 6.1 (Retail Sales System)
dotnet run --project Bai6_1_RetailSalesSystem\RetailSalesSystem\RetailSalesSystem.csproj

# Chạy Bài 6.2 (HRM & Payroll System)
dotnet run --project Bai6_2_HRMPayrollSystem\HRMPayrollSystem\HRMPayrollSystem.csproj
```
