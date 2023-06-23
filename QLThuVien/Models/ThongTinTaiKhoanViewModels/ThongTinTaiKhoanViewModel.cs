using Microsoft.EntityFrameworkCore;

namespace QLThuVien.Models.ThongTinTaiKhoanViewModels
{
    
    public class ThongTinTaiKhoanViewModel
    {
        public NhanVien NhanVien { get; set; }
        public User User { get; set; }
        public NguoiDoc NguoiDoc { get; set; }

    }
}
