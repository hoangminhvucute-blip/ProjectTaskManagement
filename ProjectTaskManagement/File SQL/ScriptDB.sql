IF DB_ID('ProjectTaskDB') IS NULL
BEGIN
    CREATE DATABASE ProjectTaskDB;
END
GO

USE ProjectTaskDB;
GO

IF OBJECT_ID('dbo.PhanCong', 'U') IS NOT NULL DROP TABLE dbo.PhanCong;
IF OBJECT_ID('dbo.CongViec', 'U') IS NOT NULL DROP TABLE dbo.CongViec;
IF OBJECT_ID('dbo.ThanhVien', 'U') IS NOT NULL DROP TABLE dbo.ThanhVien;
IF OBJECT_ID('dbo.DuAn', 'U') IS NOT NULL DROP TABLE dbo.DuAn;
GO

-- 1. Bảng Dự án
CREATE TABLE dbo.DuAn (
    MaDuAn INT IDENTITY(1,1) PRIMARY KEY,
    TenDuAn NVARCHAR(150) NOT NULL,
    NgayBatDau DATETIME NOT NULL DEFAULT GETDATE(),
    NgayKetThuc DATETIME NULL,
    TrangThai NVARCHAR(50) NOT NULL DEFAULT N'Đang chạy'
);
GO

-- 2. Bảng Thành viên nhóm
CREATE TABLE dbo.ThanhVien (
    MaThanhVien INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    ChuyenMon NVARCHAR(100) NULL
);
GO

-- 3. Bảng Công việc / Task
CREATE TABLE dbo.CongViec (
    MaTask INT IDENTITY(1,1) PRIMARY KEY,
    MaDuAn INT NOT NULL,
    TenTask NVARCHAR(200) NOT NULL,
    MoTa NVARCHAR(MAX) NULL,
    HanNop DATETIME NOT NULL,
    DoUuTien NVARCHAR(20) NOT NULL DEFAULT N'Trung bình',
    TrangThai NVARCHAR(50) NOT NULL DEFAULT N'To Do',
    CONSTRAINT FK_CongViec_DuAn FOREIGN KEY (MaDuAn)
        REFERENCES dbo.DuAn(MaDuAn) ON DELETE CASCADE
);
GO

-- 4. Bảng Phân công công việc
CREATE TABLE dbo.PhanCong (
    MaTask INT NOT NULL,
    MaThanhVien INT NOT NULL,
    NgayGiao DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_PhanCong PRIMARY KEY (MaTask, MaThanhVien),
    CONSTRAINT FK_PhanCong_CongViec FOREIGN KEY (MaTask)
        REFERENCES dbo.CongViec(MaTask) ON DELETE CASCADE,
        CONSTRAINT FK_PhanCong_ThanhVien FOREIGN KEY (MaThanhVien)
        REFERENCES dbo.ThanhVien(MaThanhVien) ON DELETE CASCADE
);
GO

-- Dữ liệu mẫu
INSERT INTO dbo.DuAn (TenDuAn, NgayBatDau, NgayKetThuc, TrangThai) VALUES
(N'Hệ thống Quản lý Bán hàng Đa kênh', '2026-10-01', '2026-12-20', N'Đang chạy'),
(N'Ứng dụng Theo dõi Sức khỏe Cá nhân', '2026-09-15', '2026-11-10', N'Đang chạy'),
(N'Website Giới thiệu Doanh nghiệp', '2026-08-01', '2026-09-30', N'Hoàn thành');

INSERT INTO dbo.ThanhVien (HoTen, Email, ChuyenMon) VALUES
(N'Nguyễn Minh Anh', 'minhanh.dev@gmail.com', N'Backend .NET & SQL'),
(N'Trần Hoàng Nam', 'hoangnam.fe@gmail.com', N'Frontend & WinForms UI'),
(N'Lê Thu Thảo', 'thuthao.qa@gmail.com', N'QC / Tester & BA'),
(N'Phạm Đức Huy', 'duchuy.mobile@gmail.com', N'Mobile & API');

INSERT INTO dbo.CongViec (MaDuAn, TenTask, MoTa, HanNop, DoUuTien, TrangThai) VALUES
(1, N'Thiết kế CSDL và Stored Procedure', N'Tạo bảng sản phẩm, đơn hàng, tồn kho', '2026-10-15', N'Cao', N'Done'),
(1, N'Xây dựng API đăng nhập & phân quyền', N'JWT authentication & role check', '2026-11-05', N'Cao', N'In Progress'),
(1, N'Giao diện quản lý hóa đơn', N'Form tìm kiếm, in hóa đơn', '2026-11-01', N'Trung bình', N'To Do'),
(1, N'Kiểm thử hiệu năng giỏ hàng', N'Test 500 concurrent users', '2026-11-25', N'Thấp', N'Review'),
(2, N'Tích hợp cảm biến bước chân', N'Đọc dữ liệu sức khỏe từ thiết bị', '2026-10-20', N'Cao', N'Done'),
(2, N'Biểu đồ thống kê calo hàng tuần', N'Vẽ biểu đồ theo ngày và tuần', '2026-11-02', N'Trung bình', N'In Progress'),
(3, N'Tối ưu SEO & tốc độ tải trang', N'Nén ảnh, cấu hình meta tags', '2026-09-25', N'Trung bình', N'Done');

INSERT INTO dbo.PhanCong (MaTask, MaThanhVien, NgayGiao) VALUES
(1, 1, '2026-10-02'),
(2, 1, '2026-10-16'),
(2, 4, '2026-10-16'),
(3, 2, '2026-10-18'),
(4, 3, '2026-10-20'),
(5, 4, '2026-09-16'),
(6, 2, '2026-10-22'),
(7, 2, '2026-08-05'),
(7, 3, '2026-08-06');
GO
