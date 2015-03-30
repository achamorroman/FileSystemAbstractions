using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.Cryptography;

namespace FileSystemAbstractions
{
    public class FileLocator
    {
        private readonly string _searchDirectory;
        private readonly string _searchPattern;
        private readonly string _fileExtension;
        private readonly IFileSystem _fileSystem;

        public FileLocator(IFileSystem fileSystem, String searchDirectory, string searchPattern, string fileExtension)
        {
            if (fileSystem == null) throw new ArgumentNullException("fileSystem");
            if (string.IsNullOrEmpty(searchDirectory)) throw new ArgumentException("searchDirectory");
            if (string.IsNullOrEmpty(searchPattern)) throw new ArgumentException("searchString");
            if (string.IsNullOrEmpty(fileExtension)) throw new ArgumentException("fileExtension");

            if (!fileSystem.Directory.Exists(searchDirectory))
                throw new ArgumentException("Root folder doesn't exists", "searchDirectory");

            _searchDirectory = Path.GetFullPath(searchDirectory);
            _searchPattern = searchPattern;
            _fileExtension = fileExtension;
            _fileSystem = fileSystem;
        }

        public FileLocator(String searchDirectory, string searchPattern, string fileExtension)
            : this(new FileSystem(), searchDirectory, searchPattern, fileExtension) { }

        public IEnumerable<MyFileInfo> GetSearchResult()
        {
            var searchDirectoryInfo = _fileSystem.DirectoryInfo.FromDirectoryName(_searchDirectory);
            var matchingFiles = GetMatchingFiles(searchDirectoryInfo);

            if (!matchingFiles.Any()) return new List<MyFileInfo>();

            var files = mapToMyFiles(matchingFiles);
            return files;
        }

        private static List<MyFileInfo> mapToMyFiles(IEnumerable<FileInfoBase> matchingFiles)
        {
            var myFiles = (from file in matchingFiles
                           select new MyFileInfo()
                           {
                               FileName = file.Name,
                               DirectoryName = file.DirectoryName
                           }).ToList();

            return myFiles;
        }

        private IEnumerable<FileInfoBase> GetMatchingFiles(DirectoryInfoBase searchDirectoryInfo)
        {
            var fileList = searchDirectoryInfo.GetFiles(_searchPattern, SearchOption.AllDirectories);
            var matchingFiles = from file in fileList
                                where file.Extension == _fileExtension
                                select file;

            return matchingFiles;
        }
    }
}
