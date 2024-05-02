using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHousingApi.Entities.Models
{
    public class ReminderViewModel
    {
        public int ReminderID { get; set; }


        [Display(Name = "عنوان یادآوری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} وارد نشده است.")]
        public string ReminderTitle { get; set; }


        [Display(Name = "متن یادآوری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} وارد نشده است.")]
        public string ReminderContent { get; set; }

        public DateTime ReminderCreateDate { get; set; }

        [Display(Name = "تاریخ یادآوری")]
        public string ReminderDate { get; set; }
        [Display(Name = "وضعیت خواندن")]
        public bool IsRead { get; set; }
        public string UserID { get; set; }
    }
    public class RemindereditViewModel
    {
        public int reminderIDE { get; set; }


        [Display(Name = "عنوان یادآوری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} وارد نشده است.")]
        public string ReminderTitleE { get; set; }


        [Display(Name = "متن یادآوری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} وارد نشده است.")]
        public string ReminderContentE { get; set; }

        public string createDateTimeE { get; set; }

        [Display(Name = "تاریخ یادآوری")]
        public string ReminderDateE { get; set; }
        [Display(Name = "وضعیت خواندن")]
        public bool isReadE { get; set; }
        public string UserIDE { get; set; }
    }
}
