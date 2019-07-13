using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_Tour_Du_Lich.Models
{
    public class Nguoi_Dung
    {
        [HiddenInput(DisplayValue =false)]
        public int Id { get; set; }
        [Required(ErrorMessage ="Vui lòng không để trống !")]
        [Display(Name ="Họ đệm")]
        public string Ho_Dem { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống !")]
        [Display(Name = "Tên")]
        public string Ten { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống !")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Số điện thoại")]
        public string Sdt { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống !")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Địa chỉ email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống !")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Ngày sinh")]
        public DateTime Ngay_Sinh { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống !")]
        [Display(Name = "Địa chỉ")]
        public string Dia_Chi { get; set; }
        [HiddenInput(DisplayValue =false)]
        public int Loai_Nguoi_Dung_Id { get; set; }
        public virtual Loai_Nguoi_Dung Loai_Nguoi_Dung{get;set;}
    }
}