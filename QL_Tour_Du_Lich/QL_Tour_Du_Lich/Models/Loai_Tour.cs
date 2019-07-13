using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_Tour_Du_Lich.Models
{
    public class Loai_Tour
    {
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int Loai_Tour_Id { get; set; }
        [Required(ErrorMessage ="Vui lòng không đẻ trống !")]
        [Display(Name ="Tên loại tour")]
        public string Ten_Loai_Tour { get; set; }
        [Display(Name ="Ghi chú")]
        [DataType(DataType.MultilineText)]
        public string Ghi_Chu { get; set; }
        public ICollection<Tour> Tours { get; set; }
    }
}