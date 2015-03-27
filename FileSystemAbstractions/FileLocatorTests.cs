using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace FileSystemAbstractions
{
    public class FileLocatorTests
    {

        [TestFixtureSetUp]
        public void SetUp() { }

        [TestFixtureTearDown]
        public void TearDown() { }

        [Test]
        [ExpectedException("System.ArgumentException")]
        public void When_rootFolderNotExists_Expected_ArgumentException()
        {
            //Arrange
            const string notExistingDirectory = "C:\notExistingDirectory";


            //Act - Assert
            var sut = new FileLocator(notExistingDirectory, "*.*", ".txt");
        }

        [Test]
        public void When_rootFolderExists_Expected_NewFileLocator()
        {
            //Arrange
            const string viewsRootFolder = @"C:\Temp";

            //Act
            var sut = new FileLocator(viewsRootFolder, "*.*", ".txt");

            //Assert
            Assert.IsNotNull(sut);
        }
    }
}
