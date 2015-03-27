using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Abstractions;
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

            if (files.Any())
            {
                foreach (var file in files)
                {
                    Debug.Print("Fichero: {0} en directorio: {1}", file.FileName, file.DirectoryName);
                }
            }
            else
            {
                Debug.Print("Files not found.");
            }


        }
    }
}
