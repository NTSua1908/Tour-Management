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
    
    public partial class DoanDuLich
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DoanDuLich()
        {
            this.DSKhachSans = new HashSet<DSKhachSan>();
            this.DSNhanViens = new HashSet<DSNhanVien>();
            this.DSPhuongTiens = new HashSet<DSPhuongTien>();
            this.KhachDuLiches = new HashSet<KhachDuLich>();
        }
    
        public int MaDoan { get; set; }
        public string TenDoan { get; set; }
        public Nullable<int> MaTour { get; set; }
        public Nullable<System.DateTime> NgayKhoiHanh { get; set; }
        public Nullable<System.DateTime> NgayKetThuc { get; set; }
        public Nullable<int> SoLuong { get; set; }
        public string ChiTiet { get; set; }
        public Nullable<decimal> TongGiaPT { get; set; }
        public Nullable<decimal> TongGiaKS { get; set; }
        public Nullable<decimal> TongGiaAU { get; set; }
        public Nullable<decimal> ChiPhiKhac { get; set; }
    
        public virtual Tour Tour { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DSKhachSan> DSKhachSans { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DSNhanVien> DSNhanViens { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DSPhuongTien> DSPhuongTiens { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KhachDuLich> KhachDuLiches { get; set; }
    }
}
