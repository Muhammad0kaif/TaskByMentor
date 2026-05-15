using System;
using System.Collections.Generic;
using System.Text;

namespace PocoClasses.Dto
{
    public class PagedResult<T>
    {
        public int TotalCount { get; set; }

        public List<T> Items { get; set; }
    }
}
