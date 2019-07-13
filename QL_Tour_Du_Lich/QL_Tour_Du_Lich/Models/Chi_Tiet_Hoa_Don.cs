using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_Tour_Du_Lich.Models
{
    public class Chi_Tiet_Hoa_Don
    {
        [HiddenInput(DisplayValue =false)]
        public int Id { get; set; }
        [Required(ErrorMessage ="Vui lòng không để trống !")]
        [Display(Name ="Tên khách hàng")]
        public string Ten_Khach_Hang { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name ="Ngày lập")]
        public DateTime Ngay_Lap { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống !")]
        [Display(Name = "Tổng giá")]
        public double Tong_Don_Gia { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int Tour_Id { get; set; }
        public virtual Hoa_Don Hoa_Don { get; set; }
        public virtual Tour Tour { get; set; }
    }
}