using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE210_Project1
{
    internal class PageTable
    {
        private int MaxSize;
        private List<int> AccessList;
        private PageTableEntry[] table;

        public PageTable(int maxSize)
        {
            MaxSize = maxSize;
            AccessList = new List<int>(MaxSize);

            table = new PageTableEntry[MaxSize];
            for(int i = 0; i < maxSize; i++)
            {
                table[i].page = -1;
                table[i].frame = i;
            }
        }

        public int Lookup(int pageNumber)
        {
            for(int i = 0; i < MaxSize; i++)
            {
                if (table[i].page == pageNumber)
                {
                    UpdateAccessList(i);
                    return table[i].frame;
                }
            }
            return -1;
        }

        public void AddEntry(PageTableEntry entry)
        {
            int entryNumber = AddToAccessList();
            table[entryNumber] = entry;
        }

        public int AddEntry(int pageNumber)
        {
            int entryNumber = AddToAccessList();
            int frame = table[entryNumber].frame;
            table[entryNumber] = new PageTableEntry(pageNumber, frame);
            return frame;
        }

        /// <summary>
        /// reorders the access list (adds existing item)
        /// </summary>
        /// <param name="entryNumber"></param>
        private void UpdateAccessList(int entryNumber)
        {
            if (AccessList.Remove(entryNumber))
            {
                AccessList.Add(entryNumber);
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Adds a new item to the access list
        /// </summary>
        /// <param name="entryNumber"></param>
        /// <returns>the table index of the entry to replace</returns>
        private int AddToAccessList()
        {
            int location;
            if (AccessList.Count < MaxSize)
            {
                location = AccessList.Count;
                AccessList.Add(location);
                return location;
            }
            else
            {
                location = AccessList[0];
                AccessList.RemoveAt(0);
                AccessList.Add(location);
                return location;
            }
        }

        public override string ToString()
        {
            string result = WriteTable();
            result += Environment.NewLine + WriteAccessList();
            return result;
        }

        private string WriteTable()
        {
            string result = "::Table::" + Environment.NewLine;
            foreach (var entry in table)
            {
                result += String.Format("page {0} || frame {1}", entry.page, entry.frame) + Environment.NewLine;
            }
            return result;
        }
        private string WriteAccessList()
        {
            string result = "::AccessQueue::" + Environment.NewLine;
            result += "<=Front ";
            foreach (var entry in AccessList)
            {
                result += entry + " ";
            }
            result += "Back=>";
            return result;
        }
    }
}
