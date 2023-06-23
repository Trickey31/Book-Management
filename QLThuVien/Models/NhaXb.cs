using System;
using System.Collections.Generic;

namespace QLThuVien.Models;

public partial class NhaXb
{
    public string MaNxb { get; set; } = null!;

    public string? TenNxb { get; set; }

    public string? DiaChi { get; set; }

    public string? DienThoai { get; set; }

    public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
}
