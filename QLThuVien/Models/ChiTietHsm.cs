using System;
using System.Collections.Generic;

namespace QLThuVien.Models;

public partial class ChiTietHsm
{
    public int? SoLuong { get; set; }

    public string? TinhTrangSachKhiMuon { get; set; }

    public string MaHoSoMuon { get; set; } = null!;

    public string MaSach { get; set; } = null!;

    public virtual PhieuMuon MaHoSoMuonNavigation { get; set; } = null!;

    public virtual Sach MaSachNavigation { get; set; } = null!;
}
