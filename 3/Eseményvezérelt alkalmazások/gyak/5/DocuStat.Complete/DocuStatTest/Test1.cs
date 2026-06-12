using ELTE.DocuStat.Model;
using ELTE.DocuStat.Persistence;
using Moq;

namespace DocuStatTest
{
    [TestClass]
    public sealed class Test1
    {
        private Mock<IFileManager> _mockFileManager = null!;
        private IDocumentStatistics _documentStatistics = null!;

        [TestInitialize]
        public void Initialize()
        {
            _mockFileManager = new Mock<IFileManager>();
            _documentStatistics = new DocumentStatistics(_mockFileManager.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void TestEmptyString()
        {
            _mockFileManager.Setup(c => c.Load()).Returns(string.Empty);
            _documentStatistics.Load();
            Assert.AreEqual(_documentStatistics.FileContent, " ");
            Assert.IsTrue(_documentStatistics.FileContent == string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(FileManagerException))]
        public void TestException()
        {
            _mockFileManager.Setup(c => c.Load()).Throws<FileManagerException>();
            _documentStatistics.Load();
        }

        [DataTestMethod]
        [DataRow("a", "b")]
        public void TestDataRow(string a, string b)
        {
            Assert.AreEqual("a", a);
        }

        [TestMethod]
        public void TestEvent()
        {
            bool isFileLoaded = false;

            _mockFileManager.Setup(c => c.Load()).Returns("test");
            _documentStatistics.FileContentReady += (sender, args) => { isFileLoaded = true; };
            _documentStatistics.Load();

            Assert.IsTrue(isFileLoaded);
        }
    }
}
