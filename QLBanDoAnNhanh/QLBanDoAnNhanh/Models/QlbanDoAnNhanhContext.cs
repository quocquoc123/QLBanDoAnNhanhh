﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QLBanDoAnNhanh.Models;

public partial class QlbanDoAnNhanhContext : DbContext
{
    public QlbanDoAnNhanhContext()
    {
    }

    public QlbanDoAnNhanhContext(DbContextOptions<QlbanDoAnNhanhContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BinhLuan> BinhLuans { get; set; }

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }

    public virtual DbSet<DanhGium> DanhGia { get; set; }

    public virtual DbSet<DanhMuc> DanhMucs { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<GiamGium> GiamGia { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<HinhAnh> HinhAnhs { get; set; }

    public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<PhanQuyen> PhanQuyens { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<ThanhToan> ThanhToans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-EQ0I61SI;Initial Catalog=QLBanDoAnNhanh;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BinhLuan>(entity =>
        {
            entity.HasKey(e => e.MaBinhLuan).HasName("PK__BinhLuan__87CB66A0C19972B3");

            entity.ToTable("BinhLuan");

            entity.Property(e => e.MaSp).HasColumnName("maSP");
            entity.Property(e => e.NgayBinhLuan)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.BinhLuans)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BinhLuan__MaNguo__693CA210");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.BinhLuans)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BinhLuan__maSP__68487DD7");
        });

        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ChiTietD__3213E83F5D41E983");

            entity.ToTable("ChiTietDonHang");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaDh)
                .HasMaxLength(255)
                .HasColumnName("maDH");
            entity.Property(e => e.MaSp).HasColumnName("maSP");
            entity.Property(e => e.SoLuong).HasColumnName("soLuong");
            entity.Property(e => e.TongTien).HasColumnName("tongTien");

            entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaDh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDon__maDH__74AE54BC");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDon__maSP__73BA3083");
        });

        modelBuilder.Entity<ChiTietGioHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ChiTietG__3213E83F4598D469");

            entity.ToTable("ChiTietGioHang");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaGh).HasColumnName("maGH");
            entity.Property(e => e.MaSp).HasColumnName("maSP");
            entity.Property(e => e.SoLuongSp).HasColumnName("soLuongSP");
            entity.Property(e => e.TongTien).HasColumnName("tongTien");

            entity.HasOne(d => d.MaGhNavigation).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.MaGh)
                .HasConstraintName("FK__ChiTietGio__maGH__7B5B524B");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.MaSp)
                .HasConstraintName("FK__ChiTietGio__maSP__7A672E12");
        });

        modelBuilder.Entity<DanhGium>(entity =>
        {
            entity.HasKey(e => e.MaDanhGia).HasName("PK__DanhGia__AA9515BF53AD69D8");

            entity.Property(e => e.NgayBinhLuan)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DanhGia)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DanhGia__MaNguoi__6E01572D");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.DanhGia)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DanhGia__MaSanPh__6D0D32F4");
        });

        modelBuilder.Entity<DanhMuc>(entity =>
        {
            entity.HasKey(e => e.MaDm).HasName("PK__DanhMuc__7A3EF4083389E981");

            entity.ToTable("DanhMuc");

            entity.Property(e => e.MaDm).HasColumnName("maDM");
            entity.Property(e => e.TenDm)
                .HasMaxLength(100)
                .HasColumnName("tenDM");
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.MaDh).HasName("PK__DonHang__7A3EF40FE177817E");

            entity.ToTable("DonHang");

            entity.Property(e => e.MaDh)
                .HasMaxLength(255)
                .HasColumnName("maDH");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Diachi)
                .HasMaxLength(700)
                .HasColumnName("diachi");
            entity.Property(e => e.MaNguoiDung).HasColumnName("maNguoiDung");
            entity.Property(e => e.SoLuong).HasColumnName("soLuong");
            entity.Property(e => e.TongTien).HasColumnName("tongTien");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(700)
                .HasColumnName("trangThai");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.Username)
                .HasMaxLength(200)
                .HasColumnName("username");

            entity.HasOne(d => d.MaKhuyenMaiNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaKhuyenMai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonHang__MaKhuye__5EBF139D");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__DonHang__maNguoi__5FB337D6");
        });

        modelBuilder.Entity<GiamGium>(entity =>
        {
            entity.HasKey(e => e.MaGiamGia).HasName("PK__GiamGia__EF9458E4218B159F");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.MaGh).HasName("PK__GioHang__7A3E2D6B02FD4B3B");

            entity.ToTable("GioHang");

            entity.Property(e => e.MaGh).HasColumnName("maGH");
            entity.Property(e => e.MaNguoiDung).HasColumnName("maNguoiDung");
            entity.Property(e => e.SoLuong).HasColumnName("soLuong");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__GioHang__maNguoi__778AC167");
        });

        modelBuilder.Entity<HinhAnh>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HinhAnh__3213E83F2C17502B");

            entity.ToTable("HinhAnh");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaSp).HasColumnName("maSP");
            entity.Property(e => e.UrlHinh)
                .HasMaxLength(2000)
                .HasColumnName("urlHinh");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.HinhAnhs)
                .HasForeignKey(d => d.MaSp)
                .HasConstraintName("FK__HinhAnh__maSP__70DDC3D8");
        });

        modelBuilder.Entity<KhuyenMai>(entity =>
        {
            entity.HasKey(e => e.MaKhuyenMai).HasName("PK__KhuyenMa__6F56B3BDDCEA7517");

            entity.ToTable("KhuyenMai");

            entity.Property(e => e.DieuKienApDung).HasDefaultValue(0);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoLuong).HasDefaultValue(1);
            entity.Property(e => e.ThoiGianBatDau)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ThoiGianKetThuc)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__NguoiDun__446439EA7F62D222");

            entity.ToTable("NguoiDung");

            entity.Property(e => e.MaNguoiDung).HasColumnName("maNguoiDung");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasColumnName("email");
            entity.Property(e => e.HoTen)
                .HasMaxLength(200)
                .HasColumnName("hoTen");
            entity.Property(e => e.Matkhau)
                .HasMaxLength(200)
                .HasColumnName("matkhau");
            entity.Property(e => e.RoleId).HasColumnName("roleID");
            entity.Property(e => e.Sdt)
                .HasMaxLength(200)
                .HasColumnName("sdt");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasColumnName("trangThai");
            entity.Property(e => e.Username)
                .HasMaxLength(200)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.NguoiDungs)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NguoiDung__roleI__5BE2A6F2");
        });

        modelBuilder.Entity<PhanQuyen>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__PhanQuye__CD98460A44DCB821");

            entity.ToTable("PhanQuyen");

            entity.Property(e => e.RoleId).HasColumnName("roleID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(20)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.MaSp).HasName("PK__SanPham__7A227A7A1E411225");

            entity.ToTable("SanPham");

            entity.Property(e => e.MaSp).HasColumnName("maSP");
            entity.Property(e => e.ChitietSp)
                .HasMaxLength(1000)
                .HasColumnName("chitietSP");
            entity.Property(e => e.DonVi).HasColumnName("donVi");
            entity.Property(e => e.GiaTien).HasColumnName("giaTien");
            entity.Property(e => e.HinhAnh1)
                .HasMaxLength(700)
                .HasColumnName("hinhAnh1");
            entity.Property(e => e.HinhAnh2)
                .HasMaxLength(700)
                .HasColumnName("hinhAnh2");
            entity.Property(e => e.MaDm).HasColumnName("maDM");
            entity.Property(e => e.SlbanTrongNgay).HasColumnName("SLBanTrongNgay");
            entity.Property(e => e.TenSp)
                .HasMaxLength(700)
                .HasColumnName("tenSP");
            entity.Property(e => e.ThanhPhan)
                .HasMaxLength(700)
                .HasColumnName("thanhPhan");

            entity.HasOne(d => d.MaDmNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.MaDm)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SanPham__maDM__571DF1D5");

            entity.HasOne(d => d.MaGiamGiaNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.MaGiamGia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SanPham__MaGiamG__5629CD9C");
        });

        modelBuilder.Entity<ThanhToan>(entity =>
        {
            entity.HasKey(e => e.MaThanhToan).HasName("PK__ThanhToa__D4B25844AB67F420");

            entity.ToTable("ThanhToan");

            entity.Property(e => e.MaDh)
                .HasMaxLength(255)
                .HasColumnName("maDH");
            entity.Property(e => e.NgayThanhToan)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PhuongThucThanhToan).HasMaxLength(50);

            entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.ThanhToans)
                .HasForeignKey(d => d.MaDh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ThanhToan__maDH__6477ECF3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
