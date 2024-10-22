﻿using System;
using System.Collections.Generic;

namespace QLBanDoAnNhanh.Models;

public partial class ChiTietDonHang
{
    public int Id { get; set; }

    public string MaDh { get; set; } = null!;

    public int MaSp { get; set; }

    public int SoLuong { get; set; }

    public int TongTien { get; set; }

    public virtual DonHang MaDhNavigation { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
