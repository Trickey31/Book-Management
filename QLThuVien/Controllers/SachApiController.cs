using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLThuVien.Models;

namespace QLThuVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SachApiController : ControllerBase
    {
        QlthuVienLtwebContext db = new QlthuVienLtwebContext();

        [HttpGet]
        public IEnumerable<Sach> GetAllSach()
        {
            var sach = (from s in db.Saches
                        select new Sach
                        {
                            MaSach = s.MaSach,
                            TenSach = s.TenSach,
                            TacGia = s.TacGia,
                            NamXb = s.NamXb,
                            SoLuong = s.SoLuong,
                            MaNxb = s.MaNxb,
                            MaTheLoai = s.MaTheLoai,
                            MaNgonNgu = s.MaNgonNgu,
                            TenFileAnhDd = s.TenFileAnhDd,
                        }).ToList();
            return sach;
        }

        [HttpGet("{maTheLoai}")]
        public IEnumerable<Sach> GetSachByTheLoai(string maTheLoai) {
            var sach = (from s in db.Saches
                        where s.MaTheLoai == maTheLoai
                        select new Sach
                        {
                            MaSach = s.MaSach,
                            TenSach = s.TenSach,
                            TacGia = s.TacGia,
                            NamXb = s.NamXb,
                            SoLuong = s.SoLuong,
                            MaNxb = s.MaNxb,
                            MaTheLoai = s.MaTheLoai,
                            MaNgonNgu = s.MaNgonNgu,
                            TenFileAnhDd = s.TenFileAnhDd,
                        }).ToList(); ;
            return sach;
        }
    }
}
