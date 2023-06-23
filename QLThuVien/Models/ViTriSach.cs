using System;
using System.Collections.Generic;

namespace QLThuVien.Models;

public partial class ViTriSach
{
    public string MaSach { get; set; } = null!;

    public string? Hang { get; set; }

    public string? Ke { get; set; }

    public virtual Sach MaSachNavigation { get; set; } = null!;
}
