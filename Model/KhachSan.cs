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
    
    public partial class KhachSan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhachSan()
        {
            this.DSKhachSans = new HashSet<DSKhachSan>();
        }
    
        public int MaKS { get; set; }
        public string TenKS { get; set; }
        public string DiaChi { get; set; }
        public Nullable<int> SDT { get; set; }
        public Nullable<int> ChiPhi { get; set; }
        public Nullable<int> MaKhuVuc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DSKhachSan> DSKhachSans { get; set; }
        public virtual KhuVuc KhuVuc { get; set; }
    }
}
