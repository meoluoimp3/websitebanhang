using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebsiteBanHang.Models
{
    [MetadataTypeAttribute(typeof(UserMetadata))]
    public partial class user
    {
        internal sealed class UserMetadata
        {
            public int Id { get; set; }
            [DisplayName("Địa chỉ")]
            [Required(ErrorMessage ="không được bỏ trống")]

            public string address { get; set; }
            [DisplayName("Tuổi")]
            public Nullable<int> age { get; set; }
            [DisplayName("Email")]
            [Required(ErrorMessage = "không được bỏ trống")]
            [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email không hợp lệ")]
            public string email { get; set; }
            [DisplayName("Giới tính")]
            public Nullable<bool> gender { get; set; }
            [DisplayName("Họ tên")]
            [Required(ErrorMessage = "không được bỏ trống")]
            [StringLength(70, MinimumLength = 5, ErrorMessage = "độ dài không hợp lệ")]
            public string name { get; set; }
            [DisplayName("Tên đăng nhập")]
            [Required(ErrorMessage = "không được bỏ trống")]
            [StringLength(50, MinimumLength = 5, ErrorMessage = "độ dài không hợp lệ")]
            public string username { get; set; }
            [DisplayName("Mật Khẩu")]
            [Required(ErrorMessage = "không được bỏ trống")]
            [StringLength(50, MinimumLength = 8, ErrorMessage = "độ dài không hợp lệ")]
            public string password { get; set; }
           
            [DisplayName("Số điện thoại")]
            [Required(ErrorMessage = "không được bỏ trống")]
            [StringLength(20, MinimumLength = 10, ErrorMessage = "độ dài không hợp lệ")]
            
            public string phone { get; set; }
            


        }
    }
}