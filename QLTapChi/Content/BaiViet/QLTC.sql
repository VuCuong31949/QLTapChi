create database dbQuanLyTiecCuoi2


CREATE TABLE [dbo].[DichVu] (
    [MaDV]   INT             IDENTITY (1, 1) NOT NULL,
    [TenDV]  NVARCHAR (100)  NOT NULL,
    [LoaiDV] NVARCHAR (100)  NULL,
    [GhiChu] NVARCHAR (300)  NULL,
    [MoTa]   NVARCHAR (200)  NULL,
    [Gia]    DECIMAL (18, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([MaDV] ASC)
);

CREATE TABLE [dbo].[DoCuoi] (
    [MaDC]       INT             IDENTITY (1, 1) NOT NULL,
    [DanhMuc]    NVARCHAR (100)  NULL,
    [MoTa]       NVARCHAR (200)  NULL,
    [HinhAnh]    VARCHAR (200)   NULL,
    [KichThuoc]  NVARCHAR (100)  NULL,
    [ChatLieu]   NVARCHAR (100)  NULL,
    [MauSac]     NVARCHAR (100)  NULL,
    [NhaCungCap] NVARCHAR (200)  NULL,
    [Gia]        DECIMAL (18, 2) NOT NULL,
    [KhuyenMai]  DECIMAL (18, 2) NULL,
    [HinhThuc]   NVARCHAR (100)  NULL,
    [LoaiSanh]   NVARCHAR (100)  NULL,
    [LuotMua]    INT             NULL,
    PRIMARY KEY CLUSTERED ([MaDC] ASC)
);

CREATE TABLE [dbo].[DungCu] (
    [MaDC]        INT             IDENTITY (1, 1) NOT NULL,
    [TenDC]       NVARCHAR (100)  NOT NULL,
    [LoaiDC]      NVARCHAR (100)  NOT NULL,
    [SoLuongNhap] INT             NOT NULL,
    [SoLuongHu]   INT             NOT NULL,
    [SoLuongTon]  INT             NOT NULL,
    [HinhAnh]     VARCHAR (100)   NULL,
    [GhiChu]      NVARCHAR (300)  NULL,
    [Gia]         DECIMAL (18, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([MaDC] ASC)
);

CREATE TABLE [dbo].[KhachMoi] (
    [MaKM]         INT            IDENTITY (1, 1) NOT NULL,
    [TenKM]        NVARCHAR (100) NOT NULL,
    [SDT]          VARCHAR (15)   NULL,
    [DiaChi]       NVARCHAR (200) NULL,
    [NhomKhachMoi] NVARCHAR (200) NULL,
    [SoLuong]      INT            NOT NULL,
    [GhiChu]       NVARCHAR (300) NULL,
    [LoaiThiep]    NVARCHAR (100) NULL,
    [TenSanh]      NVARCHAR (100) NULL,
    [DiaChiSanh]   NVARCHAR (200) NULL,
    PRIMARY KEY CLUSTERED ([MaKM] ASC)
);

CREATE TABLE [dbo].[MonAn] (
    [MaMA]      INT            IDENTITY (1, 1) NOT NULL,
    [TenMA]     NVARCHAR (100) NOT NULL,
    [DanhMuc]   NVARCHAR (100) NULL,
    [GiaGoc]    DECIMAL (18)   NOT NULL,
    [LoiNhuan]  INT            NOT NULL,
    [GiaTong]   DECIMAL (18)   NOT NULL,
    [MoTa]      NVARCHAR (200) NULL,
    [TrangThai] NVARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([MaMA] ASC)
);

CREATE TABLE [dbo].[NguyenLieu] (
    [MaNL]       INT            IDENTITY (1, 1) NOT NULL,
    [TenNL]      NVARCHAR (100) NOT NULL,
    [DanhMuc]    NVARCHAR (100) NULL,
    [NhaCungCap] NVARCHAR (100) NULL,
    [SoLuong]    INT            NOT NULL,
    [DonVi]      NVARCHAR (100) NULL,
    [NgayNhap]   NVARCHAR (100) NOT NULL,
    [GiaNhap]    DECIMAL (18)   NOT NULL,
    [HanSuDung]  NVARCHAR (100) NOT NULL,
    [TrangThai]  NVARCHAR (100) NULL,
    [GhiChu]     NVARCHAR (300) NULL,
    PRIMARY KEY CLUSTERED ([MaNL] ASC)
);

CREATE TABLE [dbo].[SanhCuoi] (
    [MaSC]         INT             IDENTITY (1, 1) NOT NULL,
    [TenSC]        NVARCHAR (100)  NOT NULL,
    [SoLuongBan]   INT             NOT NULL,
    [DienTich]     NVARCHAR (100)  NULL,
    [LoaiSanh]     NVARCHAR (100)  NULL,
    [LoaiDen]      NVARCHAR (100)  NULL,
    [SoLuongDen]   INT             NULL,
    [LoaiMayLanh]  NVARCHAR (100)  NULL,
    [SLMayLanh]    INT             NULL,
    [LoaiMayChieu] NVARCHAR (100)  NULL,
    [LoaiBackdrop] NVARCHAR (100)  NULL,
    [LoaiManHinh]  NVARCHAR (100)  NULL,
    [TongChiPhi]   DECIMAL (18, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([MaSC] ASC)
);

CREATE TABLE [dbo].[TaiKhoan] (
    [MaTK]    INT            IDENTITY (1, 1) NOT NULL,
    [TenTK]   NVARCHAR (100) NOT NULL,
    [MatKhau] VARCHAR (100)  NOT NULL,
    PRIMARY KEY CLUSTERED ([MaTK] ASC)
);

CREATE TABLE [dbo].[ThiepCuoi] (
    [MaTC]       INT             IDENTITY (1, 1) NOT NULL,
    [HinhAnh]    VARCHAR (200)   NULL,
    [KichThuoc]  NVARCHAR (100)  NULL,
    [ChatLieu]   NVARCHAR (100)  NULL,
    [MauSac]     NVARCHAR (100)  NULL,
    [NhaCungCap] NVARCHAR (200)  NULL,
    [Gia]        DECIMAL (18, 2) NOT NULL,
    [KhuyenMai]  DECIMAL (18, 2) NULL,
    [TrangThai]  NVARCHAR (100)  NULL,
    [HoaTiec]    NVARCHAR (200)  NULL,
    PRIMARY KEY CLUSTERED ([MaTC] ASC)
);

CREATE TABLE [dbo].[ThietBi] (
    [MaTB]        INT             IDENTITY (1, 1) NOT NULL,
    [TenTB]       NVARCHAR (100)  NOT NULL,
    [LoaiTB]      NVARCHAR (100)  NOT NULL,
    [SoLuongNhap] INT             NOT NULL,
    [SoLuongHu]   INT             NOT NULL,
    [SoLuongTon]  INT             NOT NULL,
    [ViTriSD]     NVARCHAR (100)  NULL,
    [HinhAnh]     VARCHAR (100)   NULL,
    [GhiChu]      NVARCHAR (300)  NULL,
    [Gia]         DECIMAL (18, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([MaTB] ASC)
);

CREATE TABLE [dbo].[TiecCuoi] (
    [MaTC]            INT             IDENTITY (1, 1) NOT NULL,
    [TenKH]           NVARCHAR (100)  NOT NULL,
    [NgayDat]         DATE            NOT NULL,
    [ThoiGianBatDau]  TIME (7)        NOT NULL,
    [ThoiGianKetThuc] TIME (7)        NOT NULL,
    [TenCoDau]        NVARCHAR (100)  NULL,
    [TenChuRe]        NVARCHAR (100)  NULL,
    [TenSanh]         NVARCHAR (100)  NULL,
    [SlBan]           INT             NOT NULL,
    [GoiAnTiec]       NVARCHAR (100)  NULL,
    [GoiDichVu]       NVARCHAR (100)  NULL,
    [LoaiThiepCuoi]   NVARCHAR (100)  NULL,
    [GoiThucDon]      NVARCHAR (100)  NULL,
    [GoiNuoc]         NVARCHAR (100)  NULL,
    [GoiThietBi]      NVARCHAR (100)  NULL,
    [GoiGiaTri]       NVARCHAR (100)  NULL,
    [DonGiaTiec]      DECIMAL (18, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([MaTC] ASC)
);
CREATE TABLE [dbo].[ChiTietMonAnNguyenLieu] (
    [MaMA]    INT             NOT NULL,
    [MaNL]    INT             NOT NULL,
    [TenNL]   NVARCHAR (100)  NOT NULL,
    [SoLuong] DECIMAL (18, 2) NULL,
    [DonVi]   NVARCHAR (50)   NULL,
    PRIMARY KEY CLUSTERED ([MaMA] ASC, [MaNL] ASC),
    FOREIGN KEY ([MaNL]) REFERENCES [dbo].[NguyenLieu] ([MaNL]) ON DELETE CASCADE,
    FOREIGN KEY ([MaMA]) REFERENCES [dbo].[MonAn] ([MaMA]) ON DELETE CASCADE
);