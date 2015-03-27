using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

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
            if (String.IsNullOrEmpty(searchDirectory)) throw new ArgumentNullException("searchDirectory");
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
            var files = new List<MyFileInfo>();
            foreach (var file in matchingFiles)
            {
                files.Add(new MyFileInfo() { FileName = file.Name, DirectoryName = file.DirectoryName });
            }
            return files;
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
