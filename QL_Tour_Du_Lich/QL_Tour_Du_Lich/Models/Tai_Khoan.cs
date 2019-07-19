using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_Tour_Du_Lich.Models
{
    public class Tai_Khoan
    {
        [HiddenInput(DisplayValue =false)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống !")]
        [Display(Name = "Tên đăng nhập")]
        public string Ten_Dang_Nhap { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống !")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Mat_Khau { get; set; }
        [HiddenInput(DisplayValue =false)]
        public int Loai_Nguoi_Dung_Id { get; set; }
        public virtual Loai_Nguoi_Dung Loai_Nguoi_Dung { get; set; }
    }
}