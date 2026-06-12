using FishingContest;
using System.Globalization;
using static FishingContest.Program;

namespace FishingContestTest
{
    [TestClass]
    public sealed class SearchTest
    {
        [TestMethod]
        public void FileEmpty()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-Us");
            Infile f = new Infile("input2.txt");
            bool found = RunSearch(f, out bool l, out string? name);

            Assert.IsFalse(l);
        }

        [TestMethod] // Nincs talalat
        public void NoSearchResult()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-Us");
            Infile f = new Infile("input3.txt");
            bool found = RunSearch(f, out bool l, out string? name);

            Assert.IsFalse(l);
        }

        [TestMethod] // Van talalat
        public void CorrectSearchResult()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-Us");
            Infile f = new Infile("input1.txt");
            bool found = RunSearch(f, out bool l, out string? name);

            Assert.IsTrue(l);
            Assert.AreEqual("János", name);
        }
    }
}
