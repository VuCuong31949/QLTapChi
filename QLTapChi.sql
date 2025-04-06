CREATE DATABASE QLTapChi;
USE QLTapChi;

-- Bảng Lĩnh Vực
CREATE TABLE LinhVuc (
    IDLinhVuc INT IDENTITY(1,1) PRIMARY KEY,
    TenLinhVuc NVARCHAR(100) NOT NULL
);
GO

-- Bảng Vai Trò
CREATE TABLE VaiTro (
    IDVaiTro INT IDENTITY(1,1) PRIMARY KEY,
    TenVaiTro NVARCHAR(100) NOT NULL
);
GO

-- Bảng Người Dùng
CREATE TABLE NguoiDung (
    IDNguoiDung INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(300) NOT NULL,
    Email VARCHAR(200) NOT NULL,
    MatKhau VARCHAR(70) NOT NULL,
    SDT VARCHAR(15) NOT NULL,
    DiaChi NVARCHAR(300) NOT NULL,
    QuocGia NVARCHAR(50) NOT NULL,
	ChucDanh nvarchar(100),
	GioiTinh int,
	ToChuc nvarchar(100),
    IDLinhVuc INT NULL,
    FOREIGN KEY (IDLinhVuc) REFERENCES LinhVuc(IDLinhVuc)
);
GO

-- Bảng Biên Tập Viên
CREATE TABLE BienTapVien (
    IDBienTapVien INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(300) NOT NULL,
    Email VARCHAR(200) NOT NULL,
    MatKhau VARCHAR(70) NOT NULL,
    SDT VARCHAR(15) NOT NULL,
    DiaChi NVARCHAR(300) NOT NULL,
    QuocGia NVARCHAR(50) NOT NULL,
    ChuyenNganh NVARCHAR(100) NOT NULL
);
GO

-- Bảng Người Dùng - Vai Trò
CREATE TABLE NguoiDung_VaiTro (
    IDNguoiDung INT NOT NULL,
    IDVaiTro INT NOT NULL,
    PRIMARY KEY (IDNguoiDung, IDVaiTro),
    FOREIGN KEY (IDNguoiDung) REFERENCES NguoiDung(IDNguoiDung),
    FOREIGN KEY (IDVaiTro) REFERENCES VaiTro(IDVaiTro)
);
GO

-- Bảng Tạp Chí - Bài Viết
CREATE TABLE TapChiBaiViet (
    IDTapChiBaiViet INT IDENTITY(1,1) PRIMARY KEY,
    TieuDe NVARCHAR(200) NOT NULL,
    TacGia NVARCHAR(300) NOT NULL,
    NoiDung VARCHAR(100) NOT NULL,
    IDLinhVuc INT NOT NULL,
    TrangThai INT DEFAULT 0,  -- 0: Chờ duyệt, 1: Đã duyệt, 2: Xuất bản
    NgayGui DATE NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (IDLinhVuc) REFERENCES LinhVuc(IDLinhVuc)
);
GO

-- Bảng Phản Biện
CREATE TABLE PhanBien (
    IDPhanBien INT IDENTITY(1,1) PRIMARY KEY,
    NhanXet NVARCHAR(500) NOT NULL,
    NgayPhanBien DATE NOT NULL DEFAULT GETDATE(),
    IDTapChiBaiViet INT NOT NULL,
    IDNguoiPhanBien INT NOT NULL,
    FOREIGN KEY (IDTapChiBaiViet) REFERENCES TapChiBaiViet(IDTapChiBaiViet),
    FOREIGN KEY (IDNguoiPhanBien) REFERENCES NguoiDung(IDNguoiDung)
);
GO

-- Bảng Phân Công (Phân công phản biện tạp chí)
CREATE TABLE PhanCong (
    IDPhanCong INT IDENTITY(1,1) PRIMARY KEY,
    NgayPhanCong DATE NOT NULL DEFAULT GETDATE(),
    NgayKetThuc DATE NULL,
    IDTapChiBaiViet INT NOT NULL,
    IDNguoiPhanBien INT NOT NULL,
    FOREIGN KEY (IDTapChiBaiViet) REFERENCES TapChiBaiViet(IDTapChiBaiViet),
    FOREIGN KEY (IDNguoiPhanBien) REFERENCES NguoiDung(IDNguoiDung)
);
GO


-- Bảng Xuất Bản Tạp Chí
CREATE TABLE XuatBan (
    IDXuatBan INT IDENTITY(1,1) PRIMARY KEY,
    SoTapChi NVARCHAR(50) NOT NULL,
    NgayXuatBan DATE NOT NULL DEFAULT GETDATE(),
    IDTapChiBaiViet INT NOT NULL,
    IDBienTapVien INT NOT NULL,
    FOREIGN KEY (IDTapChiBaiViet) REFERENCES TapChiBaiViet(IDTapChiBaiViet),
    FOREIGN KEY (IDBienTapVien) REFERENCES BienTapVien(IDBienTapVien)
);

CREATE TABLE PhanCongBienTap (
    IDPhanCongBienTap INT IDENTITY(1,1) PRIMARY KEY,
    IDBienTapVien INT NOT NULL,
    IDTapChiBaiViet INT NOT NULL,
    NgayPhanCong DATE NOT NULL DEFAULT GETDATE(),
    GhiChu NVARCHAR(300) NULL,

    FOREIGN KEY (IDBienTapVien) REFERENCES BienTapVien(IDBienTapVien),
    FOREIGN KEY (IDTapChiBaiViet) REFERENCES TapChiBaiViet(IDTapChiBaiViet)
);
ALTER TABLE BienTapVien
ADD LoaiBienTapVien NVARCHAR(50);  -- 'Tong' hoặc 'PhuTrach'

GO
-- Chèn dữ liệu vào bảng VaiTro
INSERT INTO VaiTro (TenVaiTro) VALUES (N'Tác Giả');
INSERT INTO VaiTro (TenVaiTro) VALUES (N'Phản Biện');

-- Kiểm tra dữ liệu đã được thêm vào chưa
SELECT * FROM VaiTro;
select * from NguoiDung
alter table NguoiDung add PhanBien bit
alter table TapChiBaiViet add TuKhoa nvarchar(300)
select * from TapChiBaiViet
ALTER TABLE TapChiBaiViet
ALTER COLUMN NoiDung NVARCHAR(MAX);
