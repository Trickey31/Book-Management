using QLThuVien.Models.ThongTinTaiKhoanViewModels;
using QLThuVien.Models.GioHangViewModels;

namespace QLThuVien.Models.ProfileAndLichSuViewModels
{
    public class ProfileAndLichSuViewModel
    {
        public ThongTinTaiKhoanViewModel ThongTinTaiKhoan { get; set; }
        public List<GioHangViewModel> LichSuMuon { get; set; }
    }
}
