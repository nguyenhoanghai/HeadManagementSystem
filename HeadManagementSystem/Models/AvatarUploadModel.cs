using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HeadManagementSystem.Models
{
    public class AvatarUploadModel
    {
        [DataType(DataType.Upload)] 
        [Required(ErrorMessage = "Vui lòng chọn ảnh.")]
        public string file { get; set; }
    }
}