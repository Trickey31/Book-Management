using Microsoft.EntityFrameworkCore;

namespace QLThuVien.Models.GioHangViewModels
{
    public class GioHangViewModel
    {
        public string tenSach { get; set; }
        public string maPhieuMuon { get; set; }
        public DateTime ngayMuon { get; set; }
        public DateTime ngayHenTra { get; set; }
        public string tinhTrang { get; set; }
        public string nguoiMuon { get; set; }
        public string daMuon { get; set; }

    }
}
