using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_Tour_Du_Lich.Models
{
    public class Hoa_Don
    {
        [HiddenInput(DisplayValue =false)]
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Ngày lập")]
        public DateTime Ngay_Lap { get; set; }
        [Required]
        [Display(Name ="Đơn giá")]
        public double Don_Gia { get; set; }
    }
}