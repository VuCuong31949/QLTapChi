//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLTapChi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PhanCong
    {
        public int IDPhanCong { get; set; }
        public System.DateTime NgayPhanCong { get; set; }
        public Nullable<System.DateTime> NgayKetThuc { get; set; }
        public int IDTapChiBaiViet { get; set; }
        public int IDNguoiPhanBien { get; set; }
        public Nullable<int> VongPhanBien { get; set; }
        public Nullable<int> TrangThaiPhanBien { get; set; }
    
        public virtual NguoiDung NguoiDung { get; set; }
        public virtual TapChiBaiViet TapChiBaiViet { get; set; }
    }
}
