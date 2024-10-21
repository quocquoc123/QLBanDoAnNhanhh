use master
Create Database QLBanDoAnNhanh;
	use QLBanDoAnNhanh;
	go

DROP DATABASE QLBanDoAnNhanh;


CREATE TABLE [dbo].[DanhMuc](
	[maDM] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY ,
	[tenDM] [nvarchar](100) NOT NULL,
);





CREATE TABLE GiamGia
(
  MaGiamGia INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
  GiaTri int not null DEFAULT 0,
);


CREATE TABLE KhuyenMai
(
  MaKhuyenMai INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
  GiaTri int not null,
  ThoiGianBatDau datetime not null default getdate(),
  ThoiGianKetThuc datetime not null default getdate(),
  TrangThai bit not null default 0,
  NgayTao datetime not null default getdate(),
  DieuKienApDung int default 0,
  SoLuong int not null default 1
);

CREATE TABLE [dbo].[SanPham](
	[maSP] [int] IDENTITY(1,1) NOT NULL Primary key,
	[tenSP] [nvarchar](700) NOT NULL,
	[MaGiamGia] INT NOT NULL,
	[thanhPhan] [nvarchar](700) NOT NULL,
	[giaTien] [float] NOT NULL,
	[donVi] [float] NOT NULL,
	[chitietSP] [nvarchar](1000) NULL,
	[maDM] [int] NOT NULL,
	[SLBanTrongNgay] [int] NULL,
	[hinhAnh1] [nvarchar](700) NULL,
	[hinhAnh2] [nvarchar](700) NULL,
	FOREIGN KEY (MaGiamGia) REFERENCES GiamGia(MaGiamGia),
	FOREIGN KEY (maDM) REFERENCES DanhMuc(maDM)
);

CREATE TABLE [dbo].[PhanQuyen](
	[roleID] [int] IDENTITY(1,1) NOT NULL Primary key,
	[roleName] [nvarchar](20) NOT NULL,
);
GO

CREATE TABLE [dbo].[NguoiDung](
    [maNguoiDung] [int] IDENTITY(1,1) NOT NULL Primary key,
	[username] [nvarchar](200) NOT NULL,
	[trangThai] [nvarchar](50) NOT NULL,
	[hoTen] [nvarchar](200) NOT NULL,
	[email] [nvarchar](200) NOT NULL,
	[sdt] [nvarchar](200) NOT NULL,
	[matkhau] [nvarchar](200) NOT NULL,
	[roleID] [int] NOT NULL,
	FOREIGN KEY (roleID) REFERENCES PhanQuyen(roleID)
);

CREATE TABLE [dbo].[DonHang](
	[maDH] [nvarchar](255) NOT NULL Primary key,
	[username] [nvarchar](200) NOT NULL,
	[diachi] [nvarchar](700) NOT NULL,
	MaKhuyenMai INT NOT NULL,
	[tongTien] [float] NOT NULL,
	[soLuong] [int] NOT NULL,
	[trangThai] [nvarchar](700) NOT NULL,
	[createdAt] [datetime] NULL,
	[updatedAt] [datetime] NULL,
	[maNguoiDung] [int],
	FOREIGN KEY (MaKhuyenMai) REFERENCES KhuyenMai(MaKhuyenMai),
	FOREIGN KEY ([maNguoiDung]) REFERENCES [NguoiDung]([maNguoiDung])     
	,
);

CREATE TABLE ThanhToan
(
    MaThanhToan INT IDENTITY(1,1) PRIMARY KEY, 
    maDH [nvarchar](255) NOT NULL,                      
    PhuongThucThanhToan NVARCHAR(50) NOT NULL,   
	NgayThanhToan DATETIME DEFAULT GETDATE(),    
    TongTien FLOAT NOT NULL,                     
    TrangThaiThanhToan bit not null default 0, 
    FOREIGN KEY (maDH) REFERENCES DonHang(maDH)     
);

CREATE TABLE BinhLuan
(
  MaBinhLuan INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
  maSP INT NOT NULL,
  MaNguoiDung INT NOT NULL,
  NoiDung NVARCHAR(MAX) NOT NULL,
  NgayBinhLuan DATETIME NOT NULL DEFAULT GETDATE(),
  FOREIGN KEY (maSP) REFERENCES SanPham(maSP),
  FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung)
);
CREATE TABLE DanhGia
(
  MaDanhGia INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
  MaSanPham INT NOT NULL,
  MaNguoiDung INT NOT NULL,
  NoiDung NVARCHAR(MAX) NOT NULL,
  NgayBinhLuan DATETIME NOT NULL DEFAULT GETDATE(),
  FOREIGN KEY (MaSanPham) REFERENCES SanPham(maSP),
  FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung)
);



CREATE TABLE [dbo].[HinhAnh](
	[id] [int] IDENTITY(1,1) NOT NULL Primary key,
	[urlHinh] [nvarchar](2000) NOT NULL,
	[maSP] [int] NULL,
	FOREIGN KEY (maSP) REFERENCES SanPham(maSP)
);


CREATE TABLE [dbo].[ChiTietDonHang](
	[id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY ,
	[maDH] [nvarchar](255) NOT NULL,
	[maSP] [int] NOT NULL,
	[soLuong] [int] NOT NULL,
	[tongTien] [int] NOT NULL,
	FOREIGN KEY ([maSP]) REFERENCES SanPham(maSP),
	FOREIGN KEY (maDH) REFERENCES DonHang(maDH)
);
CREATE TABLE [dbo].[GioHang](
	[maGH] [int] IDENTITY(1,1) NOT NULL Primary key,
	[maNguoiDung] [int] NULL,
	[soLuong] [int] NULL,
	FOREIGN KEY (maNguoiDung) REFERENCES NguoiDung(maNguoiDung)
);

CREATE TABLE [dbo].[ChiTietGioHang](
	[id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY ,
	[maGH] [int]  NULL,
	[soLuongSP] [int] NULL,
	[maSP] [int] NULL,
	[tongTien] [int] NULL,
	FOREIGN KEY (maSP) REFERENCES SanPham(maSP),
	FOREIGN KEY (maGH) REFERENCES GioHang(maGH)

);




