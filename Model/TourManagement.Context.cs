﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tour_management.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TourEntities : DbContext
    {
        public TourEntities()
            : base("name=TourEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DiaDiem> DiaDiems { get; set; }
        public virtual DbSet<DoanDuLich> DoanDuLiches { get; set; }
        public virtual DbSet<DSDiaDiem> DSDiaDiems { get; set; }
        public virtual DbSet<DSKhachSan> DSKhachSans { get; set; }
        public virtual DbSet<DSNhanVien> DSNhanViens { get; set; }
        public virtual DbSet<DSPhuongTien> DSPhuongTiens { get; set; }
        public virtual DbSet<KhachDuLich> KhachDuLiches { get; set; }
        public virtual DbSet<KhachHang> KhachHangs { get; set; }
        public virtual DbSet<KhachSan> KhachSans { get; set; }
        public virtual DbSet<KhuVuc> KhuVucs { get; set; }
        public virtual DbSet<LoaiKhach> LoaiKhaches { get; set; }
        public virtual DbSet<LoaiTour> LoaiTours { get; set; }
        public virtual DbSet<LoaiUser> LoaiUsers { get; set; }
        public virtual DbSet<NhanVien> NhanViens { get; set; }
        public virtual DbSet<PhuongTien> PhuongTiens { get; set; }
        public virtual DbSet<Tour> Tours { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
