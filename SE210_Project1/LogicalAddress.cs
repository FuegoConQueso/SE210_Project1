using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE210_Project1
{
    public class LogicalAddress
    {
        public int page;
        public int offset;

        public LogicalAddress(int address)
        {
            int pagesize = Processor.PageSize;
            page = address / pagesize;
            offset = address % pagesize;
        }
    }
}
