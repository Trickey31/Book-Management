using System;
using System.Collections.Generic;

namespace QLThuVien.Models;

public partial class PhieuMuon
{
    public string MaPhieuMuon { get; set; } = null!;

    public string? MaNguoiDoc { get; set; }

    public string? MaNhanVien { get; set; }

    public string? MaBanSao { get; set; }

    public virtual ChiTietPm? ChiTietPm { get; set; }

    public virtual BanSaoSach? MaBanSaoNavigation { get; set; }

    public virtual NguoiDoc? MaNguoiDocNavigation { get; set; }

    public virtual NhanVien? MaNhanVienNavigation { get; set; }
}
