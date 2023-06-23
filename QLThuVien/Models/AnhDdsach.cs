using System;
using System.Collections.Generic;

namespace QLThuVien.Models;

public partial class AnhDdsach
{
    public string MaSach { get; set; } = null!;

    public string? TenFileAnh { get; set; }

    public string? ViTri { get; set; }

    public virtual Sach MaSachNavigation { get; set; } = null!;
}
