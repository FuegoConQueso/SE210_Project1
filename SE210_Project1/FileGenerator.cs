using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SE210_Project1
{
    public static class FileGenerator
    {
        public static void GenerateVMFile(int numOfPages, int pageSize, string path)
        {
            Random rand = new Random();
            int filesize = numOfPages * pageSize;
            using (FileStream vmFile = File.Open(path, FileMode.Create))
            {
                byte[] buffer = new byte[filesize];
                rand.NextBytes(buffer);
                vmFile.Write(buffer, 0, filesize);
            }
        }
        public static void GenerateAddressFile(int numOfPages, int pageSize, int numOfAddresses, string path)
        {
            Random rand = new Random();
            int filesize = numOfPages * pageSize;
            List<string> addresses = new List<string>();
            for (int i = 0; i < numOfAddresses; i++)
            {
                addresses.Add(rand.Next(filesize).ToString());
            }
            File.WriteAllLines(path, addresses);
        }
    }
}
