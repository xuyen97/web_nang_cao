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
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống !")]
        [Display(Name = "Tên khách hàng")]
        public string Ten_Khach_Hang { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống !")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống !")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày lập")]
        public DateTime Ngay_Lap { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống !")]
        [Display(Name = "Tổng giá")]
        public double Tong_Don_Gia { get; set; }
        [Display(Name = "Tour được chọn")]
        public int Tour_Id { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống !")]
        [Display(Name = "Số lượng người tham gia")]
        public int SoLuong { get; set; }
        [Display(Name ="Trạng thái")]
        public string Trang_Thai { get; set; }
        public virtual Tour Tour { get; set; }
    }
}