-- ========================================================
-- SCRIPT TẠO CSDL VÀ DỮ LIỆU MẪU CHO BÀI 6.1: SalesDB
-- HỆ THỐNG QUẢN LÝ BÁN HÀNG & SIÊU THỊ MINI (RETAIL SALES SYSTEM)
-- ========================================================

USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'SalesDB')
BEGIN
    ALTER DATABASE SalesDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE SalesDB;
END
GO

CREATE DATABASE SalesDB;
GO

USE SalesDB;
GO

-- 1. Bảng NhanVien (Nhân viên bán hàng)
CREATE TABLE NhanVien (
    MaNV INT IDENTITY(1,1) PRIMARY KEY,
    TenNV NVARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(15) UNIQUE,
    ChucVu NVARCHAR(50) -- Ví dụ: 'Thu ngân', 'Quản lý'
);
GO

-- 2. Bảng KhachHang (Khách hàng)
CREATE TABLE KhachHang (
    MaKH INT IDENTITY(1,1) PRIMARY KEY,
    TenKH NVARCHAR(100) NOT NULL,
    DienThoai VARCHAR(15) UNIQUE,
    DiemTichLuy INT DEFAULT 0
);
GO

-- 3. Bảng SanPham (Sản phẩm / Hàng hóa)
CREATE TABLE SanPham (
    MaSP VARCHAR(20) PRIMARY KEY,
    TenSP NVARCHAR(150) NOT NULL,
    GiaBan DECIMAL(18,2) CHECK (GiaBan > 0),
    SoLuongTon INT CHECK (SoLuongTon >= 0)
);
GO

-- 4. Bảng HoaDon (Hóa đơn bán hàng)
CREATE TABLE HoaDon (
    MaHD INT IDENTITY(1,1) PRIMARY KEY,
    NgayLap DATETIME DEFAULT GETDATE(),
    MaNV INT FOREIGN KEY REFERENCES NhanVien(MaNV),
    MaKH INT NULL FOREIGN KEY REFERENCES KhachHang(MaKH),
    TongTien DECIMAL(18,2) DEFAULT 0
);
GO

-- 5. Bảng ChiTietHoaDon (Chi tiết hóa đơn)
CREATE TABLE ChiTietHoaDon (
    MaHD INT FOREIGN KEY REFERENCES HoaDon(MaHD),
    MaSP VARCHAR(20) FOREIGN KEY REFERENCES SanPham(MaSP),
    SoLuong INT CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2),
    PRIMARY KEY (MaHD, MaSP)
);
GO

-- ========================================================
-- DỮ LIỆU MẪU (SEED DATA)
-- ========================================================

-- Chèn Nhân viên
INSERT INTO NhanVien (TenNV, SoDienThoai, ChucVu) VALUES
(N'Nguyễn Văn An', '0901234567', N'Quản lý'),
(N'Trần Thị Bích', '0912345678', N'Thu ngân'),
(N'Lê Hoàng Nam', '0987654321', N'Thu ngân'),
(N'Phạm Thu Hà', '0978123456', N'Bán hàng');
GO

-- Chèn Khách hàng
INSERT INTO KhachHang (TenKH, DienThoai, DiemTichLuy) VALUES
(N'Đỗ Minh Trí', '0933111222', 15),
(N'Vũ Mai Phương', '0944222333', 30),
(N'Hoàng Quốc Bảo', '0966555444', 5),
(N'Bùi Thu Trang', '0977888999', 0);
GO

-- Chèn Sản phẩm
INSERT INTO SanPham (MaSP, TenSP, GiaBan, SoLuongTon) VALUES
('SP01', N'Sữa tươi tiệt trùng Vinamilk 1L', 38000, 150),
('SP02', N'Mì tôm Hảo Hảo tôm chua cay (Thùng 30 gói)', 125000, 80),
('SP03', N'Dầu ăn Simply đậu nành 1L', 65000, 100),
('SP04', N'Gạo ST25 Ông Cua 5kg', 185000, 45),
('SP05', N'Nước ngọt Coca Cola lon 330ml', 10000, 250),
('SP06', N'Bánh Chocopie hộp 12 cái', 55000, 60),
('SP07', N'Cà phê hòa tan G7 3in1 (Hộp 20 gói)', 62000, 90),
('SP08', N'Nước giặt OMO Matic túi 3.6kg', 210000, 35),
('SP09', N'Trứng gà Ba Huân hộp 10 quả', 32000, 75),
('SP10', N'Socola Ferrero Rocher 16 viên (Hàng tồn ít)', 290000, 2); -- Để test tồn kho ít
GO

-- Chèn Hóa đơn lịch sử
INSERT INTO HoaDon (NgayLap, MaNV, MaKH, TongTien) VALUES
(DATEADD(DAY, -2, GETDATE()), 2, 1, 311000),
(DATEADD(DAY, -1, GETDATE()), 3, 2, 420000),
(GETDATE(), 2, 3, 195000);
GO

-- Chèn Chi tiết hóa đơn
INSERT INTO ChiTietHoaDon (MaHD, MaSP, SoLuong, DonGia) VALUES
(1, 'SP01', 2, 38000),
(1, 'SP02', 1, 125000),
(1, 'SP03', 1, 65000),
(1, 'SP05', 4, 10000),
(2, 'SP08', 2, 210000),
(3, 'SP04', 1, 185000),
(3, 'SP05', 1, 10000);
GO
