using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHousingApi.Entities.Base;

namespace WareHousingApi.Entities.Entities
{
    public class Reminder_Tbl: FieldPublicInherits
    {
        [Key]
        public int ReminderID { get; set; }
        public string ReminderTtile { get; set; }
        public string ReminderContent { get; set; }
        public DateTime ReminderDate { get; set; }
        public bool IsRead { get; set; }
    }
}
