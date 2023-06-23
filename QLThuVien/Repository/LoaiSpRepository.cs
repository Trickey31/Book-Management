using QLThuVien.Models;

namespace QLThuVien.Repository
{
	public class LoaiSpRepository : ILoaiSpRepository
	{
		private readonly QlthuVienLtwebContext _context;
		public LoaiSpRepository(QlthuVienLtwebContext context)
		{
			_context = context;
		}

		public TheLoai Add(TheLoai loaiSach)
		{
			_context.TheLoais.Add(loaiSach);
			_context.SaveChanges();
			return loaiSach;
		}

		public TheLoai Delete(TheLoai maTheLoai)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<TheLoai> GetAllLoai()
		{
			return _context.TheLoais;
		}

		public TheLoai GetTheLoai(TheLoai maTheLoai)
		{
			return _context.TheLoais.Find(maTheLoai);
		}

		public TheLoai Update(TheLoai loaiSach)
		{
			_context.Update(loaiSach);
			_context.SaveChanges();
			return loaiSach;
		}
	}
}
