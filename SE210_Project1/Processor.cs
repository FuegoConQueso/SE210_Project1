using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace SE210_Project1
{
    public class Processor
    {
        Translator translator;
        sbyte[] MainMemory;
        FileManager fileManager;
        string logs;
        public static int PageSize { get; set; }

        public Processor(int numOfPages, int numOfFrames,
                            string vmFile, string addressFile, string logFile,
                            int pageSize = 256, int pageTableSize = 256, int tlbSize = 16)
        {
            PageSize = pageSize;
            MainMemory = new sbyte[numOfFrames * PageSize];
            translator = new Translator(numOfPages, this, pageSize, pageTableSize, tlbSize);
            fileManager = new FileManager(addressFile, vmFile, logFile);
            logs = "";
        }

        public sbyte GetValue(int address)
        {
            int physicalAddress = translator.Translate(address);
            sbyte value = MainMemory[physicalAddress];
            logs += String.Format("Virtual address: {0} Physical address: {1} Value: {2}{3}",
                     address, physicalAddress, value, Environment.NewLine);
            //fileManager.LogTranslation(address, physicalAddress, value);
            return value;
        }

        /// <summary>
        /// loads a page from the virtual memory into the indicated frame
        /// </summary>
        /// <param name="sourcePage"></param>
        /// <param name="destinationFrame"></param>
        public void LoadPage(int sourcePage, int destinationFrame)
        {
            sbyte[] page = fileManager.ReadPage(sourcePage);
            int offset = destinationFrame * PageSize;
            page.CopyTo(MainMemory, offset);
        }

        public void TranslateAll()
        {
            List<int> addresses = fileManager.GetAddresses();
            foreach(var address in addresses)
            {
                GetValue(address);
            }
            int TotalLookups, PageFaults, TLBHits;
            translator.Tally(out TotalLookups, out PageFaults, out TLBHits);
            logs += String.Format("Number of Translated Addresses = {0}{1}" +
                                  "Page Faults = {2}{1}" +
                                  "Page Fault Rate = {3}{1}" +
                                  "TLB Hits = {4}{1}" +
                                  "TLB Hit Rate = {5}", TotalLookups, Environment.NewLine,
                                  PageFaults, (float)(PageFaults)/TotalLookups,
                                  TLBHits, (float)(TLBHits)/TotalLookups);
            Debug.WriteLine(String.Format("Number of Translated Addresses = {0}{1}" +
                                  "Page Faults = {2}{1}" +
                                  "Page Fault Rate = {3}{1}" +
                                  "TLB Hits = {4}{1}" +
                                  "TLB Hit Rate = {5}", TotalLookups, Environment.NewLine,
                                  PageFaults, (float)(PageFaults) / TotalLookups,
                                  TLBHits, (float)(TLBHits) / TotalLookups));
            fileManager.Log(logs);

        }
    }
}
