using System;
using System.Collections.Generic;

namespace QLBanDoAnNhanh.Models;

public partial class DanhGium
{
    public int MaDanhGia { get; set; }

    public int MaSanPham { get; set; }

    public int MaNguoiDung { get; set; }

    public string NoiDung { get; set; }

    public DateTime NgayBinhLuan { get; set; }

    public virtual NguoiDung MaNguoiDungNavigation { get; set; }

    public virtual SanPham MaSanPhamNavigation { get; set; }
}
