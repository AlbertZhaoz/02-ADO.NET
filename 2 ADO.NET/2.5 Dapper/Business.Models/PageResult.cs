using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class PageResult<T>
    {
        public PageResult()
        { }

        public PageResult(int pageIndex,int pageSize)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long TotalCount { get; set; }

        public List<T> DataList { get; set; }
    }
}
