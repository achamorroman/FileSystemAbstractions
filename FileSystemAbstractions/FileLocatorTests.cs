using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace FileSystemAbstractions
{
    public class FileLocatorTests
    {

        [TestFixtureSetUp]
        public void SetUp()
        {
        }

        [TestFixtureTearDown]
        public void TearDown() { }

        private static IFileSystem GetMockedFileSystem()
        {
            return new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    { @"c:\myfile.txt", new MockFileData("Testing is meh.") },
                    { @"c:\demo\jQuery.js", new MockFileData("some js") },
                    { @"c:\demo\document.doc", new MockFileData("document text") },
                    { @"c:\demo\ExcelSheet.xls", new MockFileData("excel data") },
                    { @"c:\demo\otherDemo\MyOwn.js", new MockFileData("more js") }
                });
        }

        [Test]
        [ExpectedException("System.ArgumentException")]
        public void When_rootFolderNotExists_Expected_ArgumentException()
        {
            //Arrange
            const string notExistingDirectory = "C:\notExistingDirectory";
            var fileSystem = GetMockedFileSystem();

            //Act - Assert
            var sut = new FileLocator(fileSystem, notExistingDirectory, "*.*", ".txt");
        }

        [Test]
        public void When_rootFolderExists_Expected_NewFileLocator()
        {
            //Arrange
            const string searchRootFolder = @"C:\Demo";
            var fileSystem = GetMockedFileSystem();

            //Act
            var sut = new FileLocator(fileSystem, searchRootFolder, "*.*", ".txt");

            //Assert
            Assert.IsNotNull(sut);
        }

        [Test]
        public void When_FilesFound_Expected_ResultsWithMyFileInfo()
        {
            //Arrange
            const string searchRootFolder = @"C:\demo";
            var fileSystem = GetMockedFileSystem();
            var sut = new FileLocator(fileSystem, searchRootFolder, "*.*", ".js");
            const int expectedFiles = 2;

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
            const string searchRootFolder = @"C:\demo";
            var fileSystem = GetMockedFileSystem();
            var sut = new FileLocator(fileSystem, searchRootFolder, "*.*", ".dat");

            //Act
            var files = sut.GetSearchResult();

            //Assert
            Assert.IsEmpty(files);
        }
    }
}
