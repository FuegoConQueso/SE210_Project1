using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SE210_Project1
{
    internal class FileManager
    {
        string addressFileName;
        string vmFileName;
        string logFileName;

        public FileManager(string addressFileName, string vmFileName, string logFileName)
        {
            File.Delete(logFileName);
            this.addressFileName = addressFileName;
            this.vmFileName = vmFileName;
            this.logFileName = logFileName;
        }

        public List<int> GetAddresses()
        {
            List<int> result = new List<int>();
            using (var file = File.OpenText(addressFileName))
            {
                while (!file.EndOfStream)
                {
                    result.Add(int.Parse(file.ReadLine()));
                }
            }
            return result;
        }

        //public void LogTranslation(int virtualAddress, int physicalAddress, sbyte value)
        //{
        //    using (var file = File.AppendText(logFileName))
        //    {
        //        file.WriteLine(String.Format("Virtual address: {0} Physical address: {1} Value: {2}",
        //            virtualAddress, physicalAddress, value));
        //    }
        //}

        public void Log(string log)
        {
            File.AppendAllText(logFileName, log);
        }

        public sbyte[] ReadPage(int page)
        {
            sbyte[] result; 
            int offset = page * Processor.PageSize;
            using (var file = File.OpenRead(vmFileName))
            {
                var buffer = new byte[Processor.PageSize];
                file.Seek(offset, SeekOrigin.Begin);
                file.Read(buffer, 0, Processor.PageSize);
                result = buffer.Select<byte, sbyte>(o => (sbyte)o).ToArray();
            }
            return result;
        }
    }
}
