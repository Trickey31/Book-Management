using System;
using System.Collections.Generic;

namespace QLThuVien.Models;

public partial class Sach
{
    public string MaSach { get; set; } = null!;

    public string? TenSach { get; set; }

    public string? TacGia { get; set; }

    public string? NamXb { get; set; }

    public int? SoLuong { get; set; }

    public string? MaNxb { get; set; }

    public string? MaTheLoai { get; set; }

    public string? MaNgonNgu { get; set; }

    public string? TenFileAnhDd { get; set; }

    public virtual ICollection<BanSaoSach> BanSaoSaches { get; set; } = new List<BanSaoSach>();

    public virtual NgonNgu? MaNgonNguNavigation { get; set; }

    public virtual NhaXb? MaNxbNavigation { get; set; }

    public virtual TheLoai? MaTheLoaiNavigation { get; set; }

    public virtual ViTriSach? ViTriSach { get; set; }
}
