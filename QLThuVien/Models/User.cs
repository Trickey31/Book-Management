using System;
using System.Collections.Generic;

namespace QLThuVien.Models;

public partial class User
{
    public string Username { get; set; } = null!;

    public string? Password { get; set; }

    public string? LoaiUser { get; set; }

    public string? EmailDk { get; set; }

    public virtual ICollection<NguoiDoc> NguoiDocs { get; set; } = new List<NguoiDoc>();

    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
}
