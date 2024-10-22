using System;
using System.Collections.Generic;

namespace QLBanDoAnNhanh.Models;

public partial class HinhAnh
{
    public int Id { get; set; }

    public string UrlHinh { get; set; } = null!;

    public int? MaSp { get; set; }

    public virtual SanPham? MaSpNavigation { get; set; }
}
