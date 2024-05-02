using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHousingApi.Entities.Base;

namespace WareHousingApi.Entities.Entities
{
    public class Setting_Tbl : FieldPublicInherits
    {
        [Key]
        public int Id { get; set; }
        public string MySetting { get; set; }
    }
}
