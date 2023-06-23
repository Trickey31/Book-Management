using System;
using System.Collections.Generic;

namespace QLThuVien.Models;

public partial class NguoiDoc
{
    public string MaNguoiDoc { get; set; } = null!;

    public string? HoTen { get; set; }

    public string? GioiTinh { get; set; }

    public string? DiaChi { get; set; }

    public string? Sdt { get; set; }

    public string? Username { get; set; }

    public string? AnhDaiDien { get; set; }

    public virtual ICollection<PhieuMuon> PhieuMuons { get; set; } = new List<PhieuMuon>();

    public virtual ICollection<PhieuTra> PhieuTras { get; set; } = new List<PhieuTra>();

    public virtual User? UsernameNavigation { get; set; }
}
