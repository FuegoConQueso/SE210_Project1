using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE210_Project1
{
    internal struct PageTableEntry
    {
        public int page;
        public int frame;

        internal PageTableEntry(int page, int frame)
        {
            this.page = page;
            this.frame = frame;
        }
    }
}
