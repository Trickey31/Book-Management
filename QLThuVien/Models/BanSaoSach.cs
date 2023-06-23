using System;
using System.Collections.Generic;

namespace QLThuVien.Models;

public partial class BanSaoSach
{
    public string MaBanSao { get; set; } = null!;

    public string? MaSach { get; set; }

    public string? DaMuon { get; set; }

    public virtual Sach? MaSachNavigation { get; set; }

    public virtual ICollection<PhieuMuon> PhieuMuons { get; set; } = new List<PhieuMuon>();

    public virtual ICollection<PhieuTra> PhieuTras { get; set; } = new List<PhieuTra>();
}
