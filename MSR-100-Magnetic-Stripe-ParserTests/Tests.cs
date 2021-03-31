using NUnit.Framework;

namespace MSR_100_Magnetic_Stripe_ParserTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CheckPAN()
        {
            var data = "%B5168755544412233^PKMMV/UNEMBOXXXX       ^1807111100000000000000111000000?;5168755544412233=18071111000011100000?";
            var result = Repos.MSR100Controller.MSR100Controller.ParseData(data);
            Assert.AreEqual(result.Track1.PAN, "5168755544412233");
        }

        [Test]
        public void CheckLongTrack1()
        {
            var data = "%B5168755544412233^PKMMV/UNEMBOXXXXUNEMBOXXXXUNEMBOXXXXUNEMBOXXXXUNEMBOXXXX^1807111100000000000000111000000?;5168755544412233=18071111000011100000?";
            var result = Repos.MSR100Controller.MSR100Controller.ParseData(data);
            Assert.AreEqual(result.Track1.PAN, null);
        }
    }
}