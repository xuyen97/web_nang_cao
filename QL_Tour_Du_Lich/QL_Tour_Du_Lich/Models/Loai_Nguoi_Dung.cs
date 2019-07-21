using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_Tour_Du_Lich.Models
{
    public class Loai_Nguoi_Dung
    {
        [HiddenInput(DisplayValue =false)]
        [Key]
        public int Loai_Nguoi_Dung_Id { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống !")]
        [Display(Name = "Tên loại")]
        public string Ten_Loai { get; set; }
        public ICollection<Tai_Khoan> TaiKhoans { get; set; }
    }
}