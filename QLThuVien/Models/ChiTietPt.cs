using System;
using System.Collections.Generic;

namespace QLThuVien.Models;

public partial class ChiTietPt
{
    public string MaPhieuTra { get; set; } = null!;

    public DateTime? NgayTra { get; set; }

    public string? NhanXet { get; set; }

    public virtual PhieuTra MaPhieuTraNavigation { get; set; } = null!;
}
