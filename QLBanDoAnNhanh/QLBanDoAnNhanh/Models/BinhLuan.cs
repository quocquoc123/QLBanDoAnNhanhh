﻿using System;
using System.Collections.Generic;

namespace QLBanDoAnNhanh.Models;

public partial class BinhLuan
{
    public int MaBinhLuan { get; set; }

    public int MaSp { get; set; }

    public int MaNguoiDung { get; set; }

    public string NoiDung { get; set; } = null!;

    public DateTime NgayBinhLuan { get; set; }

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
