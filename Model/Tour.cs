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
    
    public partial class Tour
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tour()
        {
            this.DoanDuLiches = new HashSet<DoanDuLich>();
            this.DSDiaDiems = new HashSet<DSDiaDiem>();
        }
    
        public int MaTour { get; set; }
        public string TenTour { get; set; }
        public Nullable<int> MaLoaiTour { get; set; }
        public Nullable<double> GiaTour { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DoanDuLich> DoanDuLiches { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DSDiaDiem> DSDiaDiems { get; set; }
        public virtual LoaiTour LoaiTour { get; set; }
    }
}
