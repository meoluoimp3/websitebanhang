//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebsiteBanHang.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class billproduct
    {
        public int Id { get; set; }
        public Nullable<int> product_id { get; set; }
        public Nullable<int> quantity { get; set; }
        public Nullable<int> unit_price { get; set; }
        public Nullable<int> bill_id { get; set; }
    
        public virtual Bill Bill { get; set; }
        public virtual product product { get; set; }
    }
}
