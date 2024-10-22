﻿using System;
using System.Collections.Generic;

namespace QLBanDoAnNhanh.Models;

public partial class PhanQuyen
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
}
