using System;
using System.Collections.Generic;

namespace QLThuVien.Models;

public partial class NgonNgu
{
    public string MaNgonNgu { get; set; } = null!;

    public string? TenNgonNgu { get; set; }

    public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
}
