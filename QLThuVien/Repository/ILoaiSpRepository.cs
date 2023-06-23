using QLThuVien.Models;

namespace QLThuVien.Repository
{
	public interface ILoaiSpRepository
	{
		TheLoai Add(TheLoai loaiSach);
		TheLoai Update(TheLoai loaiSach);
		TheLoai Delete(TheLoai maTheLoai);
		TheLoai GetTheLoai(TheLoai maTheLoai);
		IEnumerable<TheLoai> GetAllLoai();
		
	}
}
