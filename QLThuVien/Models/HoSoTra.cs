using System;
using System.Collections.Generic;

namespace QLThuVien.Models;

public partial class HoSoTra
{
    public string MaHoSotra { get; set; } = null!;

    public DateTime? NgayTra { get; set; }

    public string MaNhanVien { get; set; } = null!;

    public virtual ICollection<ChiTietHst> ChiTietHsts { get; set; } = new List<ChiTietHst>();

    public virtual NhanVien MaNhanVienNavigation { get; set; } = null!;

    public virtual ICollection<PhieuMuon> PhieuMuons { get; set; } = new List<PhieuMuon>();
}
