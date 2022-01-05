//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class KhachHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhachHang()
        {
            this.KhachDuLiches = new HashSet<KhachDuLich>();
            this.KhachHangThanThiets = new HashSet<KhachHangThanThiet>();
        }
    
        public int MaKH { get; set; }
        public string Hoten { get; set; }
        public string GioiTinh { get; set; }
        public Nullable<int> Tuoi { get; set; }
        public string CMND_Passport { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
        public Nullable<int> MaLoaiKhach { get; set; }
        public Nullable<System.DateTime> HanVisa { get; set; }
        public Nullable<System.DateTime> HanPassort { get; set; }
        public Nullable<int> SoLanDiTour { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KhachDuLich> KhachDuLiches { get; set; }
        public virtual LoaiKhach LoaiKhach { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KhachHangThanThiet> KhachHangThanThiets { get; set; }
    }
}
