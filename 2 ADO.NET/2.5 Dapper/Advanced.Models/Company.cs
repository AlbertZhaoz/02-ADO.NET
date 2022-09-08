using Dapper.Extension;
using System;
using System.Collections.Generic;
 

namespace Advanced.Models
{
   [Table("Company")] 
    public class CompanyInfo : BaseModel
    { 
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreatorId { get; set; }
        public int? LastModifierId { get; set; }
        public DateTime? LastModifyTime { get; set; }

        //public virtual List<SysUser> SysUsers { get; set; }

    }
}