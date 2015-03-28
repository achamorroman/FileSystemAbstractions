using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using NUnit.Framework;

namespace FileSystemAbstractions
{
    public class FileLocatorTests
    {
        private const string AllFilesSearchPattern = "*.*";
        private const string RootFolder = @"C:\root";
        private IFileSystem _mockedFileSystem;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _mockedFileSystem = GetMockedFileSystem();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
        }

        [Test]
        public void When_FilesFound_Expected_ResultsWithMyFileInfo()
        {
            //Arrange
            const string fileExtension = ".js";
            const int expectedFiles = 2;
            var sut = new FileLocator(_mockedFileSystem, RootFolder, AllFilesSearchPattern, fileExtension);

            //Act
            var files = sut.GetSearchResult();

            //Assert
            Assert.AreEqual(expectedFiles, files.Count());
            Assert.IsInstanceOf<MyFileInfo>(files.First());
        }

        [Test]
        public void When_SearchPatternProvidedAndExistsCoincidence_Expected_ResultsWithMyFileInfo()
        {
            //Arrange
            const string fileExtension = ".doc";
            const string searchPattern = "*doc*";
            const int expectedFiles = 2;

            var sut = new FileLocator(_mockedFileSystem, RootFolder, searchPattern, fileExtension);

            //Act
            var files = sut.GetSearchResult();

            //Assert
            Assert.AreEqual(expectedFiles, files.Count());
            Assert.IsInstanceOf<MyFileInfo>(files.First());
        }


        [Test]
        public void When_FilesNotFound_Expected_EmptyResults()
        {
            //Arrange
            const string fileExtension = ".dat";
            var sut = new FileLocator(_mockedFileSystem, RootFolder, AllFilesSearchPattern, fileExtension);

            //Act
            IEnumerable<MyFileInfo> files = sut.GetSearchResult();

            //Assert
            Assert.IsEmpty(files);
        }

        [Test]
        public void When_rootFolderExists_Expected_NewFileLocator()
        {
            //Arrange
            const string fileExtension = ".txt";

            //Act
            var sut = new FileLocator(_mockedFileSystem, RootFolder, AllFilesSearchPattern, fileExtension);

            //Assert
            Assert.IsNotNull(sut);
            Assert.IsInstanceOf<FileLocator>(sut);
        }

        [Test]
        [ExpectedException("System.ArgumentException")]
        public void When_rootFolderNotExists_Expected_ArgumentException()
        {
            //Arrange
            const string notExistingFolder = "C:\notExistingFolder";
            const string fileExtension = ".txt";

            //Act - Assert
            var sut = new FileLocator(_mockedFileSystem, notExistingFolder, AllFilesSearchPattern, fileExtension);
        }

        private static IFileSystem GetMockedFileSystem()
        {
            return new MockFileSystem(new Dictionary<string, MockFileData>
                                      {
                                          {@"c:\myfile.txt", new MockFileData("Testing is meh.")},
                                          {@"c:\root\jQuery.js", new MockFileData("some js")},
                                          {@"c:\root\document.doc", new MockFileData("document text")},
                                          {@"c:\root\excelSheet.xls", new MockFileData("excel data")},
                                          {@"c:\root\folder1\myDoc.js", new MockFileData("more js")},
                                          {@"c:\root\folder2\otherdoc.doc", new MockFileData("more js")}
                                      });
        }
    }
}