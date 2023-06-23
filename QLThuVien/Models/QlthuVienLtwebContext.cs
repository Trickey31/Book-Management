using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QLThuVien.Models;

public partial class QlthuVienLtwebContext : DbContext
{
    public QlthuVienLtwebContext()
    {
    }

    public QlthuVienLtwebContext(DbContextOptions<QlthuVienLtwebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BanSaoSach> BanSaoSaches { get; set; }

    public virtual DbSet<ChiTietPm> ChiTietPms { get; set; }

    public virtual DbSet<ChiTietPt> ChiTietPts { get; set; }

    public virtual DbSet<NgonNgu> NgonNgus { get; set; }

    public virtual DbSet<NguoiDoc> NguoiDocs { get; set; }

    public virtual DbSet<NhaXb> NhaXbs { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<PhieuMuon> PhieuMuons { get; set; }

    public virtual DbSet<PhieuTra> PhieuTras { get; set; }

    public virtual DbSet<Sach> Saches { get; set; }

    public virtual DbSet<TheLoai> TheLoais { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<ViTriSach> ViTriSaches { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=TRICKEY\\SQLEXPRESS;Initial Catalog=QLThuVien_LTweb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BanSaoSach>(entity =>
        {
            entity.HasKey(e => e.MaBanSao);

            entity.ToTable("BanSaoSach");

            entity.Property(e => e.MaBanSao).HasMaxLength(50);
            entity.Property(e => e.DaMuon).HasMaxLength(50);
            entity.Property(e => e.MaSach).HasMaxLength(50);

            entity.HasOne(d => d.MaSachNavigation).WithMany(p => p.BanSaoSaches)
                .HasForeignKey(d => d.MaSach)
                .HasConstraintName("FK_BanSaoSach_Sach");
        });

        modelBuilder.Entity<ChiTietPm>(entity =>
        {
            entity.HasKey(e => e.MaPhieuMuon);

            entity.ToTable("ChiTietPM");

            entity.Property(e => e.MaPhieuMuon).HasMaxLength(50);
            entity.Property(e => e.NgayHenTra).HasColumnType("datetime");
            entity.Property(e => e.NgayMuon).HasColumnType("datetime");
            entity.Property(e => e.TinhTrang).HasMaxLength(50);

            entity.HasOne(d => d.MaPhieuMuonNavigation).WithOne(p => p.ChiTietPm)
                .HasForeignKey<ChiTietPm>(d => d.MaPhieuMuon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietPM_PhieuMuon");
        });

        modelBuilder.Entity<ChiTietPt>(entity =>
        {
            entity.HasKey(e => e.MaPhieuTra);

            entity.ToTable("ChiTietPT");

            entity.Property(e => e.MaPhieuTra).HasMaxLength(50);
            entity.Property(e => e.NgayTra).HasColumnType("datetime");
            entity.Property(e => e.NhanXet).HasMaxLength(50);

            entity.HasOne(d => d.MaPhieuTraNavigation).WithOne(p => p.ChiTietPt)
                .HasForeignKey<ChiTietPt>(d => d.MaPhieuTra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietPT_PhieuTra");
        });

        modelBuilder.Entity<NgonNgu>(entity =>
        {
            entity.HasKey(e => e.MaNgonNgu).HasName("PK__NgonNgu__131924EE0796FE13");

            entity.ToTable("NgonNgu");

            entity.Property(e => e.MaNgonNgu).HasMaxLength(50);
            entity.Property(e => e.TenNgonNgu).HasMaxLength(50);
        });

        modelBuilder.Entity<NguoiDoc>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDoc);

            entity.ToTable("NguoiDoc");

            entity.Property(e => e.MaNguoiDoc).HasMaxLength(50);
            entity.Property(e => e.AnhDaiDien).HasMaxLength(100);
            entity.Property(e => e.DiaChi).HasMaxLength(50);
            entity.Property(e => e.GioiTinh).HasMaxLength(50);
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.Sdt)
                .HasMaxLength(50)
                .HasColumnName("SDT");
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.UsernameNavigation).WithMany(p => p.NguoiDocs)
                .HasForeignKey(d => d.Username)
                .HasConstraintName("FK_NguoiDoc_User");
        });

        modelBuilder.Entity<NhaXb>(entity =>
        {
            entity.HasKey(e => e.MaNxb).HasName("PK__NhaXB__3A19482C1463C5DC");

            entity.ToTable("NhaXB");

            entity.Property(e => e.MaNxb)
                .HasMaxLength(50)
                .HasColumnName("MaNXB");
            entity.Property(e => e.DiaChi).HasMaxLength(50);
            entity.Property(e => e.DienThoai).HasMaxLength(50);
            entity.Property(e => e.TenNxb)
                .HasMaxLength(50)
                .HasColumnName("TenNXB");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNhanVien).HasName("PK__NhanVien__77B2CA47138E8BAF");

            entity.ToTable("NhanVien");

            entity.Property(e => e.MaNhanVien).HasMaxLength(50);
            entity.Property(e => e.AnhDaiDien).HasMaxLength(100);
            entity.Property(e => e.CaLam).HasMaxLength(50);
            entity.Property(e => e.DiaChi).HasMaxLength(50);
            entity.Property(e => e.GioiTinh).HasMaxLength(50);
            entity.Property(e => e.Que).HasMaxLength(50);
            entity.Property(e => e.Sdt)
                .HasMaxLength(50)
                .HasColumnName("SDT");
            entity.Property(e => e.TenNhanVien).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.UsernameNavigation).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.Username)
                .HasConstraintName("FK_NhanVien_User");
        });

        modelBuilder.Entity<PhieuMuon>(entity =>
        {
            entity.HasKey(e => e.MaPhieuMuon);

            entity.ToTable("PhieuMuon");

            entity.Property(e => e.MaPhieuMuon).HasMaxLength(50);
            entity.Property(e => e.MaBanSao).HasMaxLength(50);
            entity.Property(e => e.MaNguoiDoc).HasMaxLength(50);
            entity.Property(e => e.MaNhanVien).HasMaxLength(50);

            entity.HasOne(d => d.MaBanSaoNavigation).WithMany(p => p.PhieuMuons)
                .HasForeignKey(d => d.MaBanSao)
                .HasConstraintName("FK_PhieuMuon_BanSaoSach");

            entity.HasOne(d => d.MaNguoiDocNavigation).WithMany(p => p.PhieuMuons)
                .HasForeignKey(d => d.MaNguoiDoc)
                .HasConstraintName("FK_PhieuMuon_NguoiDoc");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.PhieuMuons)
                .HasForeignKey(d => d.MaNhanVien)
                .HasConstraintName("FK_PhieuMuon_NhanVien");
        });

        modelBuilder.Entity<PhieuTra>(entity =>
        {
            entity.HasKey(e => e.MaPhieuTra);

            entity.ToTable("PhieuTra");

            entity.Property(e => e.MaPhieuTra).HasMaxLength(50);
            entity.Property(e => e.MaBanSao).HasMaxLength(50);
            entity.Property(e => e.MaNguoiDoc).HasMaxLength(50);
            entity.Property(e => e.MaNhanVien).HasMaxLength(50);

            entity.HasOne(d => d.MaBanSaoNavigation).WithMany(p => p.PhieuTras)
                .HasForeignKey(d => d.MaBanSao)
                .HasConstraintName("FK_PhieuTra_BanSaoSach");

            entity.HasOne(d => d.MaNguoiDocNavigation).WithMany(p => p.PhieuTras)
                .HasForeignKey(d => d.MaNguoiDoc)
                .HasConstraintName("FK_PhieuTra_NguoiDoc");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.PhieuTras)
                .HasForeignKey(d => d.MaNhanVien)
                .HasConstraintName("FK_PhieuTra_NhanVien");
        });

        modelBuilder.Entity<Sach>(entity =>
        {
            entity.HasKey(e => e.MaSach).HasName("PK__Sach__B235742DA0BF0890");

            entity.ToTable("Sach");

            entity.Property(e => e.MaSach).HasMaxLength(50);
            entity.Property(e => e.MaNgonNgu).HasMaxLength(50);
            entity.Property(e => e.MaNxb)
                .HasMaxLength(50)
                .HasColumnName("MaNXB");
            entity.Property(e => e.MaTheLoai).HasMaxLength(50);
            entity.Property(e => e.NamXb)
                .HasMaxLength(50)
                .HasColumnName("NamXB");
            entity.Property(e => e.TacGia).HasMaxLength(50);
            entity.Property(e => e.TenFileAnhDd).HasColumnName("TenFIleAnhDD");
            entity.Property(e => e.TenSach).HasMaxLength(100);

            entity.HasOne(d => d.MaNgonNguNavigation).WithMany(p => p.Saches)
                .HasForeignKey(d => d.MaNgonNgu)
                .HasConstraintName("FK_Sach_NgonNgu");

            entity.HasOne(d => d.MaNxbNavigation).WithMany(p => p.Saches)
                .HasForeignKey(d => d.MaNxb)
                .HasConstraintName("FK_Sach_NhaXB");

            entity.HasOne(d => d.MaTheLoaiNavigation).WithMany(p => p.Saches)
                .HasForeignKey(d => d.MaTheLoai)
                .HasConstraintName("FK_Sach_TheLoai");
        });

        modelBuilder.Entity<TheLoai>(entity =>
        {
            entity.HasKey(e => e.MaTheLoai).HasName("PK__TheLoai__D73FF34A76BAA0AE");

            entity.ToTable("TheLoai");

            entity.Property(e => e.MaTheLoai).HasMaxLength(50);
            entity.Property(e => e.TenTheLoai).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Username);

            entity.ToTable("User");

            entity.Property(e => e.Username).HasMaxLength(50);
            entity.Property(e => e.EmailDk)
                .HasMaxLength(50)
                .HasColumnName("EmailDK");
            entity.Property(e => e.LoaiUser).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
        });

        modelBuilder.Entity<ViTriSach>(entity =>
        {
            entity.HasKey(e => e.MaSach).HasName("PK__Ke__8582FBBE67250FFE");

            entity.ToTable("ViTriSach");

            entity.Property(e => e.MaSach).HasMaxLength(50);
            entity.Property(e => e.Hang).HasMaxLength(50);
            entity.Property(e => e.Ke)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.MaSachNavigation).WithOne(p => p.ViTriSach)
                .HasForeignKey<ViTriSach>(d => d.MaSach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ViTriSach_Sach");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
