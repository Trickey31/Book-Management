using System;
using System.Collections.Generic;

namespace QLThuVien.Models;

public partial class ChiTietPm
{
    public string MaPhieuMuon { get; set; } = null!;

    public DateTime? NgayMuon { get; set; }

    public int? ThoiGianCho { get; set; }

    public DateTime? NgayHenTra { get; set; }

    public string? TinhTrang { get; set; }

    public virtual PhieuMuon MaPhieuMuonNavigation { get; set; } = null!;
}
