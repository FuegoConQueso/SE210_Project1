using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE210_Project1
{
    class Program
    {
        static void Main(string[] args)
        {
            int pageSize = 256;
            int numOfAddresses = 2000;
            int numOfFrames = 256;
            int numOfPages = 1024;

            string vmFile = "vmFile";
            string addressFile = "AddressInputs.txt";
            string logFile = "Output.txt";

            //Generates a new file representing the virtual memory of the process
            FileGenerator.GenerateVMFile(numOfPages, pageSize, vmFile);

            //Generates a new file representing the addresses referenced by the process
            FileGenerator.GenerateAddressFile(numOfPages, pageSize, numOfAddresses, addressFile);

            var processor = new Processor(numOfPages, numOfFrames, vmFile, addressFile, logFile, pageSize);
            processor.TranslateAll();
        }
    }
}
