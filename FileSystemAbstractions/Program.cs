using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAbstractions
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileLocator = new FileLocator(@"D:\Ocio", "The*", ".avi");
            var files = fileLocator.GetSearchResult();

            foreach (var file in files)
            {
                Debug.Print("Fichero: {0} en directorio: {1}", file.FileName, file.DirectoryName);
            }
        }
    }
}
