using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreRelation.Models
{
    public class Comment
    {
        public long ID { get; set; } 

        public string Message { get; set; }

        public Article Article { get; set; }
    }
}
