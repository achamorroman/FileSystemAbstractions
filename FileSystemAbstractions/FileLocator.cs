using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSystemAbstractions
{
    public class FileLocator
    {
        private readonly string _searchDirectory;
        private readonly string _searchPattern;
        private readonly string _fileExtension;

        public FileLocator(String searchDirectory, string searchPattern, string fileExtension)
        {
            if (String.IsNullOrEmpty(searchDirectory)) throw new ArgumentNullException("searchDirectory");
            if (string.IsNullOrEmpty(searchPattern)) throw new ArgumentException("searchString");
            if (string.IsNullOrEmpty(fileExtension)) throw new ArgumentException("fileExtension");

            if (!Directory.Exists(searchDirectory))
                throw new ArgumentException("Root folder doesn't exists", "searchDirectory");

            _searchDirectory = Path.GetFullPath(searchDirectory);
            _searchPattern = searchPattern;
            _fileExtension = fileExtension;
        }

        public IEnumerable<MyFile> GetSearchResult()
        {
            var searchDirectoryInfo = new DirectoryInfo(_searchDirectory);
            var matchingFiles = GetMatchingFiles(searchDirectoryInfo);


            if (!matchingFiles.Any()) return new List<MyFile>();

            var files = new List<MyFile>();

            foreach (var file in matchingFiles)
            {
                files.Add(new MyFile() { FileName = file.Name, DirectoryName = file.DirectoryName });
            }
            return files;
        }

        private IEnumerable<FileInfo> GetMatchingFiles(DirectoryInfo viewsRootDirectoryInfo)
        {
            var fileList = viewsRootDirectoryInfo.GetFiles(_searchPattern, SearchOption.AllDirectories);
            var matchingFiles = from file in fileList
                                where file.Extension == _fileExtension
                                select file;

            return matchingFiles;
        }
    }

    public class MyFile
    {
        public String FileName { get; set; }
        public String DirectoryName { get; set; }
    }

}
