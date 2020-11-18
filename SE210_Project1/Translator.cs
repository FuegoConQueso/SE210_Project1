using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE210_Project1
{
    public class Translator
    {
        PageTable tlb;
        PageTable pageTable;
        Processor processor;
        int PageFaults = 0;
        int TLBHits = 0;
        int TotalLookups = 0;
        int NumOfPages { get; set; }
        int PageSize { get; set; }

        public Translator(int numOfPages, Processor processor, int pageSize = 256, int pageTableSize = 256, int tlbSize = 16)
        {
            tlb = new PageTable(tlbSize);
            pageTable = new PageTable(pageTableSize);
            PageSize = pageSize;
            NumOfPages = numOfPages;
            this.processor = processor;
        }

        public int Translate(int address)
        {
            if (!IsLegalAddress(address))
            {
                throw new Exception("Memory access error: address " + address + " is not valid");
            }

            TotalLookups++;
            LogicalAddress logicalAddress = new LogicalAddress(address);

            int frame = tlb.Lookup(logicalAddress.page);
            //tlb hit
            if (frame != -1)
            {
                TLBHits++;
                return BuildPhysicalAddress(frame, logicalAddress.offset);
            }

            frame = pageTable.Lookup(logicalAddress.page);
            //page table hit
            if (frame != -1)
            {
                tlb.AddEntry(new PageTableEntry(logicalAddress.page, frame));
                return BuildPhysicalAddress(frame, logicalAddress.offset);
            }

            //page fault
            PageFaults++;
            frame = pageTable.AddEntry(logicalAddress.page);
            processor.LoadPage(logicalAddress.page, frame);
            tlb.AddEntry(new PageTableEntry(logicalAddress.page, frame));
            return BuildPhysicalAddress(frame, logicalAddress.offset);
        }

        private bool IsLegalAddress(int address)
        {
            if (address >= 0 && address < PageSize * NumOfPages)
            {
                return true;
            }
            return false;
        }

        private int BuildPhysicalAddress(int frame, int offset)
        {
            int address = frame * PageSize + offset;
            return address;
            
        }

        public void Tally(out int TotalLookups, out int PageFaults, out int TLBHits)
        {
            TotalLookups = this.TotalLookups;
            PageFaults = this.PageFaults;
            TLBHits = this.TLBHits;
        }
    }
}
