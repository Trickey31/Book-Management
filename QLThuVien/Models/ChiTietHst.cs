using System;
using System.Collections.Generic;

namespace QLThuVien.Models;

public partial class ChiTietHst
{
    public string? ViPham { get; set; }

    public decimal? ThanhTien { get; set; }

    public string MaHoSotra { get; set; } = null!;

    public string MaSach { get; set; } = null!;

    public virtual HoSoTra MaHoSotraNavigation { get; set; } = null!;

    public virtual Sach MaSachNavigation { get; set; } = null!;
}
