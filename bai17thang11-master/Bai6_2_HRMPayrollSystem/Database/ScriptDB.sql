-- ========================================================
-- SCRIPT TẠO CSDL VÀ DỮ LIỆU MẪU CHO BÀI 6.2: HRMDB
-- HỆ THỐNG QUẢN LÝ NHÂN SỰ & TÍNH LƯƠNG (HRM & PAYROLL SYSTEM)
-- ========================================================

USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'HRMDB')
BEGIN
    ALTER DATABASE HRMDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE HRMDB;
END
GO

CREATE DATABASE HRMDB;
GO

USE HRMDB;
GO

-- 1. Bảng PhongBan (Phòng ban)
CREATE TABLE PhongBan (
    MaPB INT IDENTITY(1,1) PRIMARY KEY,
    TenPB NVARCHAR(100) NOT NULL,
    Mota NVARCHAR(255)
);
GO

-- 2. Bảng NhanVien (Hồ sơ nhân viên)
CREATE TABLE NhanVien (
    MaNV INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATETIME,
    CCCD VARCHAR(20) UNIQUE,
    MaPB INT FOREIGN KEY REFERENCES PhongBan(MaPB),
    LuongCoBan DECIMAL(18,2) CHECK (LuongCoBan > 0),
    HeSoLuong FLOAT DEFAULT 1.0
);
GO

-- 3. Bảng BangChamCong (Bảng chấm công tháng)
CREATE TABLE BangChamCong (
    MaCong INT IDENTITY(1,1) PRIMARY KEY,
    MaNV INT FOREIGN KEY REFERENCES NhanVien(MaNV),
    ThangNam VARCHAR(7), -- Ví dụ: '11/2026'
    SoNgayCong FLOAT CHECK (SoNgayCong <= 31),
    SoGioOT FLOAT DEFAULT 0,
    DaKhoa BIT DEFAULT 0,
    CONSTRAINT UQ_BangChamCong UNIQUE (MaNV, ThangNam)
);
GO

-- 4. Bảng BangLuong (Bảng lương chi tiết)
CREATE TABLE BangLuong (
    MaLuong INT IDENTITY(1,1),
    MaNV INT FOREIGN KEY REFERENCES NhanVien(MaNV),
    ThangNam VARCHAR(7),
    ThucLinh DECIMAL(18,2),
    PRIMARY KEY (MaNV, ThangNam)
);
GO

-- ========================================================
-- DỮ LIỆU MẪU (SEED DATA)
-- ========================================================

-- Chèn Phòng Ban
INSERT INTO PhongBan (TenPB, Mota) VALUES
(N'Phòng Giám Đốc', N'Ban điều hành công ty'),
(N'Phòng Kỹ Thuật', N'Nghiên cứu & phát triển phần mềm'),
(N'Phòng Kế Toán', N'Quản lý thu chi và tài chính'),
(N'Phòng Nhân Sự', N'Tuyển dụng và đào tạo nhân sự'),
(N'Phòng Kinh Doanh', N'Phát triển thị trường và bán hàng');
GO

-- Chèn Nhân viên
INSERT INTO NhanVien (HoTen, NgaySinh, CCCD, MaPB, LuongCoBan, HeSoLuong) VALUES
(N'Nguyễn Văn Hùng', '1988-04-12', '001088001234', 1, 25000000, 2.5),
(N'Trần Minh Tuấn', '1995-08-20', '001095002345', 2, 18000000, 1.8),
(N'Lê Thị Hương', '1997-12-15', '001097003456', 2, 15000000, 1.4),
(N'Phạm Hoàng Lan', '1993-06-18', '001093004567', 3, 14000000, 1.5),
(N'Vũ Đức Anh', '1999-02-28', '001099005678', 4, 12000000, 1.2),
(N'Bùi Quỳnh Nga', '1996-10-10', '001096006789', 5, 13500000, 1.3);
GO

-- Chèn Bảng chấm công mẫu cho tháng 10/2026 (Đã chốt lương)
INSERT INTO BangChamCong (MaNV, ThangNam, SoNgayCong, SoGioOT, DaKhoa) VALUES
(1, '10/2026', 22, 5, 1),
(2, '10/2026', 21.5, 12, 1),
(3, '10/2026', 22, 8, 1),
(4, '10/2026', 20, 2, 1),
(5, '10/2026', 22, 0, 1),
(6, '10/2026', 19, 6, 1);
GO

-- Chèn Bảng lương tháng 10/2026
-- ThucLinh = (LuongCoBan * HeSoLuong / 22) * SoNgayCong + (SoGioOT * 50,000)
INSERT INTO BangLuong (MaNV, ThangNam, ThucLinh) VALUES
(1, '10/2026', ROUND((25000000.0 * 2.5 / 22.0) * 22.0 + (5 * 50000), 2)),
(2, '10/2026', ROUND((18000000.0 * 1.8 / 22.0) * 21.5 + (12 * 50000), 2)),
(3, '10/2026', ROUND((15000000.0 * 1.4 / 22.0) * 22.0 + (8 * 50000), 2)),
(4, '10/2026', ROUND((14000000.0 * 1.5 / 22.0) * 20.0 + (2 * 50000), 2)),
(5, '10/2026', ROUND((12000000.0 * 1.2 / 22.0) * 22.0 + (0 * 50000), 2)),
(6, '10/2026', ROUND((13500000.0 * 1.3 / 22.0) * 19.0 + (6 * 50000), 2));
GO

-- Chèn Bảng chấm công mẫu cho tháng 11/2026 (Chưa chốt lương để kiểm thử trên ứng dụng)
INSERT INTO BangChamCong (MaNV, ThangNam, SoNgayCong, SoGioOT, DaKhoa) VALUES
(1, '11/2026', 22, 4, 0),
(2, '11/2026', 21, 15, 0),
(3, '11/2026', 22, 10, 0),
(4, '11/2026', 21.5, 4, 0),
(5, '11/2026', 20, 2, 0),
(6, '11/2026', 22, 8, 0);
GO
